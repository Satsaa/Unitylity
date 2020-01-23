#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Mathematics;


/// <summary>
/// Relaxed graph editor
/// </summary>
[CustomEditor(typeof(Graph), true)]
public class GraphEditor : Editor {

  private Graph t { get => (Graph)target; }

  private bool mouse = false;
  private bool shift = false;

  private bool control { get => controlRight || controlLeft; }
  private bool controlRight = false;
  private bool controlLeft = false;

  private bool alt { get => altRight || altLeft; }
  private bool altRight = false;
  private bool altLeft = false;

  protected virtual void OnSceneGUI() {
    shift = Event.current.shift;

    switch (Event.current.type) {
      case EventType.Repaint:
        Draw();
        break;

      case EventType.MouseDown:
        if (Event.current.button == 0)
          mouse = true;
        break;

      case EventType.MouseUp:
        if (Event.current.button == 0) mouse = false;
        break;

      case EventType.KeyDown:
        if (Event.current.keyCode == KeyCode.RightAlt) altRight = true;
        else if (Event.current.keyCode == KeyCode.LeftAlt) altLeft = true;

        if (Event.current.keyCode == KeyCode.RightControl) controlRight = true;
        else if (Event.current.keyCode == KeyCode.LeftControl) controlLeft = true;
        break;

      case EventType.KeyUp:
        if (Event.current.keyCode == KeyCode.RightAlt) altRight = false;
        else if (Event.current.keyCode == KeyCode.LeftAlt) altLeft = false;

        if (Event.current.keyCode == KeyCode.RightControl) controlRight = false;
        else if (Event.current.keyCode == KeyCode.LeftControl) controlLeft = false;
        break;
    }
  }

  public Graph.Node FindClosestNode(Graph.Node node, out float dist) {
    Graph.Node minNode = null;
    var minDistSq = float.PositiveInfinity;
    foreach (var other in t.nodes) {
      if (other == node) continue;
      var distSq = math.distancesq(node.position, other.position);
      if (distSq < minDistSq) {
        minNode = other;
        minDistSq = distSq;
      }
    }
    dist = math.sqrt(minDistSq);
    return minNode;
  }

  public Graph.Node FindClosestNode(float3 position, out float dist) => FindClosestNode(null, out dist);

  void Draw() {

    foreach (var node in t.nodes) {
      foreach (var @out in node.GetOut()) {
        if (t.drawDirection)
          Handles.ArrowCap(0, node.position, Quaternion.LookRotation(@out.position - node.position), node.);
        else
          Handles.DrawLine(node.position, @out.position);
      }
    }
  }
}
#endif