#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Mathematics;

/// <summary>
/// Relaxed DirectedGraph editor
/// </summary>
[CustomEditor(typeof(DirectedGraph), true)]
public class DirectedGraphEditor : Editor {

  private DirectedGraph t { get => (DirectedGraph)target; }

  private Camera cam { get => Camera.current; }
  private Quaternion cameraLook { get => Quaternion.LookRotation(cam.transform.forward, cam.transform.up); }

  private List<DirectedNode> selection = new List<DirectedNode>();
  private float3 selectionAveragePos;

  private bool clickUsed;
  private bool mouse = false;
  private Vector2 mousePos;
  private bool shift { get => Event.current.shift; }

  private bool control { get => controlRight || controlLeft; }
  private bool controlRight = false;
  private bool controlLeft = false;

  private bool alt { get => altRight || altLeft; }
  private bool altRight = false;
  private bool altLeft = false;


  DirectedGraphEditor() {
    Undo.undoRedoPerformed += UpdateAveragePosition;
  }

  void OnSceneGUI() {
    switch (Event.current.type) {
      case EventType.Repaint:
        break;

      case EventType.MouseMove:
        mousePos = Event.current.mousePosition;
        break;

      case EventType.MouseDown:
        if (Event.current.button == 0) mouse = true;
        break;

      case EventType.MouseUp:
        if (Event.current.button == 0) {
          clickUsed = false;
          mouse = false;
          soloCreating = false;
        }
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
    Draw();
  }

  public DirectedNode FindClosestNode(DirectedNode node, out float dist) {
    DirectedNode minNode = null;
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
  public DirectedNode FindClosestNode(float3 position, out float dist) {
    DirectedNode minNode = null;
    var minDistSq = float.PositiveInfinity;
    foreach (var node in t.nodes) {
      var distSq = math.distancesq(position, node.position);
      if (distSq < minDistSq) {
        minNode = node;
        minDistSq = distSq;
      }
    }
    dist = math.sqrt(minDistSq);
    return minNode;
  }
  public DirectedNode FindClosestNodeToLine(Line line, out float dist) {
    DirectedNode minNode = null;
    var distsq = float.PositiveInfinity;
    foreach (var node in t.nodes) {
      var ndistsq = math.distancesq(node.position, line.ClampToLine(node.position));
      if (ndistsq < distsq) {
        minNode = node;
        distsq = ndistsq;
      }
    }
    dist = math.sqrt(distsq);
    return minNode;
  }
  public DirectedNode FindClosestNodeToRay(Ray ray, out float dist) {
    DirectedNode minNode = null;
    dist = float.PositiveInfinity;
    foreach (var node in t.nodes) {
      var nodeDist = DistanceToRay(ray, node.position);
      if (nodeDist < dist) {
        minNode = node;
        dist = nodeDist;
      }
    }
    return minNode;
  }
  public float3 FindClosestPointToLine(Line line, out float dist) {
    float3 minPos = 0;
    var distsq = float.PositiveInfinity;
    foreach (var node in t.nodes) {
      foreach (var outNode in node.outbound) {
        var outLine = node.LineTo(outNode);
        var distLine = line.ShortestConnectingLine(outLine);
        if (distLine.lengthsq < distsq) {
          minPos = distLine.end;
          distsq = distLine.lengthsq;
        }
      }
    }
    if (distsq == float.PositiveInfinity) {
      dist = math.distance(float3.zero, line.ClampToLine(float3.zero));
      return float3.zero;
    }
    dist = math.sqrt(distsq);
    return minPos;
  }

  void Draw() {
    var defaultColor = Handles.color;
    foreach (var node in t.nodes) {
      Handles.color = defaultColor;
      SelectionMoveHandle(node);
    }
    if (shift)
      if (selection.Count == 0)
        DrawNewSoloPair();
      else
        DrawNewConnectedNode();
    else
      soloCreating = false;
    DrawAveragePositionHandle();

    foreach (var node in t.nodes) {
      DrawConnections(node);
    }
  }

  void DrawNewConnectedNode() {
    if (Event.current.isMouse) Event.current.Use();
    var ray = cam.ScreenPointToRay(new Vector2(mousePos.x, cam.pixelHeight - mousePos.y));
    var plane = new Plane(-cam.transform.forward, selectionAveragePos);
    if (plane.Raycast(ray, out var distance)) {
      var target = ray.GetPoint(distance);
      foreach (var node in selection) {
        Handles.DrawDottedLine(node.position, target, 3);
        DrawDirArrow(node.position, target);
      }
      Handles.Button(target, cameraLook, 0.1f, 0.1f, Handles.RectangleHandleCap);
      if (mouse && !clickUsed) {
        var node = CreateNode(target);
        node.AddInbound(selection);
        foreach (var selNode in selection)
          Undo.RegisterCompleteObjectUndo(selNode, "Create node");
        Select(node);
        Undo.RegisterCompleteObjectUndo(node, "Create node");
        clickUsed = true;
        Dirty();
      }
    }
  }

  float3 soloStartPos;
  bool soloCreating;
  void DrawNewSoloPair() {
    if (Event.current.isMouse) Event.current.Use();
    var ray = cam.ScreenPointToRay(new Vector2(mousePos.x, cam.pixelHeight - mousePos.y));
    var closestPoint = FindClosestPointToLine(new Line(ray.origin, ray.origin + ray.direction * 5), out var dist);
    Handles.SphereHandleCap(0, closestPoint, Quaternion.identity, 0.1f, EventType.Repaint);
    var plane = new Plane(-cam.transform.forward, closestPoint);
    if (!soloCreating) {
      if (plane.Raycast(ray, out var distance)) {
        soloStartPos = ray.GetPoint(distance);
        soloCreating = true;
      }
    } else {
      if (plane.Raycast(ray, out var distance)) {
        var target = ray.GetPoint(distance);
        Handles.DrawDottedLine(soloStartPos, target, 3);
        DrawDirArrow(soloStartPos, target);

        Handles.Button(target, cameraLook, 0.1f, 0.1f, Handles.RectangleHandleCap);
        if (mouse && !clickUsed) {
          var node1 = CreateNode(soloStartPos);
          var node2 = CreateNode(target);
          node1.AddOutbound(node2);
          Undo.RegisterCompleteObjectUndo(t, "Create node pair");
          clickUsed = true;
          Select(node2);
          Dirty();
        }
      }
    }
  }

  void DrawAveragePositionHandle() {
    if (selection.Count == 0) return;
    EditorGUI.BeginChangeCheck();
    float3 newPos = Handles.PositionHandle(selectionAveragePos, Quaternion.identity);
    if (EditorGUI.EndChangeCheck()) {
      foreach (var node in selection) {
        node.position += newPos - selectionAveragePos;
        Undo.RegisterCompleteObjectUndo(node, "Move node selection");
      }
      UpdateAveragePosition();
      Dirty();
    }
  }

  void SelectionMoveHandle(DirectedNode node) {
    if (selection.Contains(node)) {
      Handles.color = Color.green;
      if (Handles.Button(node.position, cameraLook, 0.1f, 0.1f, Handles.RectangleHandleCap)) {
        selection.Remove(node);
        UpdateAveragePosition();
      }
    } else {
      if (Handles.Button(node.position, cameraLook, 0.1f, 0.1f, Handles.RectangleHandleCap)) {
        selection.Add(node);
        UpdateAveragePosition();
      }
    }
  }

  void Select(DirectedNode node) {
    selection.Clear();
    selection.Add(node);
    UpdateAveragePosition();
  }
  void Select(List<DirectedNode> nodes) {
    selection.Clear();
    selection.AddRange(nodes);
    UpdateAveragePosition();
  }

  void DrawMoveHandle(DirectedNode node) {
    EditorGUI.BeginChangeCheck();
    Vector3 newPos = Handles.PositionHandle(node.position, Quaternion.identity);
    if (EditorGUI.EndChangeCheck()) {
      node.position = newPos;
      Undo.RegisterCompleteObjectUndo(node, "Move node");
      Dirty();
    }
  }

  void DrawConnections(DirectedNode node) {
    foreach (var outNode in node.outbound) {
      DrawDirArrow(node.position, outNode.position);
      Handles.DrawLine(node.position, outNode.position);
    }
  }

  void DrawDirArrow(float3 sourcePoint, float3 endPoint) {
    var dist = math.distance(endPoint, sourcePoint);
    if (dist < 0.0001f) return;
    var maxSize = 0.1f;
    var size = math.min(maxSize, dist / 10);
    Handles.ConeHandleCap(0, endPoint - (endPoint - sourcePoint).SetLen(size) * 0.7f, Quaternion.LookRotation(endPoint - sourcePoint), size, EventType.Repaint);
  }

  DirectedNode CreateNode(float3 position) {
    var node = (DirectedNode)ScriptableObject.CreateInstance(typeof(DirectedNode));
    t.nodes.Add(node);
    node.position = position;
    return node;
  }

  void Dirty() {
    if (!Application.isPlaying) {
      EditorUtility.SetDirty(t);
      UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
  }

  void UpdateAveragePosition() {
    float3 average = 0;
    foreach (var node in selection)
      average += node.position;
    average /= selection.Count;
    selectionAveragePos = average;
  }
  public static float DistanceToRay(Ray ray, Vector3 point) {
    return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
  }
}
#endif