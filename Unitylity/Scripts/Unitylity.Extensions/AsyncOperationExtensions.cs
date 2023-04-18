
namespace Unitylity.Extensions {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using UnityEngine;

	public static class AsyncOperationExtensions {

		public static ProgressTask MakeTask(this AsyncOperation asyncOp) {
			var tcs = new TaskCompletionSource<AsyncOperation>();
			asyncOp.completed += operation => { tcs.SetResult(operation); };
			return new(tcs.Task, asyncOp);
		}

	}

	/// <summary>
	/// Task and AsyncOperation.
	/// </summary>
	public class ProgressTask : IDisposable {

		public ProgressTask(Task<AsyncOperation> task, AsyncOperation asyncOp) {
			this.task = task;
			this.asyncOp = asyncOp;
		}

		public float progress => asyncOp.progress;
		public Task<AsyncOperation> task { get; }
		public AsyncOperation asyncOp { get; }

		public void Dispose() {
			task.Dispose();
		}

	}

}