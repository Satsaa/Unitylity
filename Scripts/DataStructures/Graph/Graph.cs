using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

abstract public class Graph : MonoBehaviour {

  public abstract List<Node> nodes { get; set; }

  /// <summary> Draw arrows after  </summary> 
  protected bool drawDirection = false;

  public abstract class Node {

    public abstract float3 position { get; set; }

    public abstract Node[] Out();
    public abstract Node[] In();

    public abstract void SetOut(Node[] nodes);
    public abstract void SetIn(Node[] nodes);

    public abstract void SetOut(List<Node> nodes);
    public abstract void SetIn(List<Node> nodes);
  }
}
