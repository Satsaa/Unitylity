using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Cyclic) Directed Graph. Useful for creating predefined path or networks
/// </summary>
public class DirectionalGraph : MonoBehaviour {

  public class Node {
    Vector3 position;
    Node[] branches;
  }
}
