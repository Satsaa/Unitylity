
namespace Unitylity.Data.Trees {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	public partial class VoxelTree<T> : VoxCell<T>, ITree<T> {

		public int debth { get; }
		public int length { get; }
		public ulong maxTotal { get; }
		public bool dataIsNullable { get; }

		ITreeEnumerator<ICell> ITree.GetEnumerator() => (ITreeEnumerator<ICell>)GetEnumerator();
		ITreeEnumerator<ICell<T>> ITree<T>.GetEnumerator() => (ITreeEnumerator<ICell<T>>)GetEnumerator();

		public T this[int x, int y, int z] {
			get => Get(x, y, z);
			set => Set(x, y, z, value);
		}
		public T this[Vector3Int v] {
			get => Get(v.x, v.y, v.z);
			set => Set(v.x, v.y, v.z, value);
		}

		public VoxelTree(int debth) {
			if (debth < 0) throw new ArgumentOutOfRangeException(nameof(debth), $"Argument {nameof(debth)} must be non-negative.");
			if (debth >= 31) throw new ArgumentOutOfRangeException(nameof(debth), $"Argument {nameof(debth)} must be less than 31.");
			this.debth = debth;
			this.length = (int)Mathf.Pow(2, debth);
			this.maxTotal = (ulong)this.length * (ulong)this.length * (ulong)this.length;
			this.dataIsNullable = typeof(T).IsClass;
		}

		private T Get(int x, int y, int z) {
			if (x >= length || y >= length || z >= length || x < 0 || y < 0 || z < 0) throw OobError();

			VoxCell<T> e = this;
			var currentLength = length;
			while (currentLength > 1) {
				if (e.isLeaf) return default;
				var index = 0;
				currentLength /= 2;
				if (x > currentLength) { x -= currentLength; index += 1; }
				if (y > currentLength) { y -= currentLength; index += 2; }
				if (z > currentLength) { z -= currentLength; index += 4; }
				e = e.children[index];
			}
			return e.data;
		}

		private void Set(int x, int y, int z, T value) {
			if (x >= length || y >= length || z >= length || x < 0 || y < 0 || z < 0) throw OobError();

			VoxCell<T> e = this;
			var currentLength = length;
			while (currentLength > 1) {
				var index = 0;
				currentLength /= 2;
				if (x > currentLength) { x -= currentLength; index += 1; }
				if (y > currentLength) { y -= currentLength; index += 2; }
				if (z > currentLength) { z -= currentLength; index += 4; }
				e.Split();
				e = e.children[index];
			}
			e.data = value;
		}

		private IndexOutOfRangeException OobError(string x = "x", string y = "y", string z = "z")
			=> new($"One or more of the index arguments {x}, {y} or {z} was outside the bounds of the {nameof(VoxelTree<T>)}");

	}

}