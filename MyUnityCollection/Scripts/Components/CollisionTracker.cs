

namespace Muc.Components {

  using System.Linq;
  using System.Collections.Generic;
  using UnityEngine;

  [DefaultExecutionOrder(1000)]
  [RequireComponent(typeof(Collider))]
  public class CollisionTracker : MonoBehaviour {

    public bool colliding {
      get {
        PruneStale();
        return colliders.Count > 0;
      }
    }

    private List<Collider> colliders = new List<Collider>();

    public bool stale { get; private set; }

    void LateUpdate() {
      stale = true;
    }

    public bool CollidingWith(Collider collider) {
      PruneStale();
      return colliders.Contains(collider);
    }

    /// <summary>
    /// State is marked stale at each LateUpdate.  
    /// However you may want to signal that some collisions may have gone stale
    /// so prune is called immediately when necessary.  
    /// </summary>
    public void SetStale() {
      stale = true;
    }

    public bool CollidingWith(GameObject gameObject, bool includeChildren = true) {
      PruneStale();
      var cols = includeChildren ? gameObject.GetComponentsInChildren<Collider>() : gameObject.GetComponents<Collider>();
      return colliders.Any(c => cols.Contains(c));
    }


    public void PruneStale() {
      if (stale) Prune();
    }

    public void Prune() {
      stale = false;
      colliders.RemoveAll(c => !c || !c.enabled || c.isTrigger);
    }


    void OnCollisionEnter(Collision col) {
      colliders.Add(col.collider);
    }

    void OnCollisionExit(Collision col) {
      colliders.RemoveAll(c => c == col.collider);
    }
  }

}