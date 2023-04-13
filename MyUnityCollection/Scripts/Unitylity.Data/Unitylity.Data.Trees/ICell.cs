
namespace Unitylity.Data.Trees {

	using System.Collections.Generic;

	public interface ICell : IBranch {
		new IReadOnlyList<ICell> children { get; }
		bool Split();
	}

	public interface ICell<T> : ICell, IBranch<T> {
		new IReadOnlyList<ICell<T>> children { get; }
	}

}