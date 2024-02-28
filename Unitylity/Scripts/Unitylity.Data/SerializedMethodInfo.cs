
namespace Unitylity.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEngine;

	[Serializable]
	public class SerializedMethodInfo : SerializedMemberInfo<MethodInfo> {

		public MethodInfo methodInfo => base.memberInfo;

		protected virtual bool omitReturnType => false;

		protected override void Update() {
			base.Update();
			if (String.IsNullOrEmpty(_memberName) || _type == null) _memberInfo = null;
			else _memberInfo = String.IsNullOrEmpty(_name) ? null : GetMethod();

			MethodInfo GetMethod() {
				var type = Type.GetType(_name);
				if (type == null) return null;
				var current = type;
				do {
					var methods = current.GetMethods(
						BindingFlags.Static |
						BindingFlags.Public |
						BindingFlags.NonPublic |
						BindingFlags.Instance |
						BindingFlags.DeclaredOnly
					);
					var result = methods.FirstOrDefault(v => FormatMethodName(v) == _memberName);
					if (result != null) return result;
					current = (current.IsGenericType && !current.IsGenericTypeDefinition) ? current.GetGenericTypeDefinition() : current.BaseType;
				} while (current != null);
				return null;
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
		public string FormatMethodName(MethodInfo methodInfo) {
			var paramsString = String.Join(", ", methodInfo.GetParameters().Select(p => p.IsOptional ? $"[{p.ParameterType.Name}]" : p.ParameterType.Name).ToArray());
			var returnTypeName = methodInfo.ReturnType.Name;
			var returnTypeString = omitReturnType ? "" : $"{returnTypeName} ";

			if (methodInfo.IsGenericMethod) {
				var typeParamsString = String.Join(", ", methodInfo.GetGenericArguments().Select(g => g.Name).ToArray());

				return $"{returnTypeString}{methodInfo.Name}<{typeParamsString}>({paramsString})";
			}

			return $"{returnTypeString}{methodInfo.Name}({paramsString})";
		}
	}

}


#if UNITY_EDITOR
namespace Unitylity.Data.Editor {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;
	using static Unitylity.Editor.EditorUtil;
	using static Unitylity.Editor.PropertyUtil;

	[CanEditMultipleObjects]
	[CustomPropertyDrawer(typeof(SerializedMethodInfo), true)]
	public class SerializedMethodInfoDrawer : SerializedMemberInfoDrawer<SerializedMethodInfo> {

	}

}
#endif