

namespace Muc.Types {

  using UnityEngine;

  public abstract class ScriptablePredicate : ScriptableObject {
    public abstract bool predicate(GameObject target);

  }
}