
namespace Unitylity.Data.Trees {

	using System.Collections.Generic;

	public interface IBranch {
		IReadOnlyCollection<IBranch> children { get; }
	}

	public interface IBranch<T> : IBranch {
		T data { get; set; }
		new IReadOnlyCollection<IBranch<T>> children { get; }
	}

}