using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AvoidRayCollision : MonoBehaviour {

  public Vector3 direction = Vector3.down;

  public float distance = 1;
  public float maxStrength = 1;
  public AnimationCurve strengthCurve;

  public LayerMask mask;

  private Rigidbody rb;

  // Start is called before the first frame update
  void Start() {
    rb = rb ?? GetComponent<Rigidbody>();
    direction.Normalize();
  }

  // Update is called once per frame
  void Update() {
    rb.velocity += GetAvoidance();
  }

  protected virtual Vector3 GetAvoidance() {
    if (Physics.Raycast(transform.position, direction, out var hit, distance, mask)) {
      var time = 1 + -(hit.distance / distance); // Reversed time
      var mult = strengthCurve.Evaluate(time) * Time.deltaTime;
      var strength = -maxStrength * mult;
      return direction * strength * mult;
    }
    return Vector3.zero;
  }

  void OnDrawGizmosSelected() {
    Gizmos.DrawRay(transform.position, direction * distance);
  }
}
