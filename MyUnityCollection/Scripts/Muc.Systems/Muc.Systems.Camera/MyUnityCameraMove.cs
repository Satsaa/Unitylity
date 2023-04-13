
namespace Muc.Systems.Camera {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Muc.Systems.Camera) + "/" + nameof(MyUnityCameraMove))]
#endif
	[RequireComponent(typeof(MyUnityCamera))]
	public class MyUnityCameraMove : MonoBehaviour {

		[SerializeField, HideInInspector] MyUnityCamera mucam;

		public float speed = 0.0025f;
		public bool multiplyByZoom = true;
		Vector2 prevPos;
		Vector2 pos;
		bool moving;

		void Awake() {
			mucam = gameObject.GetComponent<MyUnityCamera>();
		}

		public virtual void Move(Vector2 delta) {
			if (enabled && moving) {
				mucam.displacement += mucam.transform.right * delta.x * speed * (multiplyByZoom ? mucam.distance : 1);
				mucam.displacement += mucam.transform.up * delta.y * speed * (multiplyByZoom ? mucam.distance : 1);
			}
		}

		public virtual void SetMoving(bool moving) {
			this.moving = moving;
		}

	}

}