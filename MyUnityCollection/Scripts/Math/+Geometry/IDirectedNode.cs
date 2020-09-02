

namespace Muc.Math {

  using System.Collections.Generic;

  using UnityEngine;

  public interface IDirectedNode {

    Vector3 position { get; set; }

    ICollection<IDirectedNode> inNodes { get; }
    ICollection<IDirectedNode> outNodes { get; }

    void AddInbound(IEnumerable<IDirectedNode> fromNodes);
    void AddInbound(IDirectedNode fromNode);
    void AddOutbound(IEnumerable<IDirectedNode> toNodes);
    void AddOutbound(IDirectedNode toNode);

    void RemoveInbound(IEnumerable<IDirectedNode> fromNodes);
    void RemoveInbound(IDirectedNode fromNode);
    void RemoveOutbound(IEnumerable<IDirectedNode> toNodes);
    void RemoveOutbound(IDirectedNode toNode);

    void ClearConnections();

  }

  public interface IDirectedNode<TNode> {

    Vector3 position { get; set; }

    ICollection<TNode> inLinks { get; }
    ICollection<TNode> outLinks { get; }

    void AddInbound(IEnumerable<TNode> fromNodes);
    void AddInbound(TNode fromNode);
    void AddOutbound(IEnumerable<TNode> toNodes);
    void AddOutbound(TNode toNode);

    void RemoveInbound(IEnumerable<TNode> fromNodes);
    void RemoveInbound(TNode fromNode);
    void RemoveOutbound(IEnumerable<TNode> toNodes);
    void RemoveOutbound(TNode toNode);

    void ClearLinks();

  }

}