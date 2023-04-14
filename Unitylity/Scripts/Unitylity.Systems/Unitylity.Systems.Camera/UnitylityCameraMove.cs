
namespace Unitylity.Systems.Camera {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

#if (UNITYLITY_HIDE_COMPONENTS || UNITYLITY_HIDE_SYSTEM_COMPONENTS || UNITYLITY_HIDE_SYSTEM_CAMERA)
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Camera) + "/" + nameof(UnitylityCameraMove))]
#endif
	[RequireComponent(typeof(UnitylityCamera))]
	public class UnitylityCameraMove : MonoBehaviour {

		[SerializeField, HideInInspector] UnitylityCamera Unitylityam;

		public float speed = 0.0025f;
		public bool multiplyByZoom = true;
		Vector2 prevPos;
		Vector2 pos;
		bool moving;

		void Awake() {
			Unitylityam = gameObject.GetComponent<UnitylityCamera>();
		}

		public virtual void Move(Vector2 delta) {
			if (enabled && moving) {
				Unitylityam.displacement += Unitylityam.transform.right * delta.x * speed * (multiplyByZoom ? Unitylityam.distance : 1);
				Unitylityam.displacement += Unitylityam.transform.up * delta.y * speed * (multiplyByZoom ? Unitylityam.distance : 1);
			}
		}

		public virtual void SetMoving(bool moving) {
			this.moving = moving;
		}

	}

}