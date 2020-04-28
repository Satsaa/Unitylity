using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentedCameraZoom : MonoBehaviour {
  public float zoomMult = 1.2f;
  public float zoomMin = 1.2f;
  public float zoomMinStep = 0.1f;

  ParentedCamera pc;
  void Start() {
    pc = gameObject.GetComponent<ParentedCamera>();
  }

  // Update is called once per frame
  void Update() {

    // Scroll zoom
    if (Input.mouseScrollDelta.y < 0) {
      pc.distance -= zoomMin;
      pc.distance *= zoomMult;
      pc.distance += zoomMin + zoomMinStep;
    } else if (Input.mouseScrollDelta.y > 0) {
      pc.distance -= zoomMin;
      pc.distance /= zoomMult;
      pc.distance += zoomMin - zoomMinStep;
    }

    // Make sure zoom doesnt fall behind min zoom
    if (pc.distance < zoomMin) {
      pc.distance = zoomMinStep > 0 ? zoomMin : zoomMin + 0.1f;
    }
  }
}
