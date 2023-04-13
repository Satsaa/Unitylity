
namespace Unitylity {

	using System;

	/// <summary>
	/// Executes onDispose at the end of the block when used in a using statement.
	/// </summary>
	public readonly struct Deferred : IDisposable {

		private readonly Action onDispose;

		public Deferred(Action onDispose) {
			this.onDispose = onDispose;
		}

		public void Dispose() {
			onDispose?.Invoke();
		}

	}

}