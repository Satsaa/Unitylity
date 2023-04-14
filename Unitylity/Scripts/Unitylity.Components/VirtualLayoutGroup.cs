
namespace Unitylity.Components {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.Pool;
	using UnityEngine.UI;
	using Unitylity.Components.Extended;
	using Unitylity.Extensions;
	using Object = UnityEngine.Object;

#if UNITYLITY_GENERAL_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/General/" + nameof(VirtualLayoutGroup))]
#endif
	[DefaultExecutionOrder(5)]
	public class VirtualLayoutGroup : ExtendedUIBehaviour {

		public enum Direction { Right, Left, Up, Down, }

		public abstract class Item : ScriptableObject {

			public VirtualLayoutGroup group;
			public float size;

			public abstract void Position(float position);
			public abstract void Offset(float offset, bool animate = false);
			public abstract void Show();
			public abstract void Hide();
			public abstract void Remove();
			public virtual void Update() { }
			public virtual void Init() { }

			protected void ConfigureUpdates(bool enable) => group.ConfigureUpdates(this, enable);

			public void SetSize(float size) {
				var oldSize = this.size;
				this.size = size;
				group.UpdateSize(this, oldSize);
			}

		}

		public class PrefabItem : Item {

			public int creationIndex;
			public bool calculateSize;
			public bool createOnInit;
			public RectTransform prefab;
			public RectTransform item;

			[field: SerializeField] public bool destroy { get; private set; }

			public override void Init() {
				base.Init();
				if (calculateSize) {
					var prefabSize = prefab.rect.size;
					if (group.IsVert()) {
						size = group.rectTransform.rect.width * prefabSize.y / prefabSize.x;
					} else {
						size = group.rectTransform.rect.height * prefabSize.x / prefabSize.y;
					}
				}
				if (createOnInit) {
					if (!item) {
						item = Instantiate(prefab, group.transform);
						group.SetItemRect(item, size);
					}
					item.gameObject.SetActive(false);
				}
			}

			public override void Position(float position) {
				if (item) group.SetPos(item, position);
			}
			public override void Offset(float offset, bool animate) {
				if (item) group.OffsetPos(item, offset);
			}
			public override void Show() {
				if (!item) {
					item = Instantiate(prefab, group.transform);
					group.SetItemRect(item, size);
				}
				item.gameObject.SetActive(true);
			}
			public override void Remove() {
				if (item) Destroy(item.gameObject);
				item = null;
			}
			public override void Hide() {
				if (item) {
					if (destroy) {
						if (item) Destroy(item.gameObject);
						item = null;
					} else {
						item.gameObject.SetActive(false);
					}
				}
			}

		}

		public class AnimatedItem : PrefabItem {

			[Serializable]
			protected class Animation {
				public float prevEval = 0f;
				public float time = 0f;
				public float offset;
			}

			[SerializeField] protected float animationTargetPosition;
			[SerializeField] protected List<Animation> animations = new();
			[SerializeField] protected bool hidden;

			public override void Show() {
				hidden = false;
				base.Show();
			}

			public override void Hide() {
				hidden = true;
				if (!animating) {
					base.Hide();
				}
			}

			public override void Position(float position) {
				if (animating) {
					var offset = position - animationTargetPosition;
					Offset(offset, true);
				} else {
					base.Position(position);
				}
			}

			public override void Offset(float offset, bool animate) {
				if (item) {
					if (animate && item.gameObject.activeSelf) {
						if (!animating) {
							item.SetAsLastSibling();
							ConfigureUpdates(true);
							animationTargetPosition = group.Pos(item);
						}
						animations.Add(new() { offset = offset });
						animationTargetPosition += offset;
					} else {
						base.Offset(offset, animate);
						animationTargetPosition += offset;
					}
				}
			}

			public override void Update() {
				for (int i = 0; i < animations.Count; i++) {
					var animation = animations[i];
					animation.time = Mathf.Min(1, animation.time + Time.deltaTime / group.animationDuration);
					var eval = group.animationCurve.Evaluate(animation.time);
					var evalChange = eval - animation.prevEval;
					animation.prevEval = eval;
					group.OffsetPos(item, evalChange * animation.offset);
					if (animation.time >= 1) {
						animations.RemoveAt(i);
						i--;
					}
				}
				if (!animating) {
					if (hidden) Hide();
					ConfigureUpdates(false);
				}
			}

			public bool animating => animations.Any();

			protected void StopAnimating() {
				if (animating) {
					if (hidden) Hide();
					animations.Clear();
					ConfigureUpdates(false);
				}
			}

		}

		[field: SerializeField] public Direction direction { get; private set; }
		public bool expandToViewSize = true;
		public float animationDuration = 1;
		public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		public RectTransform testPrefab;
		public IReadOnlyList<Item> listItems => items;

		private HashSet<Item> updateds;
		[field: SerializeField, HideInInspector] protected List<Item> items;

		[field: SerializeField, HideInInspector] protected float position;
		[field: SerializeField, HideInInspector] protected float oldPosition;
		[field: SerializeField, HideInInspector] protected float viewSize;

		[field: SerializeField, HideInInspector] protected float firstPos;
		[field: SerializeField, HideInInspector] protected int firstVisible;
		[field: SerializeField, HideInInspector] protected float lastPos;
		[field: SerializeField, HideInInspector] protected int lastVisible = -1;

		[field: SerializeField, HideInInspector] public bool dirty;
		[field: SerializeField, HideInInspector] private float _size;
		public float size {
			get => _size;
			protected set {
				_size = value;
				UpdateRectSize();
			}
		}

		protected void UpdateRectSize() {
			var size = expandToViewSize ? Mathf.Max(viewSize, _size) : _size;
			rectTransform.SetSizeWithCurrentAnchors(IsVert() ? RectTransform.Axis.Vertical : RectTransform.Axis.Horizontal, size);
		}

		protected override void Awake() {
			base.Awake();
			rectTransform.pivot = direction switch {
				Direction.Right => rectTransform.pivot.SetX(0),
				Direction.Left => rectTransform.pivot.SetX(1),
				Direction.Up => rectTransform.pivot.SetY(0),
				Direction.Down => rectTransform.pivot.SetY(1),
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
			UpdateViewSize();
			UpdateRectSize();
		}

		private List<Item> clone;
		private bool cloneReady;
		protected virtual void Update() {
			if (updateds == null || !updateds.Any()) return;

			if (!cloneReady) {
				cloneReady = true;
				clone ??= new();
				int cur = clone.Count;
				int target = updateds.Count;
				if (target < cur) clone.RemoveRange(target, cur - target);
				else if (target > cur) clone.AddRange(Enumerable.Repeat(default(Item), target - cur));

				int i = 0;
				foreach (var updated in updateds) {
					clone[i] = updated;
					i++;
				}
			}

			foreach (var updated in clone) {
				updated.Update();
			}
		}

		// Call manually from e.g. scroll rect
		public void Refresh() => Refresh(default);
		protected void Refresh(float animation) {
			UpdateViewSize();
			var parent = transform.parent as RectTransform;
			position = -Select(transform.localPosition.Add(IsNeg() ? -(Vector2.one - parent.pivot) * parent.rect.size : parent.pivot * parent.rect.size));
			var change = position - oldPosition;
			if ((change == 0 && !dirty) || items.Count == 0) return;
			var up = change >= 0;
			oldPosition = position;

			if (up || dirty) {
				UpCheck();
			}
			if (!up || dirty) {
				DownCheck();
			}

			dirty = false;

			void UpCheck() {
				// Show items that became visible upwards
				if (lastVisible < items.Count) {
					var pos = lastVisible == -1 ? 0 : lastPos + items[lastVisible].size;
					for (int i = lastVisible + 1; i < items.Count; i++) {
						var item = items[i];
						var vis = Visibility(pos, pos + item.size);
						if (vis == 0) {
							item.Show();
							if (animation != 0) {
								item.Position(pos + animation);
								item.Offset(-animation, true);
							} else {
								item.Position(pos);
							}
							lastVisible = i;
							lastPos = pos;
						} else if (vis == 1) {
							break;
						}
						pos += item.size;
					}
				}
				// Hide items that became invisible downwards
				{
					var pos = firstPos;
					for (int i = firstVisible; i < items.Count; i++) {
						var item = items[i];
						var vis = Visibility(pos, pos + item.size);
						if (vis < 0) {
							item.Hide();
							firstVisible = i + 1;
							pos += item.size;
							firstPos = pos;
						} else {
							break;
						}
					}
				}
			}
			void DownCheck() {
				// Show items that became visible downwards
				{
					var pos = firstPos;
					for (int i = firstVisible - 1; i >= 0; i--) {
						var item = items[i];
						pos -= item.size;
						var vis = Visibility(pos, pos + item.size);
						if (vis == 0) {
							item.Show();
							if (animation != 0) {
								item.Position(pos - animation);
								item.Offset(animation, true);
							} else {
								item.Position(pos);
							}
							firstVisible = i;
							firstPos = pos;
						} else if (vis == -1) {
							break;
						}
					}
				}
				// Hide items that became invisible upwards
				{
					var pos = lastPos;
					for (int i = lastVisible; i >= 0; i--) {
						var item = items[i];
						var vis = Visibility(pos, pos + item.size);
						if (vis > 0) {
							item.Hide();
							if (i == 0) {
								lastVisible = -1;
								pos = 0;
							} else {
								lastVisible = i - 1;
								pos -= items[i - 1].size;
							}
							lastPos = pos;
						} else {
							break;
						}
					}
				}
			}
		}

		public void UpdateSize(Item item, float oldSize) {
			for (int i = 0; i < lastVisible; i++) {
				if (items[i] == item) {
					UpdateSize(i, oldSize);
					return;
				}
			}

		}
		public void UpdateSize(int index, float oldSize) {
			var diff = items[index].size - oldSize;
			if (index < lastVisible) {
				lastPos += diff;
			}
			if (index < firstVisible) {
				firstPos += diff;
			}
			for (int i = Mathf.Max(index + 1, firstVisible); i <= lastVisible; i++) {
				items[i].Offset(diff, index >= firstVisible);
			}
			size += diff;
			if (index >= firstVisible) {
				dirty = true;
				Refresh(-diff);
			} else {
				OffsetPos(rectTransform, -diff);
			}
		}

		public bool UpdateViewSize() {
			var prev = viewSize;
			viewSize = Mathf.Abs(Select((transform.parent.transform as RectTransform).rect.size));
			dirty |= prev != viewSize;
			var res = prev != viewSize;
			if (res) UpdateRectSize();
			return res;
		}

		internal void ConfigureUpdates(Item item, bool enabled) {
			updateds ??= new();
			if (enabled ? updateds.Add(item) : updateds.Remove(item)) cloneReady = false;
		}

		public virtual void Add(Item item) {
			item.group = this;
			item.Init();
			items.Add(item);
			size += item.size;
			dirty = true;
			Refresh();
		}

		public virtual void MoveItem(int from, int to) {
			if (from < 0 || from > items.Count) throw new IndexOutOfRangeException();
			if (to < 0 || to > items.Count) throw new IndexOutOfRangeException();
			if (from == to) return;

			var item = items[from];

			var fromPos = 0f;
			if (from >= firstVisible) {
				fromPos = firstPos;
				for (int i = firstVisible; i < items.Count; i++) {
					var other = items[i];
					if (i == from) {
						break;
					}
					fromPos += other.size;
				}
			} else {
				fromPos = firstPos;
				for (int i = firstVisible; i >= 0; i--) {
					var other = items[i];
					fromPos -= other.size;
					if (i == from) {
						break;
					}
				}
			}

			if (from < to) {
				// Move item upstream
				// 3 -> 7
				// 0,1,2,3[4,5,6,7]8,9 <- Offset items
				// 0,1,2,4,5,6,7[3]8,9 Move item to new position
				for (int i = Mathf.Max(from + 1, firstVisible); i <= Mathf.Min(to, lastVisible); i++) {
					items[i].Offset(-item.size, true);
				}

				if (to < lastVisible) {
					// target = lastPos + (lastVisible == to ? item.size : items[lastVisible].size);
					var toPos = lastPos + items[lastVisible].size;
					for (int i = lastVisible; i >= firstVisible; i--) {
						var other = items[i];
						if (i == to) {
							toPos -= item.size;
							break;
						}
						toPos -= other.size;
					}
					item.Show();
					if (from < firstVisible || from > lastVisible) item.Position(fromPos);
					item.Offset(toPos - fromPos, true);
				} else {
					var toPos = lastPos;
					for (int i = lastVisible; i < items.Count; i++) {
						var other = items[i];
						if (i == to) {
							toPos += other.size - item.size;
							break;
						}
						toPos += other.size;
					}
					item.Show();
					item.Position(fromPos);
					item.Offset(toPos - fromPos, true);
					item.Hide();
				}

				if (from < firstVisible && to >= firstVisible) {
					firstPos -= item.size;
					firstVisible--;
				}
				if (from <= lastVisible && to >= lastVisible) {
					lastPos -= from == lastVisible ? lastVisible == 0 ? 0 : items[lastVisible - 1].size : item.size;
					lastVisible--;
				}
			} else {
				// Move item downstream
				// 7 -> 3
				// 0,1,2[3,4,5,6]7,8,9 -> Offset items
				// 0,1,2[7]3,4,5,6,8,9 Move item to new position
				for (int i = Mathf.Max(to, firstVisible); i <= Mathf.Min(from - 1, lastVisible); i++) {
					items[i].Offset(item.size, true);
				}

				if (to >= firstVisible) {
					var toPos = firstPos;
					for (int i = firstVisible; i <= lastVisible; i++) {
						var other = items[i];
						if (i == to) break;
						toPos += other.size;
					}
					item.Show();
					if (from < firstVisible || from > lastVisible) item.Position(fromPos);
					item.Offset(toPos - fromPos, true);
				} else {
					var toPos = firstPos + items[firstVisible].size;
					for (int i = firstVisible; i >= 0; i--) {
						var other = items[i];
						toPos -= other.size;
						if (i == to) {
							break;
						}
					}
					item.Show();
					item.Position(fromPos);
					item.Offset(toPos - fromPos, true);
					item.Hide();
				}

				if (from >= firstVisible && to < firstVisible) {
					firstPos += item.size;
					firstVisible++;
				}
				if (from >= lastVisible && to <= lastVisible) {
					lastPos += item.size;
					lastVisible++;
				}
			}
			items.RemoveAt(from);
			items.Insert(to, item);
			dirty = true;
			Refresh(item.size);
		}

		public virtual void RemoveAt(int index) {
			var item = items[index];
			for (int i = Mathf.Max(index + 1, firstVisible); i <= lastVisible; i++) {
				items[i].Offset(-item.size, true);
			}
			if (index < firstVisible) {
				firstVisible--;
				firstPos -= item.size;
				OffsetPos(rectTransform, item.size);
			}
			if (index < lastVisible) {
				lastVisible--;
				lastPos -= item.size;
			} else if (index == lastVisible) {
				lastVisible--;
				lastPos -= items[index - 1].size;
			}

			item.Remove();
			ConfigureUpdates(item, false);
			items.RemoveAt(index);
			size -= item.size;
			dirty = true;
			Refresh(item.size);
		}

		public virtual void Insert(int index, Item item) {
			if (index < 0 || index > items.Count) throw new IndexOutOfRangeException();
			item.group = this;
			item.Init();
			if (index <= firstVisible && firstVisible != 0) { // Before first visible
				for (int i = firstVisible; i <= lastVisible; i++) {
					items[i].Offset(item.size);
				}
				firstVisible++;
				firstPos += item.size;
				lastVisible++;
				lastPos += item.size;
				items.Insert(index, item);
				size += item.size;
				OffsetPos(rectTransform, -item.size);
				return;
			} else if (index <= lastVisible || firstVisible == 0) { // Within visible area
				for (int i = index; i <= lastVisible; i++) {
					items[i].Offset(item.size, true);
				}
				lastVisible++;
				lastPos += item.size;
				items.Insert(index, item);
				size += item.size;
				var pos = firstPos;
				for (int i = firstVisible; i <= lastVisible; i++) {
					var other = items[i];
					if (i == index) {
						item.Show();
						other.Position(pos);
						break;
					}
					pos += other.size;
				}
				dirty = true;
				Refresh(item.size);
				return;
			}
			items.Insert(index, item);
			size += item.size;
		}

		/// <summary> Returns -1 if before the visible part, 0 if inside, and 1 if after. </summary>
		public virtual int Visibility(float start, float end) {
			if (end < position) return -1;
			if (start > position + viewSize) return 1;
			return 0;
		}

		protected void SetPos(RectTransform rect, float inset) => rect.localPosition = Select(rect.localPosition, inset + Size(rect) * Pivot(rect));
		protected void OffsetPos(RectTransform rect, float offset) => rect.localPosition += ToPos(offset);

		protected float Pivot(RectTransform rt) => Select(rt.pivot);
		protected float Size(RectTransform rt) => Select(rt.sizeDelta);
		protected float Pos(RectTransform rt) => Select(rt.localPosition) - Size(rt) * Pivot(rt);
		protected Vector3 ToPos(float value) => Select(0, value);

		protected void SetItemRect(RectTransform rt, float size) {
			rt.anchorMin = direction switch {
				Direction.Right => new(0, 0),
				Direction.Left => new(1, 0),
				Direction.Up => new(0, 0),
				Direction.Down => new(0, 1),
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
			rt.anchorMax = direction switch {
				Direction.Right => new(0, 1),
				Direction.Left => new(1, 1),
				Direction.Up => new(1, 0),
				Direction.Down => new(1, 1),
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
			rt.sizeDelta = Select(0, size).Abs();
			rt.anchoredPosition = default;
		}

		protected bool IsNeg() {
			return direction switch {
				Direction.Right => false,
				Direction.Left => true,
				Direction.Up => false,
				Direction.Down => true,
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
		}

		public bool IsVert() {
			return direction switch {
				Direction.Right => false,
				Direction.Left => false,
				Direction.Up => true,
				Direction.Down => true,
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
		}

		protected float CondNeg(float value) {
			return IsNeg() ? -value : value;
		}

		protected float Select(Vector2 source) {
			return direction switch {
				Direction.Right => source.x,
				Direction.Left => -source.x,
				Direction.Up => source.y,
				Direction.Down => -source.y,
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
		}

		protected Vector2 Select(Vector2 zero, Vector2 value) {
			return direction switch {
				Direction.Right => new(value.x, zero.y),
				Direction.Left => new(-value.x, zero.y),
				Direction.Up => new(zero.x, value.y),
				Direction.Down => new(zero.x, -value.y),
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
		}

		protected Vector2 Select(float zero, Vector2 value) {
			return direction switch {
				Direction.Right => new(value.x, zero),
				Direction.Left => new(-value.x, zero),
				Direction.Up => new(zero, value.y),
				Direction.Down => new(zero, -value.y),
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
		}

		protected Vector2 Select(Vector2 zero, float value) {
			return direction switch {
				Direction.Right => new(value, zero.y),
				Direction.Left => new(-value, zero.y),
				Direction.Up => new(zero.x, value),
				Direction.Down => new(zero.x, -value),
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
		}

		protected Vector2 Select(float zero, float value) {
			return direction switch {
				Direction.Right => new(value, zero),
				Direction.Left => new(-value, zero),
				Direction.Up => new(zero, value),
				Direction.Down => new(zero, -value),
				_ => throw new InvalidOperationException($"{nameof(direction)} is invalid: {direction}"),
			};
		}

	}

}


#if UNITY_EDITOR
namespace Unitylity.Components.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Components.VirtualLayoutGroup;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;
	using Object = UnityEngine.Object;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(VirtualLayoutGroup), true)]
	public class VirtualLayoutGroup2Editor : Editor {

		VirtualLayoutGroup t => (VirtualLayoutGroup)target;

		SerializedProperty direction;

		void OnEnable() {
			direction = serializedObject.FindProperty(GetBackingFieldName(nameof(VirtualLayoutGroup.direction)));
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			switch ((Direction)direction.intValue) {
				case Direction.Right:
					if (t.rectTransform.pivot.x != 0) EditorGUILayout.HelpBox("Pivot.X will be set to 0.", MessageType.Warning);
					break;
				case Direction.Left:
					if (t.rectTransform.pivot.x != 1) EditorGUILayout.HelpBox("Pivot.X will be set to 1.", MessageType.Warning);
					break;
				case Direction.Up:
					if (t.rectTransform.pivot.y != 0) EditorGUILayout.HelpBox("Pivot.Y will be set to 0.", MessageType.Warning);
					break;
				case Direction.Down:
					if (t.rectTransform.pivot.y != 1) EditorGUILayout.HelpBox("Pivot.Y will be set to 1.", MessageType.Warning);
					break;
			}

			DrawDefaultInspector();

			if (GUILayout.Button("Add 1")) {
				var item = Item.CreateInstance<AnimatedItem>();
				item.prefab = t.testPrefab;
				item.size = UnityEngine.Random.Range(5, 10) * 10f;
				item.creationIndex = t.listItems.Count;
				t.Add(item);
			}
			if (GUILayout.Button("Add 10")) {
				for (int i = 0; i < 10; i++) {
					var item = Item.CreateInstance<AnimatedItem>();
					item.prefab = t.testPrefab;
					item.size = UnityEngine.Random.Range(5, 10) * 10f;
					item.creationIndex = t.listItems.Count;
					t.Add(item);
				}
			}
			if (GUILayout.Button("Insert at 20")) {
				var item = Item.CreateInstance<AnimatedItem>();
				item.prefab = t.testPrefab;
				item.size = UnityEngine.Random.Range(5, 10) * 10f;
				item.creationIndex = -t.listItems.Count;
				t.Insert(20, item);
			}
			if (GUILayout.Button("Remove at 20")) {
				t.RemoveAt(20);
			}
			if (GUILayout.Button("Move 25 to 20")) {
				t.MoveItem(25, 20);
			}
			if (GUILayout.Button("Move 20 to 25")) {
				t.MoveItem(20, 25);
			}
			if (GUILayout.Button("Move 50 to 20")) {
				t.MoveItem(50, 20);
			}
			if (GUILayout.Button("Move 20 to 50")) {
				t.MoveItem(20, 50);
			}
			if (GUILayout.Button("Halve size of 20")) {
				var item = t.listItems[20];
				var oldSize = item.size;
				item.size /= 2;
				t.UpdateSize(20, oldSize);
			}
			if (GUILayout.Button("Double size of 20")) {
				var item = t.listItems[20];
				var oldSize = item.size;
				item.size *= 2;
				t.UpdateSize(20, oldSize);
			}

			serializedObject.ApplyModifiedProperties();
		}

	}

}
#endif