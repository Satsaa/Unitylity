

namespace Muc.Systems.Camera {

  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

#if (MUC_HIDE_COMPONENTS || MUC_HIDE_SYSTEM_COMPONENTS)
  [AddComponentMenu("")]
#else
  [AddComponentMenu("MyUnityCollection/" + nameof(Muc.Systems.Camera) + "/" + nameof(MyUnityCamera))]
#endif
  [RequireComponent(typeof(Camera))]
  [DefaultExecutionOrder(-1)]
  public class MyUnityCamera : MonoBehaviour {

    public GameObject target;
    public Camera cam;
    public Vector3 center = Vector3.zero;
    public float distance = 2f;
    public float horRot = 0f;
    public float verRot = 0f;
    public Vector3 displacement = Vector3.zero;

    [Range(0, 0.9999f)] public float moveSmooth = 0.85f;
    [Range(0, 0.9999f)] public float zoomSmooth = 0.85f;
    [Range(0, 0.9999f)] public float rotSmooth = 0.85f;
    [Range(0, 0.9999f)] public float displacementSmooth = 0.85f;

    GameObject prevTarget;
    Vector3 _displacement;
    Vector3 _center;
    GameObject parent;
    Vector3 _parentPos;
    float currentDistance = 2f;
    float currentHorRot = 0f;
    float currentVerRot = 0f;

    void Awake() {
      cam = GetComponent<Camera>();
    }

    void Start() {
      _displacement = displacement;
      prevTarget = target;
      // Child gives birth to the parent 
      parent = new GameObject("Parent");
      // Make parent parent
      transform.parent = parent.transform;
      _parentPos = parent.transform.position;
      // Set to default values
      currentDistance = distance;
      currentHorRot = horRot;
      currentVerRot = verRot;
    }

    void LateUpdate() {
      // Reset addition on target change
      if (target != prevTarget) {
        displacement = Vector3.zero;
        _displacement = displacement;
      }
      prevTarget = target;
      // Smooth addition
      _displacement = Vector3.MoveTowards(displacement, _displacement, Vector3.Distance(displacement, _displacement) * displacementSmooth);
      // Follow target
      if (target) {
        center = target.transform.position;
        _center = Vector3.MoveTowards(center, _center, Vector3.Distance(center, _center) * moveSmooth);
      }
      parent.transform.position = _center + _displacement;



      // Reset rotation
      transform.rotation = Quaternion.identity;
      // Move camera to parent
      transform.position = parent.transform.position;
      // Apply smooth zoom
      currentDistance += (distance - currentDistance) * (1f - zoomSmooth);
      // Apply smooth rotations
      currentHorRot += (horRot - currentHorRot) * (1f - rotSmooth);
      currentVerRot += (verRot - currentVerRot) * (1f - rotSmooth);
      // Move camera to distance
      transform.Translate(new Vector3(0, 0, -currentDistance));
      // Rotate parent around z axis
      parent.transform.Rotate(new Vector3(currentVerRot, 0, 0), Space.World);
      // Rotate parent around y axis
      parent.transform.RotateAround(parent.transform.position, Vector3.up, currentHorRot);
    }
  }

}