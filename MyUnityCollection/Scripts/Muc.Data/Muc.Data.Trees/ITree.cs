
namespace Unitylity.Data.Trees {

	public interface ITree {
		ITreeEnumerator<ICell> GetEnumerator();
	}

	public interface ITree<T> : ITree {
		new ITreeEnumerator<ICell<T>> GetEnumerator();
	}

}