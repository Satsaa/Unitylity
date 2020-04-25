using UnityEngine;
using Muc.Types.Extensions;


namespace Muc.Components {

  [RequireComponent(typeof(Rigidbody2D))]
  public class FaceVelocity2D : MonoBehaviour {
    public Vector3 offset;
    public bool lookRotation;
    [Tooltip("movement towards rb velocity per frame")]
    [Range(0, 1)]
    public float lerpTime = 1;
    Rigidbody2D rb;

    void Start() {
      rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
      if (lookRotation) {
        var rot = Quaternion.LookRotation(rb.velocity) * Quaternion.Euler(offset);
        rot = Quaternion.Lerp(transform.rotation, rot, lerpTime);
        rb.MoveRotation(rot);
      } else {
        var angle = rb.velocity.Angle();
        var rot = Quaternion.Euler(offset.x, offset.y, offset.z - angle + 90);
        rot = Quaternion.Lerp(transform.rotation, rot, lerpTime);
        rb.MoveRotation(rot);
      }
    }
  }

}