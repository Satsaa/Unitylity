

namespace Muc.Geometry {

  using System.Collections.Generic;
  using UnityEngine;

  public interface INode {

    Vector3 position { get; set; }

    ICollection<INode> links { get; }

    void AddLinks(IEnumerable<INode> fromNodes);
    void AddLinks(INode fromNode);
    void RemoveLinks(IEnumerable<INode> fromNodes);
    void RemoveLinks(INode fromNode);

    void ClearLinks();
  }

  public interface INode<TNode> {

    Vector3 position { get; set; }

    ICollection<TNode> links { get; }

    void AddLinks(IEnumerable<TNode> fromNodes);
    void AddLinks(TNode fromNode);
    void RemoveLinks(IEnumerable<TNode> fromNodes);
    void RemoveLinks(TNode fromNode);

    void ClearLinks();
  }
}