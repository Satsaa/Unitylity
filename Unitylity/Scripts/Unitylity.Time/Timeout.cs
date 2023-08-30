
namespace Unitylity.Time {

	using System;

	using UnityEngine;

	/// <summary>
	/// A single-use timer which can be used after a duration passes.
	/// </summary>
	[Serializable]
	public class Timeout {

		[SerializeField]
		internal float start;
		private float pauseAdjustedStart => paused ? start + Time.time - pauseTime : start;

		/// <summary>
		/// Duration after creation this Timeout can be used.
		/// </summary>
		public float delay {
			get => _delay;
			set {
				if (paused) {
					start = pauseAdjustedStart;
					pauseTime = Time.time;
				}
				if (usable) start -= value - _delay;
				_delay = value;
			}
		}
		[SerializeField]
		internal float _delay = 0;

		/// <summary>
		/// Whether this Timeout has been used.
		/// </summary>
		[field: SerializeField]
		public bool used { get; private set; }

		/// <summary>
		/// Whether this Timeout can currently be used.
		/// </summary>
		public bool usable => !paused && !used && Time.time >= pauseAdjustedStart + delay;

		/// <summary>
		/// Whether the specified delay has passed.
		/// </summary>
		public bool expired => Time.time >= pauseAdjustedStart + delay;

		/// <summary>
		/// Whether this Timeout is paused.
		/// </summary>
		public bool paused {
			get => _paused;
			set {
				if (_paused == value) return;
				if (value) {
					pauseTime = Time.time;
				} else {
					start = pauseAdjustedStart;
				}
				_paused = value;
			}
		}
		[SerializeField]
		internal bool _paused;
		[SerializeField]
		internal float pauseTime;


		Timeout() { }

		/// <summary>
		/// Creates a single-use timer which can be used after `delay` passes.
		/// </summary>
		/// <param name="delay">Time until this Timeout can be used in seconds.</param>
		/// <param name="paused">Whether this Timeout will be created in a paused state.</param>
		public Timeout(float delay, bool paused = false) {
			try {
				// Throws if scripting API is unavailable
				start = Time.time;
				this.paused = paused;
			} catch (UnityException) {
				_paused = paused;
			}

			_delay = delay;
		}


		/// <summary>
		/// If the Timeout has a remaining use, returns true and consumes the use, otherwise returns false.
		/// </summary>
		public bool Use() {
			if (usable) {
				used = true;
				return true;
			}
			return false;
		}


		/// <summary>
		/// Set the Timeout as expired.
		/// </summary>
		public bool Expire() {
			start = Time.time - delay;
			Debug.Assert(expired);
			return false;
		}

		/// <summary>
		/// Timeout is reset as if it had just begun.
		/// </summary>
		public void Reset() {
			used = false;
			pauseTime = start = Time.time;
		}

		/// <summary>
		/// Timeout is reset as if it had just begun.
		/// </summary>
		/// <param name="pause">Pause the Timeout?</param>
		public void Reset(bool pause) {
			used = false;
			pauseTime = start = Time.time;
			paused = pause;
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Time.Editor {

	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(Timeout))]
	internal class TimeoutDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			using (new EditorGUI.PropertyScope(position, label, property)) {

				var delay = property.FindPropertyRelative(nameof(Timeout._delay));
				var paused = property.FindPropertyRelative(nameof(Timeout._paused));

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
						var pauseTime = property.FindPropertyRelative(nameof(Timeout.pauseTime));
						var start = property.FindPropertyRelative(nameof(Timeout.start));
						if (inPaused) {
							pauseTime.floatValue = Time.time;
						} else {
							start.floatValue += Time.time - pauseTime.floatValue;
						}
					}
					paused.boolValue = inPaused;
				}

				// Delay value
				var delayRect = new Rect(position);
				if (noLabel) delayRect.xMin = pausedRect.xMax + 2;
				var inDelay = Mathf.Max(0, EditorGUI.FloatField(delayRect, label, delay.floatValue));
				if (inDelay != delay.floatValue) {
					if (Application.isPlaying) {
						var field = fieldInfo.GetValue(property.serializedObject.targetObject);
						if (field is Timeout target) target.delay = inDelay;
					}
					delay.floatValue = inDelay;
				}

				// Pause bool (Press down visuals)
				EditorGUI.Toggle(pausedRect, inActive);

			}
		}

	}

}
#endif