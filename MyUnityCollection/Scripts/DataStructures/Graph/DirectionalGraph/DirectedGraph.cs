using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Cyclic Directed Graph. Useful for creating predefined path or networks
/// </summary> 
public class DirectedGraph : Graph {

  public override List<Graph.Node> nodes { get; set; } = new List<Graph.Node>() { new Node(float3.zero, new Node[2] { new Node(new float3(1, 0, 0)), new Node(new float3(0, 1, 0)) }) };

  void Start() {
    drawDirection = true;
  }

  public new class Node : Graph.Node {

    public Node(float3 position = default(float3), Node[] outgoing = null, Node[] incoming = null) : base(position, outgoing, incoming) { }

    public override float3 position { get; set; }

    public override Graph.Node[] outgoing { get; set; }
    public override Graph.Node[] incoming { get; set; }

    public override void SetOut(List<Graph.Node> nodes) => outgoing = nodes.ToArray();
    public override void SetIn(List<Graph.Node> nodes) => incoming = nodes.ToArray();
  }
}
