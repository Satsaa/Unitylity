using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TransformData {
  public TransformData(Vector3 position, Quaternion rotation, Vector3 localScale) {
    this.position = position;
    this.rotation = rotation;
    this.localScale = localScale;
  }
  public Vector3 position;
  public Quaternion rotation;
  public Vector3 localScale;
}

public static class TransformExtensions {
  public static TransformData Save(this Transform t, bool local = false) {
    return local ? new TransformData(t.localPosition, t.localRotation, t.localScale) : new TransformData(t.position, t.rotation, t.localScale);
  }

  public static void Load(this Transform t, TransformData d, bool local = false) {
    if (local) {
      t.localPosition = d.position;
      t.localRotation = d.rotation;
      t.localScale = d.localScale;
    } else {
      t.position = d.position;
      t.rotation = d.rotation;
      t.localScale = d.localScale;
    }
  }
}