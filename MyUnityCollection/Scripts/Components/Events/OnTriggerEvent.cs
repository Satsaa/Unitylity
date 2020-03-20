using UnityEngine;
using UnityEngine.Events;


namespace MUC.Components {

  [RequireComponent(typeof(Collider))]
  public class OnTriggerEvent : MonoBehaviour {
    [Tooltip("Require collider to have specific tag for it to trigger the events")]
    public bool filterTag;
    // !!! DRAW IF ATTRIBUTE
    [Tooltip("The required tag for colliders")]
    public string filteredTag;
    [Tooltip("Invoked when " + nameof(OnTriggerEnter) + " is triggered")]
    public TriggerUnityEvent enterEvent;
    [Tooltip("Invoked when " + nameof(OnTriggerStay) + " is triggered")]
    public TriggerUnityEvent stayEvent;
    [Tooltip("Invoked when " + nameof(OnTriggerExit) + " is triggered")]
    public TriggerUnityEvent exitEvent;

    [System.Serializable]
    public class TriggerUnityEvent : UnityEvent<Collider> { }

    // Start is called before the first frame update
    void Start() {
      foreach (var col in GetComponents<Collider>()) {
        if (col.isTrigger) return;
      }
      throw new UnityException($"{nameof(OnTriggerEvent)} requires a trigger collider to exist on the GameObject");
    }

    public void Test(Collider col) {
      print(col);
    }

    void OnTriggerEnter(Collider col) {
      if (!filterTag || col.tag == filteredTag) {
        stayEvent.Invoke(col);
      }
    }
    void OnTriggerExit(Collider col) {
      if (!filterTag || col.tag == filteredTag) {
        stayEvent.Invoke(col);
      }
    }
    void OnTriggerStay(Collider col) {
      if (!filterTag || col.tag == filteredTag) {
        stayEvent.Invoke(col);
      }
    }
  }

}