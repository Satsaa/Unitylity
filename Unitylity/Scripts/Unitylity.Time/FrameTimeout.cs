
namespace Unitylity.Time {

	using System;

	using UnityEngine;

	/// <summary>
	/// A single-use frame based timer which can be used after a duration passes.
	/// </summary>
	[Serializable]
	public class FrameTimeout {

		[SerializeField]
		internal int start;
		private int pauseAdjustedStart => paused ? start + Time.frameCount - pauseTime : start;

		/// <summary>
		/// Frame after start when there is one remaining use.
		/// </summary>
		public int delay {
			get => _delay;
			set {
				if (paused) {
					start = pauseAdjustedStart;
					pauseTime = Time.frameCount;
				}
				if (usable) start -= value - _delay;
				_delay = value;
			}
		}
		[SerializeField]
		internal int _delay = 0;

		/// <summary>
		/// Whether this FrameTimeout has been used.
		/// </summary>
		[field: SerializeField]
		public bool used { get; private set; }

		/// <summary>
		/// Whether this FrameTimeout can currently be used.
		/// </summary>
		public bool usable => !paused && !used && Time.frameCount >= pauseAdjustedStart + delay;

		/// <summary>
		/// Whether the specified delay has passed.
		/// </summary>
		public bool expired => Time.frameCount >= pauseAdjustedStart + delay;

		/// <summary>
		/// Whether this Timeout is paused.
		/// </summary>
		public bool paused {
			get => _paused;
			set {
				if (_paused == value) return;
				if (value) {
					pauseTime = Time.frameCount;
				} else {
					start = pauseAdjustedStart;
				}
				_paused = value;
			}
		}
		[SerializeField]
		internal bool _paused;
		[SerializeField]
		internal int pauseTime;


		FrameTimeout() { }

		/// <summary>
		/// Creates a single-use frame based timer which can be used after `delay` passes.
		/// </summary>
		/// <param name="delay">Frames until this FrameTimeout can be used in milliseconds.</param>
		/// <param name="paused">Whether this FrameTimeout will be created in a paused state.</param>
		public FrameTimeout(int delay, bool paused = false) {
			try {
				// Throws if scripting API is unavailable
				start = Time.frameCount;
				this.paused = paused;
			} catch (UnityException) {
				_paused = paused;
			}

			_delay = delay;
		}


		/// <summary>
		/// If the FrameTimeout has a remaining use, returns true and consumes the use, otherwise returns false.
		/// </summary>
		/// <returns></returns>
		public bool Use() {
			if (usable) {
				used = true;
				return true;
			}
			return false;
		}


		/// <summary>
		/// FrameTimeout is reset as if it had just begun.
		/// </summary>
		public bool Reset() {
			used = false;
			pauseTime = start = Time.frameCount;
			return false;
		}

		/// <summary>
		/// FrameTimeout is reset as if it had just begun.
		/// </summary>
		/// <param name="pause">Pause the FrameTimeout?</param>
		public bool Reset(bool pause) {
			used = false;
			pauseTime = start = Time.frameCount;
			paused = pause;
			return false;
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Time.Editor {

	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(FrameTimeout))]
	internal class FrameTimeoutDrawer : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			using (new EditorGUI.PropertyScope(position, label, property)) {

				var delay = property.FindPropertyRelative(nameof(FrameTimeout._delay));
				var paused = property.FindPropertyRelative(nameof(FrameTimeout._paused));

				var noLabel = label.text is "" && label.image is null;

				// Pause bool (Click handling)
				var pausedRect = new Rect(position);
				if (!noLabel) pausedRect.xMin = pausedRect.xMin + EditorGUIUtility.labelWidth - 15 * (EditorGUI.indentLevel + 1);
				pausedRect.width = 15;
				var inActive = EditorGUI.Toggle(pausedRect, !paused.boolValue);
				var inPaused = !inActive;
				// Handle playmode fingering of pause
				if (inPaused != paused.boolValue) {
					if (Application.isPlaying) {
						var pauseTime = property.FindPropertyRelative(nameof(FrameTimeout.pauseTime));
						var start = property.FindPropertyRelative(nameof(FrameTimeout.start));
						if (inPaused) {
							pauseTime.intValue = Time.frameCount;
						} else {
							start.intValue += Time.frameCount - pauseTime.intValue;
						}
					}
					paused.boolValue = inPaused;
				}

				// Delay value
				var delayRect = new Rect(position);
				if (noLabel) delayRect.xMin = pausedRect.xMax + 2;
				var inDelay = Mathf.Max(0, EditorGUI.IntField(delayRect, label, delay.intValue));
				if (inDelay != delay.intValue && inDelay > 0) {
					if (Application.isPlaying) {
						var field = fieldInfo.GetValue(property.serializedObject.targetObject);
						if (field is FrameTimeout target) target.delay = inDelay;
					}
					delay.intValue = inDelay;
				}

				// Pause bool (Press down visuals)
				EditorGUI.Toggle(pausedRect, inActive);

			}
		}

	}

}
#endif