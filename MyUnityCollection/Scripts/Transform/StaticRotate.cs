using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class StaticRotate : MonoBehaviour {

  [Tooltip("The rotation applied per second")]
  public float3 rotation;
  public bool local = false;

  // Update is called once per frame
  void Update() {
    var qt = quaternion.EulerXYZ(math.radians(rotation) * Time.deltaTime);
    if (local)
      transform.localRotation *= qt;
    else
      transform.rotation *= qt;
  }
}
