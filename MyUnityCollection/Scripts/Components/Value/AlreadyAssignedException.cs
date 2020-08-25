

namespace Muc.Components.Values {

  using System;

  [System.Serializable]
  public class AlreadyAssignedException : System.Exception {
    public AlreadyAssignedException(string message = "This Modifier is already assigned to a Value") : base(message) { }
    public AlreadyAssignedException(string message, System.Exception inner) : base(message, inner) { }
    protected AlreadyAssignedException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}