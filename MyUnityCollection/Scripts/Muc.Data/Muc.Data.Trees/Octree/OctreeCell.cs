
namespace Unitylity.Data.Trees {

	using System.Collections;
	using System.Collections.Generic;

	public class OctreeCell : ICell {

		public const int splitCount = 8;
		public bool isLeaf => children is null;
		public OctreeCell[] children;

		IReadOnlyList<ICell> ICell.children => children;
		IReadOnlyCollection<IBranch> IBranch.children => children;

		public bool Split() {
			if (!isLeaf) return false;
			children = new OctreeCell[splitCount] {
				new OctreeCell(),
				new OctreeCell(),
				new OctreeCell(),
				new OctreeCell(),
				new OctreeCell(),
				new OctreeCell(),
				new OctreeCell(),
				new OctreeCell(),
			};
			return true;
		}

	}

	public class OctreeCell<T> : ICell<T> {

		public const int splitCount = 8;
		public bool isLeaf => children is null;
		public OctreeCell<T>[] children;

		public T data { get; set; }

		IReadOnlyList<ICell> ICell.children => children;
		IReadOnlyList<ICell<T>> ICell<T>.children => children;
		IReadOnlyCollection<IBranch> IBranch.children => children;
		IReadOnlyCollection<IBranch<T>> IBranch<T>.children => children;

		public bool Split() {
			if (!isLeaf) return false;
			children = new OctreeCell<T>[splitCount] {
				new OctreeCell<T>(),
				new OctreeCell<T>(),
				new OctreeCell<T>(),
				new OctreeCell<T>(),
				new OctreeCell<T>(),
				new OctreeCell<T>(),
				new OctreeCell<T>(),
				new OctreeCell<T>(),
			};
			return true;
		}

	}

}