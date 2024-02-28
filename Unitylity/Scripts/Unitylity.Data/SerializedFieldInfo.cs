
namespace Unitylity.Data {

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEngine;

	[Serializable]
	public class SerializedFieldInfo : SerializedMemberInfo<FieldInfo> {

		public FieldInfo fieldInfo => base.memberInfo;

		protected override void Update() {
			base.Update();
			if (String.IsNullOrEmpty(_memberName) || _type == null) _memberInfo = null;
			else _memberInfo = String.IsNullOrEmpty(_name) ? null : Type.GetType(_name)?.GetField(_memberName, bindingFlags);
		}

		public override IEnumerable<FieldInfo> GetValidMembers() {
			if (type == null) return Enumerable.Empty<FieldInfo>();
			return type.GetFields(bindingFlags);
		}

	}

	[Serializable]
	public class SerializedFieldInfo<T> : SerializedFieldInfo {

		protected override void Update() {
			base.Update();
			if (_memberInfo != null && typeof(T).IsAssignableFrom(_memberInfo.FieldType)) {
				_memberInfo = null;
			}
		}

		public override IEnumerable<FieldInfo> GetValidMembers() {
			return base.GetValidMembers().Where(v => typeof(T).IsAssignableFrom(v.FieldType));
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
	[CustomPropertyDrawer(typeof(SerializedFieldInfo), true)]
	public class SerializedFieldInfoDrawer : SerializedMemberInfoDrawer<SerializedFieldInfo> {

	}

}
#endif