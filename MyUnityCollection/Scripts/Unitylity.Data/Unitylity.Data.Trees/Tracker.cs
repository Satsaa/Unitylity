
namespace Unitylity.Data.Trees {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	internal class Tracker<T> {
		public T child;
		public int i;

		public Tracker(T cell, int i = 0) {
			this.child = cell;
			this.i = i;
		}
	}

}