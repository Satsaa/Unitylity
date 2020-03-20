using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using MUC.Inspector;

namespace MUC.Components {


  /// <summary>
  /// Create inputs that fire UnityEvents
  /// </summary>
  public class OnInputEvent : MonoBehaviour {

    [System.Serializable]
    public class InputEventList : ReorderableArray<InputEvent> { }
    [Reorderable]
    public InputEventList inputEvents;


    private List<InputEvent> fixedEvents = new List<InputEvent>();

    [System.Serializable]
    public class InputEvent {
      // !!! ENUM ATTRIBUTE
      public KeyCode key;
      public Type type = Type.Down;
      public bool fixedUpdate;
      public UnityEvent @event;
    }

    public enum Type {
      Held,
      NotHeld,
      Down,
      Up,
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
      foreach (var inputEvent in inputEvents) {
        bool activated = false;
        switch (inputEvent.type) {
          case Type.Down:
            activated = Input.GetKeyDown(inputEvent.key);
            break;
          case Type.Up:
            activated = Input.GetKeyUp(inputEvent.key);
            break;
          case Type.Held:
            activated = Input.GetKey(inputEvent.key);
            break;
          case Type.NotHeld:
            activated = !Input.GetKey(inputEvent.key);
            break;
        }
        if (activated) {
          if (inputEvent.fixedUpdate) {
            if (!fixedEvents.Contains(inputEvent)) {
              fixedEvents.Add(inputEvent);
            }
          } else {
            inputEvent.@event.Invoke();
          }
        }
      }
    }

    // Update is called once per frame
    void FixedUpdate() {
      foreach (var fixedEvent in fixedEvents) {
        fixedEvent.@event.Invoke();
      }
      fixedEvents.Clear();
    }
  }

}