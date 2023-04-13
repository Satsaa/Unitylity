
namespace Unitylity {

	using System;
	using UnityEngine;

	public class Stopwatch : IDisposable {

		public Stopwatch(string message = "Time elapsed: {0}") {
			this.message = message;
			sw = new();
			sw.Start();
		}

		public System.Diagnostics.Stopwatch sw { get; private set; }
		public string message { get; private set; }

		public void Dispose() {
			sw.Stop();
			Debug.LogFormat(message, sw.Elapsed);
		}

	}

}