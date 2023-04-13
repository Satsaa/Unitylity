
namespace Unitylity.Data.Trees {

	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public partial class Octree<T> {

		public Enumerator GetEnumerator() {
			return new Enumerator(this);
		}

		public DetailedEnumerator GetDetailedEnumerator() {
			return new DetailedEnumerator(this);
		}

		public class Enumerator : ITreeEnumerator<T> {

			public OctreeCell<T> Current => stack.Peek().child;
			public int depth => stack.Count - 1; // First/root cell is 0 debth

			private bool start = true;
			private readonly Stack<Tracker> stack;

			ICell<T> ITreeEnumerator<T>.Current => Current;
			ICell ITreeEnumerator.Current => Current;


			private class Tracker : Tracker<OctreeCell<T>> {
				public Tracker(OctreeCell<T> cell, int i = 0) : base(cell, i) { }
			}


			public Enumerator(OctreeCell<T> cell, int initialStackSize = 32) {
				stack = new Stack<Tracker>(initialStackSize);
				stack.Push(new Tracker(cell));
			}


			public void Reset() {
				while (stack.Count > 1) {
					stack.Pop();
				}
				stack.Peek().i = 0;
			}

			public bool MoveNext() {
			repeat: // Goto here instead of generating stack by calling the method recursively.
				if (start) {
					start = false;
					return true;
				}
				var t = stack.Peek();

				if (t.i > 7) {
					if (stack.Count > 1) {
						stack.Pop();
						goto repeat;
					} else {
						return false;
					}
				} else {
					if (t.child.isLeaf) {
						if (stack.Count > 1) {
							stack.Pop();
							goto repeat;
						} else {
							return false;
						}
					} else {
						stack.Push(new Tracker(t.child.children[t.i]));
						t.i++;
						return true;
					}
				}
			}

			public bool MovePrev() {
			repeat: // Goto here instead of generating stack by calling the method recursively.
				var t = stack.Peek();

				if (t.i < 1) {
					if (stack.Count > 1) {
						stack.Pop();
						goto repeat;
					} else {
						return false;
					}
				} else {
					if (t.child.isLeaf) {
						if (stack.Count > 1) {
							stack.Pop();
							goto repeat;
						} else {
							return false;
						}
					} else {
						t.i--;
						stack.Push(new Tracker(t.child.children[t.i], 8));
						return true;
					}
				}
			}

			public bool MoveUp() {
				if (stack.Count > 1) {
					stack.Pop();
					return true;
				}
				return false;
			}

			public bool MoveDown(int childIndex) {
				var t = stack.Peek();
				if (t.child.isLeaf) {
					return false;
				} else {
					stack.Push(new Tracker(t.child.children[childIndex]));
					return true;
				}
			}

		}

		public class DetailedEnumerator : ITreeEnumerator<T> {

			#region Detail
			public float currentSize => stack.Peek().size;
			public Vector3 currentOrigin => stack.Peek().origin;
			public int index => stack.Peek().i;
			#endregion

			public OctreeCell<T> Current => stack.Peek().child;
			public int depth => stack.Count - 1; // First/root cell is 0 debth

			private bool start = true;
			private readonly Stack<Tracker> stack;

			ICell<T> ITreeEnumerator<T>.Current => Current;
			ICell ITreeEnumerator.Current => Current;


			private class Tracker : Tracker<OctreeCell<T>> {
				public Vector3 origin;
				public float size;
				public Tracker(OctreeCell<T> cell, Vector3 origin, float size, int i = 0) : base(cell, i) {
					this.origin = origin;
					this.size = size;
				}
			}


			public DetailedEnumerator(OctreeCell<T> cell, int initialStackSize = 32) {
				stack = new Stack<Tracker>(initialStackSize);
				stack.Push(new Tracker(cell, Vector3.zero, 1));
			}


			public void Reset() {
				while (stack.Count > 1) {
					stack.Pop();
				}
				stack.Peek().i = 0;
			}

			public bool MoveNext() {
			repeat:
				if (start) {
					start = false;
					return true;
				}
				var t = stack.Peek();

				if (t.i > 7) {
					if (stack.Count > 1) {
						stack.Pop();
						goto repeat;
					} else {
						return false;
					}
				} else {
					if (t.child.isLeaf) {
						if (stack.Count > 1) {
							stack.Pop();
							goto repeat;
						} else {
							return false;
						}
					} else {
						var size = t.size / 2;
						var posIndex = Octree.IndexToSigns(t.i);
						var origin = t.origin + (new Vector3(posIndex.x * size, posIndex.y * size, posIndex.z * size));

						stack.Push(new Tracker(t.child.children[t.i], origin, size));
						t.i++;
						return true;
					}
				}
			}

			public bool MovePrev() {
			repeat:
				var t = stack.Peek();

				if (t.i < 1) {
					if (stack.Count > 1) {
						stack.Pop();
						goto repeat;
					} else {
						return false;
					}
				} else {
					if (t.child.isLeaf) {
						if (stack.Count > 1) {
							stack.Pop();
							goto repeat;
						} else {
							return false;
						}
					} else {
						t.i--;

						var size = t.size / 2;
						var posIndex = Octree.IndexToSigns(t.i);
						var origin = t.origin + (new Vector3(posIndex.x * size, posIndex.y * size, posIndex.z * size));

						stack.Push(new Tracker(t.child.children[t.i], origin, size, 8));
						return true;
					}
				}
			}

			public bool MoveUp() {
				if (stack.Count > 1) {
					stack.Pop();
					return true;
				}
				return false;
			}

			public bool MoveDown(int childIndex) {
				var t = stack.Peek();
				if (t.child.isLeaf) {
					return false;
				} else {
					var size = t.size / 2;
					var posIndex = Octree.IndexToSigns(t.i);
					var origin = t.origin + (new Vector3(posIndex.x * size, posIndex.y * size, posIndex.z * size));

					stack.Push(new Tracker(t.child.children[childIndex], origin, size));
					return true;
				}
			}

		}

	}

}