

namespace Muc.Data.Trees {

  using System.Collections;
  using System.Collections.Generic;

  #region Branch
  public interface IBranch {
    IReadOnlyCollection<IBranch> children { get; }
  }
  public interface IBranch<T> : IBranch {
    T data { get; set; }
    new IReadOnlyCollection<IBranch<T>> children { get; }
  }
  #endregion

  #region Tree
  public interface ITree {
    ITreeEnumerator<ICell> GetEnumerator();
  }
  public interface ITree<T> : ITree {
    new ITreeEnumerator<ICell<T>> GetEnumerator();
  }
  #endregion

  #region TreeEnumerator
  public interface ITreeEnumerator {
    ICell Current { get; }
    int debth { get; }

    bool MoveNext();
    bool MovePrev();

    bool MoveUp();
    bool MoveDown(int childIndex);
  }
  public interface ITreeEnumerator<T> : ITreeEnumerator {
    new ICell<T> Current { get; }
  }
  #endregion


  #region Cell
  public interface ICell : IBranch {
    new IReadOnlyList<ICell> children { get; }
    bool Split();
  }
  public interface ICell<T> : ICell, IBranch<T> {
    new IReadOnlyList<ICell<T>> children { get; }
  }
  #endregion
}