
namespace Unitylity.Data.Trees {

	using System.Collections;
	using System.Collections.Generic;

	public class QuadtreeCell : ICell {

		public const int splitCount = 4;
		public bool isLeaf => children is null;
		public QuadtreeCell[] children;

		IReadOnlyList<ICell> ICell.children => children;
		IReadOnlyCollection<IBranch> IBranch.children => children;

		public bool Split() {
			if (!isLeaf) return false;
			children = new QuadtreeCell[splitCount] {
				new QuadtreeCell(),
				new QuadtreeCell(),
				new QuadtreeCell(),
				new QuadtreeCell(),
			};
			return true;
		}

	}

	public class QuadtreeCell<T> : ICell<T> {

		public const int splitCount = 4;
		public bool isLeaf => children is null;
		public QuadtreeCell<T>[] children;

		public T data { get; set; }

		IReadOnlyList<ICell> ICell.children => children;
		IReadOnlyList<ICell<T>> ICell<T>.children => children;
		IReadOnlyCollection<IBranch> IBranch.children => children;
		IReadOnlyCollection<IBranch<T>> IBranch<T>.children => children;

		public bool Split() {
			if (!isLeaf) return false;
			children = new QuadtreeCell<T>[splitCount] {
				new QuadtreeCell<T>(),
				new QuadtreeCell<T>(),
				new QuadtreeCell<T>(),
				new QuadtreeCell<T>(),
			};
			return true;
		}

	}

}