

namespace Muc.Systems.Camera {

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
  [AddComponentMenu("")]
#else
  [AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Camera) + "/" + nameof(MyUnityCameraRotate))]
#endif
  [RequireComponent(typeof(MyUnityCamera))]
  public class MyUnityCameraRotate : MonoBehaviour {
    public float rotSpeed = 1f;
    public KeyCode key = KeyCode.Mouse0;
    public bool useHotkey = false;
    public bool limitVertRot = true;
    public float rotationLimit = 89.9f;

    MyUnityCamera pc;
    float prevX = 0;
    float prevY = 0;
    bool isDragging = false;
    void Start() {
      pc = gameObject.GetComponent<MyUnityCamera>();
    }

    // Update is called once per frame
    void Update() {
      // Drag rotate
      if (useHotkey) {
        if (Input.GetKeyDown(key)) {
          isDragging = true;
          prevX = Input.mousePosition.x;
          prevY = Input.mousePosition.y;
        } else if (Input.GetKeyUp(key)) {
          isDragging = false;
        }
      } else {
        isDragging = true;
      }

      if (isDragging) {
        pc.horRot -= (prevX - Input.mousePosition.x) * rotSpeed;
        pc.verRot += (prevY - Input.mousePosition.y) * rotSpeed;
        if (limitVertRot) {
          pc.verRot = Mathf.Clamp(pc.verRot, -rotationLimit, rotationLimit);
        }
        prevX = Input.mousePosition.x;
        prevY = Input.mousePosition.y;
      }
    }
  }

}