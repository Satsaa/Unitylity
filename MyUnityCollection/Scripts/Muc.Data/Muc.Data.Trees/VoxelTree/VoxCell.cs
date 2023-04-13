
namespace Unitylity.Data.Trees {

	using System.Collections;
	using System.Collections.Generic;


	public class VoxCell : ICell {

		public const int splitCount = 8;
		public bool isLeaf => children is null;
		public VoxCell[] children;

		IReadOnlyList<ICell> ICell.children => children;
		IReadOnlyCollection<IBranch> IBranch.children => children;

		public bool Split() {
			if (!isLeaf) return false;
			children = new VoxCell[splitCount] {
				new VoxCell(),
				new VoxCell(),
				new VoxCell(),
				new VoxCell(),
				new VoxCell(),
				new VoxCell(),
				new VoxCell(),
				new VoxCell(),
			};
			return true;
		}

	}

	public class VoxCell<T> : ICell<T> {

		public const int splitCount = 8;
		public bool isLeaf => children is null;
		public VoxCell<T>[] children;

		public T data { get; set; }

		IReadOnlyList<ICell> ICell.children => children;
		IReadOnlyList<ICell<T>> ICell<T>.children => children;
		IReadOnlyCollection<IBranch> IBranch.children => children;
		IReadOnlyCollection<IBranch<T>> IBranch<T>.children => children;

		public bool Split() {
			if (!isLeaf) return false;
			children = new VoxCell<T>[splitCount] {
				new VoxCell<T>(),
				new VoxCell<T>(),
				new VoxCell<T>(),
				new VoxCell<T>(),
				new VoxCell<T>(),
				new VoxCell<T>(),
				new VoxCell<T>(),
				new VoxCell<T>(),
			};
			return true;
		}

	}

}