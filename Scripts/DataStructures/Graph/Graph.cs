using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

abstract public class Graph : MonoBehaviour {

  public abstract List<Node> nodes { get; set; }

  /// <summary> Draw arrows instead of lines </summary> 
  public bool drawDirection = false;

  public abstract class Node {

    public abstract float3 position { get; set; }

    public float3 Dir(Node b) => b.position - this.position;
    public float distance(Node b) => math.length(Dir(b));
    public float distancesq(Node b) => math.lengthsq(Dir(b));

    public abstract Node[] GetOut();
    public abstract Node[] GetIn();

    public abstract void SetOut(Node[] nodes);
    public abstract void SetIn(Node[] nodes);

    public abstract void SetOut(List<Node> nodes);
    public abstract void SetIn(List<Node> nodes);
  }
}
