using Unity.Mathematics;
using UnityEngine;

public struct Line {
  public float3 start;
  public float3 end;

  public float3 dir { get => end - start; }
  public float magnitude { get => length; }
  public float length { get => math.length(dir); }
  public float lengthsq { get => math.lengthsq(dir); }
  public Line reversed { get => new Line(end, start); }

  public Line(float3 start, float3 end) {
    this.start = start;
    this.end = end;
  }

  public void Debug() => UnityEngine.Debug.DrawLine(start, end);
  public void Debug(Color color, float duration = 0) => UnityEngine.Debug.DrawLine(start, end, color, duration);

  public void Reverse() {
    var temp = start;
    start = end;
    end = temp;
  }

  public float3 ClampToLine(float3 point) {

    //get vector from point on infinite line to point in space
    var lineVec = math.normalizesafe(dir);
    float3 linePointToPoint = point - start;
    float t = math.dot(linePointToPoint, lineVec);
    float3 projectedPoint = start + lineVec * t;

    Side side = PointSide(projectedPoint);

    //The projected point is on the line segment
    switch (side) {
      case Side.inside:
        return projectedPoint;
      case Side.start:
        return start;
      case Side.end:
        return end;
      default:
        return 0;
    }
  }

  public enum Side { inside, start, end, }
  public Side PointSide(float3 point) {
    float3 dir = end - start;
    float3 pointVec = point - start;
    float dot = math.dot(pointVec, dir);
    //point is on side of linePoint2, compared to linePoint1
    if (dot > 0) {
      //point is on the line segment
      if (math.lengthsq(pointVec) <= math.lengthsq(dir)) {
        return Side.inside;
      }
      //point is not on the line segment and it is on the side of linePoint2
      else {
        return Side.end;
      }
    }
    //Point is not on side of linePoint2, compared to linePoint1.
    //Point is not on the line segment and it is on the side of linePoint1.
    else {
      return Side.start;
    }
  }

  /// <summary> Returns the line representing the shortest path connecting `this line` and `line` </summary>
  public Line ShortestConnectingLine(Line line) {
    ShortestLineConnectingTwoInfiniteLines(out var res, this.start, math.normalize(this.dir), line.start, math.normalize(line.dir));

    var startSide = this.PointSide(res.start);
    if (startSide == Line.Side.start) res.start = this.start;
    else if (startSide == Line.Side.end) res.start = this.end;

    var endSide = line.PointSide(res.end);
    if (endSide == Line.Side.start) res.end = line.start;
    else if (endSide == Line.Side.end) res.end = line.end;

    if (endSide != Line.Side.inside)
      res.start = this.ClampToLine(res.end);
    if (startSide != Line.Side.inside)
      res.end = line.ClampToLine(res.start);

    return res;
  }

  //Two non-parallel lines which may or may not touch each other have a point on each line which are closest
  //to each other. This function finds those two points. If the lines are not parallel, the function 
  //outputs true, otherwise false.
  private static bool ShortestLineConnectingTwoInfiniteLines(out Line closeLine, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2) {

    closeLine = new Line(0, 0);

    float a = Vector3.Dot(lineVec1, lineVec1);
    float b = Vector3.Dot(lineVec1, lineVec2);
    float e = Vector3.Dot(lineVec2, lineVec2);

    float d = a * e - b * b;

    //lines are not parallel
    if (d != 0.0f) {

      Vector3 r = linePoint1 - linePoint2;
      float c = Vector3.Dot(lineVec1, r);
      float f = Vector3.Dot(lineVec2, r);

      float s = (b * f - c * e) / d;
      float t = (a * f - c * b) / d;

      closeLine.start = linePoint1 + lineVec1 * s;
      closeLine.end = linePoint2 + lineVec2 * t;

      return true;
    } else {
      return false;
    }
  }
}