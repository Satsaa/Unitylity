
namespace Muc.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEngine;

	[Serializable]
	public class SerializedMethodInfo : SerializedMemberInfo<MethodInfo> {

		protected override void Update() {
			base.Update();
			if (String.IsNullOrEmpty(_memberName) || _type == null) _memberInfo = null;
			else _memberInfo = String.IsNullOrEmpty(_name) ? null : GetMethod();

			MethodInfo GetMethod() {
				var type = Type.GetType(_name);
				if (type == null) return default;
				var methods = type.GetMethods(bindingFlags);
				return methods.FirstOrDefault(v => FormatMethodName(v) == _memberName);
			}
		}

		public override IEnumerable<MethodInfo> GetValidMembers() {
			if (type == null) return Enumerable.Empty<MethodInfo>();
			return type.GetMethods(bindingFlags).Where(v => !v.IsSpecialName);
		}

		public override string FormatMemberName(MemberInfo member) {
			return FormatMethodName(member as MethodInfo);
		}

		// ReturnType YourMethod<T1, ..., TN>(Param1Type,...,ParamNType)
		public static string FormatMethodName(MethodInfo methodInfo) {
			var paramsString = String.Join(", ", methodInfo.GetParameters().Select(p => p.ParameterType.Name).ToArray());
			var returnTypeName = methodInfo.ReturnType.Name;

			if (methodInfo.IsGenericMethod) {
				var typeParamsString = String.Join(", ", methodInfo.GetGenericArguments().Select(g => g.Name).ToArray());

				return $"{returnTypeName} {methodInfo.Name}<{typeParamsString}>({paramsString})";
			}

			return $"{returnTypeName} {methodInfo.Name}({paramsString})";
		}
	}


#if UNITY_EDITOR
	namespace Editor {

		using System;
		using System.Collections.Generic;
		using System.Linq;
		using System.Reflection;
		using UnityEditor;
		using UnityEngine;
		using static Muc.Editor.EditorUtil;
		using static Muc.Editor.PropertyUtil;

		[CanEditMultipleObjects]
		[CustomPropertyDrawer(typeof(SerializedMethodInfo), true)]
		public class SerializedMethodInfoDrawer : SerializedMemberInfoDrawer<SerializedMethodInfo> {

		}

	}
#endif
}
