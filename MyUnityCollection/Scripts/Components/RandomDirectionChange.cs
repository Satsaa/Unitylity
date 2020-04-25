using UnityEngine;

namespace Muc.Components {

  [RequireComponent(typeof(Rigidbody))]
  public class RandomDirectionChange : MonoBehaviour {

    private Rigidbody rb;

    [Tooltip("Interval of velocity direction rotation")]
    public float rotationInterval = 1;
    [Tooltip("Maximum degrees of velocity direction rotation per interval")]
    public float maxRotation = 90;

    private float lastRotationChange = float.NegativeInfinity;
    private Vector3 rotation;

    // Start is called before the first frame update
    void Start() {
      rb = GetComponent<Rigidbody>();
      rotation = new Vector3(
        Random.Range(-maxRotation, maxRotation),
        Random.Range(-maxRotation, maxRotation),
        Random.Range(-maxRotation, maxRotation)
      );
      // Prevent synchronization with others sharing same values
      lastRotationChange = Time.time - Random.Range(0, rotationInterval);
    }

    // Update is called once per frame
    void FixedUpdate() {
      if (lastRotationChange < Time.time - rotationInterval) {
        rotation = new Vector3(
          Random.Range(-maxRotation, maxRotation),
          Random.Range(-maxRotation, maxRotation),
          Random.Range(-maxRotation, maxRotation)
        );
        lastRotationChange = Time.time;
      }
      var dt = Time.deltaTime / rotationInterval;
      var deltaRotation = Quaternion.Euler(
        rotation.x * dt,
        rotation.y * dt,
        rotation.z * dt
      );
      rb.velocity = deltaRotation * rb.velocity;
    }
  }

}