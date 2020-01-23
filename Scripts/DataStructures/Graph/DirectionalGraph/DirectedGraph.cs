using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Cyclic Directed Graph. Useful for creating predefined path or networks
/// </summary> 
public class DirectedGraph : Graph {

  public override List<Graph.Node> nodes { get; set; } = new List<Graph.Node>();

  void Start() {

  }

  public new class Node : Graph.Node {

    public Node(float3 pos) {
      position = pos;
    }

    public override float3 position { get; set; }

    private Graph.Node[] @out;
    private Graph.Node[] @in;

    public override Graph.Node[] GetOut() => @out;
    public override Graph.Node[] GetIn() => @in;

    public override void SetOut(Graph.Node[] nodes) => @out = nodes;
    public override void SetIn(Graph.Node[] nodes) => @in = nodes;

    public override void SetOut(List<Graph.Node> nodes) => @out = nodes.ToArray();
    public override void SetIn(List<Graph.Node> nodes) => @in = nodes.ToArray();
  }
}
