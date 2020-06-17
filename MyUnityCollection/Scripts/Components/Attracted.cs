

namespace Muc.Components {

  using UnityEngine;
  using Muc.Types.Extensions;

  [RequireComponent(typeof(Rigidbody))]
  public class Attracted : MonoBehaviour {
    private Rigidbody rb;
    [Tooltip("The transform to attract towards")]
    public Transform attTransform;
    [Min(0)]
    public float farStrength = 0.1f;
    [Min(0)]
    public float nearStrength = 1;
    [Tooltip("Near attraction strength is applied at distances lower than this")]
    public float nearDistance = 0.1f;
    [Tooltip("Far attraction strength is applied at distances higher than this")]
    public float farDistance = 1;

    // Start is called before the first frame update
    void Start() {
      rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {
      var dist = Vector3.Distance(attTransform.position, transform.position);
      dist = Mathf.Clamp(dist, nearDistance, farDistance);
      var strength = dist.Remap(nearDistance, farDistance, nearStrength, farStrength);

      rb.AddForce((attTransform.position - transform.position).SetLenSafe(strength) * Time.deltaTime);
    }
  }

}