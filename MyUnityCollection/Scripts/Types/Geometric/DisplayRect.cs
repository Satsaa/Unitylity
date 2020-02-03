using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class DisplayRect : MonoBehaviour {
  public Rect rect;
  public Color color = Color.magenta;
  [Range(0, 1)]
  public float fillAlpha = 0.2f;

  void OnDrawGizmos() {
    Gizmos.color = color;
    Gizmos.DrawWireCube(rect.center, rect.size);

    if (fillAlpha == 0) return;
    Gizmos.color = color * fillAlpha;
    Gizmos.DrawCube(rect.center, rect.size);
  }
}
