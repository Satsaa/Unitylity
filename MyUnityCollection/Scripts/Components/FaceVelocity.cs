using UnityEngine;


namespace Muc.Components {

  [RequireComponent(typeof(Rigidbody))]
  public class FaceVelocity : MonoBehaviour {
    public Vector3 offset;
    [Tooltip("movement towards rb velocity per frame")]
    [Range(0, 1)]
    public float lerpTime = 1;
    Rigidbody rb;
    void Start() {
      rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update() {
      var rot = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(offset);
      rot = Quaternion.Lerp(transform.rotation, rot, lerpTime);
      rb.MoveRotation(rot);
    }
  }

}