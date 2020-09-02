

namespace Muc.Math {

  using System;
  using System.Collections;
  using System.Collections.Generic;

  using UnityEngine;

  [System.Serializable]
  public class DirectedNode : IDirectedNode<DirectedNode> {

    [field: SerializeField]
    public virtual Vector3 position { get; set; }

    [field: SerializeField]
    public virtual List<DirectedNode> inLinks { get; private set; } = new List<DirectedNode>();

    [field: SerializeField]
    public virtual List<DirectedNode> outLinks { get; private set; } = new List<DirectedNode>();

    ICollection<DirectedNode> IDirectedNode<DirectedNode>.inLinks => inLinks;
    ICollection<DirectedNode> IDirectedNode<DirectedNode>.outLinks => outLinks;

    public void AddInbound(IEnumerable<DirectedNode> fromNodes) { foreach (var node in fromNodes) AddInbound(node); }
    public void AddInbound(DirectedNode fromNode) {
      if (fromNode == this || fromNode.outLinks.Contains(this)) return;
      this.inLinks.Add(fromNode);
      fromNode.outLinks.Add(this);
    }
    public void RemoveInbound(IEnumerable<DirectedNode> fromNodes) { foreach (var node in fromNodes) RemoveInbound(node); }
    public void RemoveInbound(DirectedNode fromNode) {
      if (fromNode == this || !fromNode.outLinks.Contains(this)) return;
      this.inLinks.Remove(fromNode);
      fromNode.outLinks.Remove(this);
    }

    public void AddOutbound(IEnumerable<DirectedNode> toNodes) { foreach (var node in toNodes) AddOutbound(node); }
    public void AddOutbound(DirectedNode toNode) {
      if (toNode == this || this.outLinks.Contains(toNode)) return;
      this.outLinks.Add(toNode);
      toNode.inLinks.Add(this);
    }
    public void RemoveOutbound(IEnumerable<DirectedNode> toNodes) { foreach (var node in toNodes) RemoveOutbound(node); }
    public void RemoveOutbound(DirectedNode toNode) {
      if (toNode == this || !this.outLinks.Contains(toNode)) return;
      this.outLinks.Remove(toNode);
      toNode.inLinks.Remove(this);
    }


    public void ClearLinks() {
      // Convert to array so we dont modify the Enumerable during enumeration
      foreach (var inLink in inLinks.ToArray())
        inLink.RemoveOutbound(this);
      foreach (var outLink in outLinks.ToArray())
        outLink.RemoveInbound(this);
    }


    #region Search

    public enum SearchType {
      DepthFirst,
      BreadthFirst,
    }


    public DirectedNode DepthFirstSearch(Predicate<DirectedNode> predicate, HashSet<DirectedNode> visited)
      => _DepthFirstSearch(predicate, visited, this);

    public DirectedNode DepthFirstSearch(Predicate<DirectedNode> predicate)
      => _DepthFirstSearch(predicate, new HashSet<DirectedNode>(), this);

    private static DirectedNode _DepthFirstSearch(in Predicate<DirectedNode> predicate, in HashSet<DirectedNode> visited, in DirectedNode node) {

      visited.Add(node);
      if (predicate(node))
        return node;

      foreach (var link in node.outLinks) {
        if (visited.Add(link))
          return _DepthFirstSearch(predicate, visited, link);
      }

      return null;
    }


    public DirectedNode BreadthFirstSearch(Predicate<DirectedNode> predicate, HashSet<DirectedNode> visited)
      => _BreadthFirstSearch(this, predicate, visited);

    public DirectedNode BreadthFirstSearch(Predicate<DirectedNode> predicate)
      => _BreadthFirstSearch(this, predicate, new HashSet<DirectedNode>());

    private static DirectedNode _BreadthFirstSearch(DirectedNode node, Predicate<DirectedNode> predicate, HashSet<DirectedNode> visited) {

      var queue = new Queue<DirectedNode>();

      queue.Enqueue(node);
      visited.Add(node);

      while (queue.Count > 0) {
        var current = queue.Dequeue();

        if (predicate(current))
          return current;

        foreach (var future in current.outLinks) {
          if (visited.Add(future))
            queue.Enqueue(future);
        }
      }

      return null;
    }

    #endregion
  }

}