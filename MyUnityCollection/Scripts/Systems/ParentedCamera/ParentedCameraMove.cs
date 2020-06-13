

namespace ParentedCamera {

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  [RequireComponent(typeof(ParentedCamera))]
  public class ParentedCameraMove : MonoBehaviour {
    public float speed = 0.0025f;
    public bool multiplyByZoom = true;
    public KeyCode key = KeyCode.Mouse2;

    ParentedCamera pc;
    Vector2 prev;
    
    void Start() {
      pc = gameObject.GetComponent<ParentedCamera>();
    }

    // Update is called once per frame
    void Update() {
      
      if (Input.GetKeyDown(key)) {
        prev = Input.mousePosition;
      }

      if (Input.GetKey(key)) {
        pc.displacement += pc.transform.right * (prev.x - Input.mousePosition.x) * speed * (multiplyByZoom ? pc.distance : 1);
        pc.displacement += pc.transform.up * (prev.y - Input.mousePosition.y) * speed * (multiplyByZoom ? pc.distance : 1);

        prev = Input.mousePosition;
      }
    }
  }

}