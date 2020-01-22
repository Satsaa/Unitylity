#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// <summary>
/// Relaxed graph editor
/// </summary>
[CustomEditor(typeof(Graph), true)]
public class GraphEditor {

  // private MoveToClosestPointInShapes t { get => ((MoveToClosestPointInShapes)target); }

  private List<Line> lines = new List<Line>();
  public string Test() { return "asd"; }

  public struct asd { }
}

#endif