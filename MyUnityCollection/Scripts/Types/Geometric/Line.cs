using Unity.Mathematics;
using UnityEngine;
using System.Runtime.CompilerServices;

public struct Line {
  public float3 start;
  public float3 end;

  public float3 dir { get => start - end; }
  public float length { get => math.length(dir); }
  public float lengthsq { get => math.lengthsq(dir); }


  public Line(float3 start, float3 end) {
    this.start = start;
    this.end = end;
  }
}