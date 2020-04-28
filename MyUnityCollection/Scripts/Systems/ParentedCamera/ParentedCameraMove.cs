using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentedCameraMove : MonoBehaviour {
  public float speed = 0.0025f;
  public bool multiplyByZoom = true;
  public KeyCode key = KeyCode.Mouse2;

  ParentedCamera pc;
  float prevX = 0;
  float prevY = 0;
  bool isDragging = false;
  void Start() {
    pc = gameObject.GetComponent<ParentedCamera>();
  }

  // Update is called once per frame
  void Update() {
    // Drag rotate
    if (Input.GetKeyDown(key)) {
      isDragging = true;
      prevX = Input.mousePosition.x;
      prevY = Input.mousePosition.y;
    } else if (Input.GetKeyUp(key)) {
      isDragging = false;
    }
    if (isDragging) {

      pc.displacement += pc.transform.right * (prevX - Input.mousePosition.x) * speed * (multiplyByZoom ? pc.distance : 1);
      pc.displacement += pc.transform.up * (prevY - Input.mousePosition.y) * speed * (multiplyByZoom ? pc.distance : 1);

      prevX = Input.mousePosition.x;
      prevY = Input.mousePosition.y;
    }
  }
}
