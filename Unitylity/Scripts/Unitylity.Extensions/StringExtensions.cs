
namespace Unitylity.Extensions {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class StringExtensions {

		/// <summary> Capitalize the first character. </summary>
		public static string Capitalize(this string str) {
			if (String.IsNullOrEmpty(str)) return str;
			return str[0].ToString().ToUpper() + str[1..];
		}

	}

}
