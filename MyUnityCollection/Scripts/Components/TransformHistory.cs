using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MUC.Collections;


namespace MUC.Components {

  [DefaultExecutionOrder(1000)]
  public class TransformHistory : MonoBehaviour, IEnumerable<TransformHistory.TransformData> {

    public TransformData this[int index] { get => history[index]; }
    public int Length { get => history.Length; private set => history.Length = value; }
    public int Size { get => history.Length; private set => history.Length = value; }
    IEnumerator IEnumerable.GetEnumerator() => history.GetEnumerator();
    public IEnumerator<TransformData> GetEnumerator() => history.GetEnumerator();

    private CircularBuffer<TransformData> history = new CircularBuffer<TransformData>(2);

    void LateUpdate() {
      history.Add(new TransformData(transform));
    }

    /// <summary>
    /// Sets the size of the history to length if it would increase it. 
    /// </summary>
    /// <returns> Resulting length of history</returns>
    public int SetMinSize(int length) {
      if (length <= Length) return Length;
      Length = length;
      return Length;
    }

    public struct TransformData {
      readonly public Vector3 position;
      readonly public Vector3 localPosition;
      readonly public Quaternion rotation;
      readonly public Quaternion localRotation;
      readonly public Vector3 localScale;

      public static implicit operator Vector3(TransformData a) => a.position;

      public TransformData(UnityEngine.Transform transform) {
        position = transform.position;
        localPosition = transform.localPosition;
        rotation = transform.rotation;
        localRotation = transform.localRotation;
        localScale = transform.localScale;
      }
    }
  }

}