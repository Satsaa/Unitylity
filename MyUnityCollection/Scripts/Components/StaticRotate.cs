

namespace Muc.Components {

  using UnityEngine;

  public class StaticRotate : MonoBehaviour {

    [Tooltip("The rotation applied per second")]
    public Vector3 rotation;
    public bool local = false;

    // Update is called once per frame
    void Update() {
      var qt = Quaternion.Euler(rotation * Time.deltaTime);
      if (local)
        transform.localRotation *= qt;
      else
        transform.rotation *= qt;
    }
  }

}