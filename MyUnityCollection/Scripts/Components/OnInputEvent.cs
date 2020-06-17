

namespace Muc.Components {

  using System.Collections.Generic;
  using UnityEngine;
  using UnityEngine.Events;
  using Muc.Inspector;

  /// <summary>
  /// Create inputs that fire UnityEvents
  /// </summary>
  public class OnInputEvent : MonoBehaviour {

    public InputEvent[] inputEvents;

    private List<InputEvent> fixedEvents = new List<InputEvent>();

    [System.Serializable]
    public class InputEvent {
      public KeyCode key;
      public InputType type = InputType.Down;
      public bool fixedUpdate;
      public UnityEvent @event;
    }

    public enum InputType { Held, NotHeld, Down, Up, }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
      foreach (var inputEvent in inputEvents) {
        bool activated = false;
        switch (inputEvent.type) {
          case InputType.Down:
            activated = Input.GetKeyDown(inputEvent.key);
            break;
          case InputType.Up:
            activated = Input.GetKeyUp(inputEvent.key);
            break;
          case InputType.Held:
            activated = Input.GetKey(inputEvent.key);
            break;
          case InputType.NotHeld:
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