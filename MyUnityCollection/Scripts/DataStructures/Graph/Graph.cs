using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

abstract public class Graph : MonoBehaviour {

  public abstract List<Node> nodes { get; set; }

  /// <summary> Draw arrows instead of lines </summary> 
  public bool drawDirection = false;

  public abstract class Node {

    public Node(float3 position = default(float3), Node[] outgoing = null, Node[] incoming = null) {
      this.position = position;
      this.outgoing = outgoing ?? new Node[0];
      this.incoming = incoming ?? new Node[0];
    }

    public abstract float3 position { get; set; }
    public abstract Node[] incoming { get; set; }
    public abstract Node[] outgoing { get; set; }

    public float3 Dir(Node b) => b.position - this.position;
    public float distance(Node b) => math.length(Dir(b));
    public float distancesq(Node b) => math.lengthsq(Dir(b));

    public abstract void SetOut(List<Node> nodes);
    public abstract void SetIn(List<Node> nodes);
  }
}
