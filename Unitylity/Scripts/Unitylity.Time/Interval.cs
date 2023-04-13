
namespace Unitylity.Time {

	using System;

	using UnityEngine;

	/// <summary>
	/// A repeating timer which accumulates one use after each time a duration passes.
	/// </summary>
	[Serializable]
	public class Interval {

		[SerializeField]
		internal float refTime;
		private float pauseAdjustedRefTime => paused ? refTime + Time.time - pauseTime : refTime;

		/// <summary>
		/// Repeating duration after which a new use becomes available.
		/// </summary>
		public float delay {
			get => _delay;
			set {
				if (delay <= 0) throw new ArgumentOutOfRangeException(nameof(delay), $"Value of {nameof(delay)} must be positive.");

				if (paused) {
					paused = false;
					paused = true;
				}

				// Calculates start as such that the remaining uses are carried over and leftover time gives up to 1 use
				var startAfterUse = refTime + uses * _delay;
				var leftovers = Mathf.Min(Time.time - startAfterUse, value);
				refTime = Time.time - (uses * value + leftovers);

				_delay = value;
			}
		}
		[SerializeField]
		internal float _delay = 1;

		/// <summary>
		/// Amount of times this Interval has been used.
		/// </summary>
		[field: SerializeField]
		public int used { get; private set; }

		/// <summary>
		/// Amount of remaining uses.
		/// </summary>
		public int uses => paused ? 0 : Mathf.FloorToInt((Time.time - pauseAdjustedRefTime) / delay);

		/// <summary>
		/// Whether this Interval is paused.
		/// </summary>
		public bool paused {
			get => _paused;
			set {
				if (_paused == value) return;
				if (value) {
					pauseTime = Time.time;
				} else {
					refTime = pauseAdjustedRefTime;
				}
				_paused = value;
			}
		}
		[SerializeField]
		internal bool _paused;
		[SerializeField]
		internal float pauseTime;


		Interval() { }

		/// <summary>
		/// Creates a repeating timer which can be used after delay has passed.
		/// </summary>
		/// <param name="delay">Duration after which Use can be used once in seconds.</param>
		/// <param name="paused">Whether this Interval will be created in a paused state.</param>
		public Interval(float delay, bool paused = false) {
			try {
				// Throws if scripting API is unavailable
				refTime = Time.time;
				this.paused = paused;
			} catch (UnityException) {
				_paused = paused;
			}

			_delay = delay;
		}


		/// <summary>
		/// If there are remaining uses, returns true and consumes one use, otherwise returns false.
		/// </summary>
		/// <returns>Whether the use was succesful.</returns>
		public bool UseOne() {
			if (uses > 0) {
				used++;
				refTime += delay;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns the amount of remaining uses and uses them.
		/// </summary>
		/// <returns>Amount of consumed uses.</returns>
		public int Use() {
			var uses = this.uses;
			used += uses;
			refTime += delay * uses;
			return uses;
		}

		/// <summary>
		/// Invokes action for each remaining use and depletes the remaining uses.
		/// </summary>
		/// <param name="action">Function invoked for each remaining use.</param>
		public void Use(Action action) {
			var iters = Use();
			for (int i = 0; i < iters; i++) action();
		}


		/// <summary>
		/// Interval is reset as if it had just begun.
		/// </summary>
		public bool Reset() {
			used = 0;
			pauseTime = refTime = Time.time;
			return false;
		}

		/// <summary>
		/// Interval is reset as if it had just begun.
		/// </summary>
		/// <param name="pause">Pause the Interval?</param>
		public bool Reset(bool pause) {
			used = 0;
			pauseTime = refTime = Time.time;
			paused = pause;
			return false;
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Time.Editor {

	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(Interval))]
	internal class IntervalDrawer : PropertyDrawer {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			using (new EditorGUI.PropertyScope(position, label, property)) {

				var delay = property.FindPropertyRelative(nameof(Interval._delay));
				var paused = property.FindPropertyRelative(nameof(Interval._paused));

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
						var pauseTime = property.FindPropertyRelative(nameof(Interval.pauseTime));
						var start = property.FindPropertyRelative(nameof(Interval.refTime));
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
				var inDelay = EditorGUI.FloatField(delayRect, label, delay.floatValue);
				if (inDelay != delay.floatValue && inDelay > 0) {
					if (Application.isPlaying) {
						var field = fieldInfo.GetValue(property.serializedObject.targetObject);
						if (field is Interval target) target.delay = inDelay;
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