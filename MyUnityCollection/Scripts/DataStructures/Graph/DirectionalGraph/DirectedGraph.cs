using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Cyclic Directed Graph. Useful for creating predefined path or networks
/// </summary> 
public class DirectedGraph : MonoBehaviour {

  [MyBox.DisplayInspector]
  public List<DirectedNode> nodes = new List<DirectedNode>();

  void Reset() {
    var node1 = (DirectedNode)DirectedNode.CreateInstance(typeof(DirectedNode));
    var node2 = (DirectedNode)ScriptableObject.CreateInstance(typeof(DirectedNode));
    var node3 = (DirectedNode)ScriptableObject.CreateInstance(typeof(DirectedNode));
    node1.position = new float3(1, 1, 1);
    node2.position = new float3(0, 0, 0);
    node3.position = new float3(-1, -1, -1);

    node2.AddOutbound(node1);
    node2.AddOutbound(node3);
    nodes = new List<DirectedNode>() { node1, node2, node3 };
  }
}
