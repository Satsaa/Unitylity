
namespace Muc.Extensions {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class TypeExtensions {

		public static bool IsGenericTypeOf(this Type type, Type genericType) {
			while (type != null && type != typeof(object)) {
				var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
				if (genericType == cur) return true;
				type = type.BaseType;
			}
			return false;
		}

		public static Type GetGenericTypeOf(this Type type, Type genericType, int genericArgumentPosition = 0) {
			while (type != null && type != typeof(object)) {
				var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
				if (genericType == cur) return type.GenericTypeArguments[genericArgumentPosition];
				type = type.BaseType;
			}
			return null;
		}

		public static IEnumerable<Type> BaseTypes(this Type type) {
			while (type != null && type != typeof(object)) {
				type = type.BaseType;
				yield return type;
			}
		}

		public static string GetShortQualifiedName(this Type type) {
			return $"{type.FullName}, {type.Assembly.GetName().Name}";
		}

	}

}