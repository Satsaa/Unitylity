
namespace Unitylity.Collections.Tests {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Unitylity.Numerics;
	using UnityEngine;

	public class CircularArrayTest : MonoBehaviour {

		public CircularArray<int> circularArray = new(5) { 1, 2, 3, 4, 5 };

		public void Reset() {
			circularArray = new CircularArray<int>(5) { 1, 2, 3, 4, 5 };

			var a = circularArray.ToArray();
			print(String.Join(", ", a));

			circularArray.Resize(10);
			circularArray.Add(6);
			circularArray.Add(7);
			circularArray.Add(8);
			circularArray.Add(9);
			circularArray.Add(10);
			print(String.Join(", ", circularArray));

			circularArray.Resize(5);
			print(String.Join(", ", circularArray));

			circularArray.Resize(55);
			print(String.Join(", ", circularArray));

			circularArray.Resize(24);
			print(String.Join(", ", circularArray));

			circularArray.Resize(6);
			print(String.Join(", ", circularArray));
		}

	}

}