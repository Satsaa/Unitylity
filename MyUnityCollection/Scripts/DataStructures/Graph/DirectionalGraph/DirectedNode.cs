using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DirectedNode : ScriptableObject {

  void OnDestroy() {
    // ToArray to avoid modifying the enumarated list directly
    foreach (var inNode in _inbound.ToArray())
      inNode.RemoveOutbound(this);
    foreach (var outNode in _outbound.ToArray())
      outNode.RemoveInbound(this);
  }

  public float3 position;

  #region Util
  public float3 Dir(DirectedNode node) => node.position - this.position;
  public Line LineTo(DirectedNode node) => new Line(this.position, node.position);
  public float Distance(DirectedNode node) => math.distance(node.position, this.position);
  public float DistanceSq(DirectedNode node) => math.distancesq(node.position, this.position);
  #endregion

  #region Inbound nodes
  public List<DirectedNode> inbound { get => _inbound; private set => _inbound = value; }
  [SerializeField]
  private List<DirectedNode> _inbound = new List<DirectedNode>();

  public void AddInbound(List<DirectedNode> nodes) => nodes.ForEach(AddInbound);
  public void AddInbound(DirectedNode node) {
    if (node == this || node.outbound.Contains(this)) return;
    this.inbound.Add(node);
    node.outbound.Add(this);
  }

  public void RemoveInbound(List<DirectedNode> nodes) => nodes.ForEach(RemoveInbound);
  public void RemoveInbound(DirectedNode node) {
    if (node == this || !node.outbound.Contains(this)) return;
    this.inbound.Remove(node);
    node.outbound.Remove(this);
  }
  #endregion

  #region Outbound nodes
  public List<DirectedNode> outbound { get => _outbound; private set => _outbound = value; }
  [SerializeField]
  private List<DirectedNode> _outbound = new List<DirectedNode>();

  public void AddOutbound(List<DirectedNode> nodes) => nodes.ForEach(AddOutbound);
  public void AddOutbound(DirectedNode node) {
    if (node == this || this.outbound.Contains(node)) return;
    this.outbound.Add(node);
    node.inbound.Add(this);
  }

  public void RemoveOutbound(List<DirectedNode> nodes) => nodes.ForEach(RemoveOutbound);
  public void RemoveOutbound(DirectedNode node) {
    if (node == this || !this.outbound.Contains(node)) return;
    this.outbound.Remove(node);
    node.inbound.Remove(this);
  }
  #endregion
}