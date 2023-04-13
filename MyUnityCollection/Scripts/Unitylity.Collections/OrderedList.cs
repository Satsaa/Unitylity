
namespace Unitylity.Collections {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	/// <summary>
	/// List in which new items are sorted based on a provided Comparison function
	/// </summary>
	public class OrderedList<T> : ICollection<T>, IEnumerable<T>, IList<T>, IReadOnlyCollection<T>, IReadOnlyList<T> {


		#region Implementation

		protected readonly List<T> items;

		protected readonly IComparer<T> comparer;


		public T this[int index] {
			get => items[index];
			set => ReplaceAt(index, value);
		}


		#region Ctors

		public OrderedList(IComparer<T> comparer) {
			this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
			this.items = new List<T>();
		}

		public OrderedList(int capacity, IComparer<T> comparer) {
			this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
			this.items = new List<T>(capacity);
		}

		public OrderedList(IEnumerable<T> items, IComparer<T> comparer) {
			if (items == null) throw new ArgumentNullException(nameof(items));
			this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
			this.items = items.ToList();
		}

		#endregion


		public void Add(T item) {

			int i = 0;
			if (items.Count != 0) {
				while (comparer.Compare(item, items[i]) >= 0 && ++i < items.Count) ;
			}

			items.Insert(i, item);
		}

		/// <summary> Not optimized </summary>
		public void AddRange(IEnumerable<T> collection) {
			foreach (var item in collection)
				Add(item);
		}


		protected void ReplaceAt(int index, T item) {

			int i = 0;
			while (comparer.Compare(item, items[i]) >= 0 && ++i < items.Count) ;

			if (i == index || i == index + 1) {
				items[index] = item;
				return;
			}

			if (i < index) {
				items.RemoveAt(index);
				items[i] = item;
			} else {
				items.Insert(i, item);
				items.RemoveAt(index);
			}
		}


		public void ReSort() => items.Sort(comparer);

		#endregion



		#region Interface implementations

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => items.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

		public void CopyTo(Array array, int index) => ((ICollection)items).CopyTo(array, index);
		bool ICollection<T>.IsReadOnly => ((IList)items).IsReadOnly;
		public void Insert(int index, T item) => ((IList<T>)items).Insert(index, item);

		public int Count => items.Count;

		public OrderedList<T> Sort(int index, int count, IComparer<T> comparer) => new(items.GetRange(index, count), comparer);
		public OrderedList<T> Sort(IComparer<T> comparer) => new(items, comparer);

		public ReadOnlyCollection<T> AsReadOnly() => items.AsReadOnly();
		public int BinarySearch(T item) => items.BinarySearch(item);
		public void Clear() => items.Clear();
		public bool Contains(T item) => items.Contains(item);
		public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => items.ConvertAll<TOutput>(converter);
		public void CopyTo(int index, T[] array, int arrayIndex, int count) => items.CopyTo(index, array, arrayIndex, count);
		public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);
		public void CopyTo(T[] array) => items.CopyTo(array);
		public bool Exists(Predicate<T> match) => items.Exists(match);
		public T Find(Predicate<T> match) => items.Find(match);
		public List<T> FindAll(Predicate<T> match) => items.FindAll(match);
		public int FindIndex(int startIndex, int count, Predicate<T> match) => items.FindIndex(startIndex, count, match);
		public int FindIndex(int startIndex, Predicate<T> match) => items.FindIndex(startIndex, match);
		public int FindIndex(Predicate<T> match) => items.FindIndex(match);
		public T FindLast(Predicate<T> match) => items.FindLast(match);
		public int FindLastIndex(int startIndex, int count, Predicate<T> match) => items.FindLastIndex(startIndex, count, match);
		public int FindLastIndex(int startIndex, Predicate<T> match) => items.FindLastIndex(startIndex, match);
		public int FindLastIndex(Predicate<T> match) => items.FindLastIndex(match);
		public void ForEach(Action<T> action) => items.ForEach(action);
		public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
		public List<T> GetRange(int index, int count) => items.GetRange(index, count);
		public int IndexOf(T item, int index, int count) => items.IndexOf(item, index, count);
		public int IndexOf(T item, int index) => items.IndexOf(item, index);
		public int IndexOf(T item) => items.IndexOf(item);

		public int LastIndexOf(T item) => items.LastIndexOf(item);
		public int LastIndexOf(T item, int index) => items.LastIndexOf(item, index);
		public int LastIndexOf(T item, int index, int count) => items.LastIndexOf(item, index, count);
		public bool Remove(T item) => items.Remove(item);
		public int RemoveAll(Predicate<T> match) => items.RemoveAll(match);
		public void RemoveAt(int index) => items.RemoveAt(index);
		public void RemoveRange(int index, int count) => items.RemoveRange(index, count);

		public T[] ToArray() => items.ToArray();
		public List<T> ToList() => items.ToList();
		public void TrimExcess() => items.TrimExcess();
		public bool TrueForAll(Predicate<T> match) => items.TrueForAll(match);

		#endregion

	}

}