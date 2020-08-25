

namespace Muc.Timing {

  using UnityEngine;

  /// <summary>
  /// A simple utility class which allows you to check whether the frame has changed.
  /// </summary>
  public class FrameChangeChecker {

    /// <summary>
    /// Frame of reference.
    /// </summary>
    public float referenceFrame { get; private set; }

    /// <summary>
    /// Whether the reference frame is the current one.
    /// </summary>
    public bool changed => referenceFrame == Time.frameCount;

    /// <summary>
    /// Creates a FrameChangeChecker.
    /// </summary>
    public FrameChangeChecker() {
      Reset();
    }

    /// <summary>
    /// Sets the reference frame to the current frame.
    /// </summary>
    public void Reset() {
      this.referenceFrame = Time.frameCount;
    }

    /// <summary>
    /// Sets the reference frame to `frame`.
    /// </summary>
    /// <param name="frame">Frame which the reference frame is set to.</param>
    public void SetReferenceFrame(int frame) {
      try {
        this.referenceFrame = frame;
      } catch (UnityException) { }
    }
  }

}