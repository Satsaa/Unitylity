
namespace Unitylity.Data.Trees {

	using UnityEngine;

	public static class Octree {

		public static Vector3Int IndexToSigns(int i) {
			var pos = Vector3Int.zero;
			if (i >= 4) { i -= 4; pos.z = 1; }
			if (i >= 2) { i -= 2; pos.y = 1; }
			if (i >= 1) { i -= 1; pos.x = 1; }
			return pos;
		}

		public static int SignsToIndex(Vector3 signs) {
			int i = 0;
			if (signs.x >= 0.5f) i += 1;
			if (signs.y >= 0.5f) i += 2;
			if (signs.z >= 0.5f) i += 4;
			return i;
		}

	}

	public partial class Octree<T> : OctreeCell<T>, ITree<T> {

		ITreeEnumerator<ICell> ITree.GetEnumerator() => (ITreeEnumerator<ICell>)GetEnumerator();
		ITreeEnumerator<ICell<T>> ITree<T>.GetEnumerator() => (ITreeEnumerator<ICell<T>>)GetEnumerator();

	}

}