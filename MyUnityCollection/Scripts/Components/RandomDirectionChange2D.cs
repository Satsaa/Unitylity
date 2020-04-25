using UnityEngine;

using Muc.Inspector;


namespace Muc.Components {

  [RequireComponent(typeof(Rigidbody2D))]
  public class RandomDirectionChange2D : MonoBehaviour {

    private Rigidbody2D rb;

    [Tooltip("Interval of velocity direction rotation")]
    public float rotationInterval = 1;
    [Tooltip("Maximum degrees of velocity direction rotation per interval")]
    public float maxRotation = 90;
    public AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Tooltip("Scale maximum rotation based on velocity")]
    public bool scaleWithMagnitude;
    [DrawIf(nameof(scaleWithMagnitude))]
    public float magnitudeScale = 1;

    private float lastRotationChange = float.NegativeInfinity;
    private float rotation;
    private float prevVal;

    // Start is called before the first frame update
    void Start() {
      rb = GetComponent<Rigidbody2D>();
      rotation = Random.Range(-maxRotation, maxRotation);
      // Prevent synchronization with others sharing same values
      lastRotationChange = Time.time - Random.Range(0, rotationInterval);
    }

    // Update is called once per frame
    void FixedUpdate() {
      if (lastRotationChange < Time.time - rotationInterval) {
        float scale = scaleWithMagnitude ? rb.velocity.magnitude * magnitudeScale : 1;
        rotation = Random.Range(-maxRotation, maxRotation) * scale;
        lastRotationChange = Time.time;
        prevVal = rotationCurve.Evaluate(0);
      }
      var fraction = (Time.time - lastRotationChange) / rotationInterval;
      var val = rotationCurve.Evaluate(fraction);
      var dif = val - prevVal;
      prevVal = val;
      var deltaRotation = rotation * dif;
      rb.velocity = Quaternion.Euler(0, 0, deltaRotation) * rb.velocity;
    }
  }

}