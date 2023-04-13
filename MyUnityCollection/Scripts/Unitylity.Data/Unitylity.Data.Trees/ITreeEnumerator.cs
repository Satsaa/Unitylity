
namespace Unitylity.Data.Trees {

	public interface ITreeEnumerator {
		ICell Current { get; }
		int depth { get; }

		bool MoveNext();
		bool MovePrev();

		bool MoveUp();
		bool MoveDown(int childIndex);
	}

	public interface ITreeEnumerator<T> : ITreeEnumerator {
		new ICell<T> Current { get; }
	}

}