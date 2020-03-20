using UnityEngine;

using MUC.Types.Extensions;


namespace MUC.Components {

  [RequireComponent(typeof(Rigidbody))]
  public class TargetVelocity : MonoBehaviour {

    [Tooltip("Aproach this velocity")]
    public float targetVelocity = 1;
    [Tooltip("Multiply time.deltaTime with this value. Will increase speed of reaching target velocity")]
    public float strength = 1;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
      rb = GetComponent<Rigidbody>();
      if (targetVelocity != 0 && rb.velocity.Equals(Vector3.zero)) rb.velocity = new Vector3(0.001f, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate() {
      var mag = rb.velocity.magnitude;
      var mult = Mathf.Lerp(mag, targetVelocity, 1 - Mathf.Pow(1 - Time.deltaTime, strength));
      rb.velocity = rb.velocity.SetLenSafe(mult);
    }
  }

}