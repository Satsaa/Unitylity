
namespace Unitylity.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEngine;

	[Serializable]
	public class SerializedPropertyInfo : SerializedMemberInfo<PropertyInfo> {

		protected override void Update() {
			base.Update();
			if (String.IsNullOrEmpty(_memberName) || _type == null) _memberInfo = null;
			else _memberInfo = String.IsNullOrEmpty(_name) ? null : Type.GetType(_name)?.GetProperty(_memberName, bindingFlags);
		}

		public override IEnumerable<PropertyInfo> GetValidMembers() {
			if (type == null) return Enumerable.Empty<PropertyInfo>();
			return type.GetProperties(bindingFlags);
		}

	}

	[Serializable]
	public class SerializedPropertyInfo<T> : SerializedPropertyInfo {

		protected override void Update() {
			base.Update();
			if (_memberInfo != null && typeof(T).IsAssignableFrom(_memberInfo.PropertyType)) {
				_memberInfo = null;
			}
		}

		public override IEnumerable<PropertyInfo> GetValidMembers() {
			return base.GetValidMembers().Where(v => typeof(T).IsAssignableFrom(v.PropertyType));
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
	[CustomPropertyDrawer(typeof(SerializedPropertyInfo), true)]
	public class SerializedPropertyInfoDrawer : SerializedMemberInfoDrawer<SerializedPropertyInfo> {

	}

}
#endif