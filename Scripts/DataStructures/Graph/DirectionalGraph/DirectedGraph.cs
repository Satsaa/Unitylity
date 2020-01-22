using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Cyclic Directed Graph. Useful for creating predefined path or networks
/// </summary> 
public class DirectedGraph : Graph {

  public override List<Graph.Node> nodes { get => _nodes; set => _nodes = value; }
  private List<Graph.Node> _nodes;

  void Start() {
    _nodes = new List<Graph.Node>();
  }

  public new class Node : Graph.Node {

    public Node(float3 pos) {
      _position = pos;
    }

    public override float3 position { get => _position; set => _position = value; }

    private Graph.Node[] @out;
    private Graph.Node[] @in;

    private float3 _position;

    public override Graph.Node[] Out() => @out;
    public override Graph.Node[] In() => @in;

    public override void SetOut(Graph.Node[] nodes) => @out = nodes;
    public override void SetIn(Graph.Node[] nodes) => @in = nodes;

    public override void SetOut(List<Graph.Node> nodes) => @out = nodes.ToArray();
    public override void SetIn(List<Graph.Node> nodes) => @in = nodes.ToArray();
  }
}
