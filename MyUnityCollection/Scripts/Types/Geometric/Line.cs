using Unity.Mathematics;
using UnityEngine;
using System.Runtime.CompilerServices;

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
    float3 clampedPoint = new float3();
    float minX, minY, minZ, maxX, maxY, maxZ;
    if (this.start.x <= this.end.x) {
      minX = this.start.x;
      maxX = this.end.x;
    } else {
      minX = this.end.x;
      maxX = this.start.x;
    }
    if (this.start.y <= this.end.y) {
      minY = this.start.y;
      maxY = this.end.y;
    } else {
      minY = this.end.y;
      maxY = this.start.y;
    }
    if (this.start.z <= this.end.z) {
      minZ = this.start.z;
      maxZ = this.end.z;
    } else {
      minZ = this.end.z;
      maxZ = this.start.z;
    }
    clampedPoint.x = (point.x < minX) ? minX : (point.x > maxX) ? maxX : point.x;
    clampedPoint.y = (point.y < minY) ? minY : (point.y > maxY) ? maxY : point.y;
    clampedPoint.z = (point.z < minZ) ? minZ : (point.z > maxZ) ? maxZ : point.z;
    return clampedPoint;
  }

  public Line DistanceLine(Line line) {
    float3 p1, p2, p3, p4, d1, d2;
    p1 = this.start;
    p2 = this.end;
    p3 = line.start;
    p4 = line.end;
    d1 = p2 - p1;
    d2 = p4 - p3;
    float eq1nCoeff = (d1.x * d2.x) + (d1.y * d2.y) + (d1.z * d2.z);
    float eq1mCoeff = (-(math.pow(d1.x, 2)) - (math.pow(d1.y, 2)) - (math.pow(d1.z, 2)));
    float eq1Const = ((d1.x * p3.x) - (d1.x * p1.x) + (d1.y * p3.y) - (d1.y * p1.y) + (d1.z * p3.z) - (d1.z * p1.z));
    float eq2nCoeff = ((math.pow(d2.x, 2)) + (math.pow(d2.y, 2)) + (math.pow(d2.z, 2)));
    float eq2mCoeff = -(d1.x * d2.x) - (d1.y * d2.y) - (d1.z * d2.z);
    float eq2Const = ((d2.x * p3.x) - (d2.x * p1.x) + (d2.y * p3.y) - (d2.y * p2.y) + (d2.z * p3.z) - (d2.z * p1.z));
    float[,] M = new float[,] { { eq1nCoeff, eq1mCoeff, -eq1Const }, { eq2nCoeff, eq2mCoeff, -eq2Const } };
    int rowCount = M.GetUpperBound(0) + 1;
    // pivoting
    for (int col = 0; col + 1 < rowCount; col++) if (M[col, col] == 0)
        // check for zero coefficients
        {
        // find non-zero coefficient
        int swapRow = col + 1;
        for (; swapRow < rowCount; swapRow++) if (M[swapRow, col] != 0) break;

        if (M[swapRow, col] != 0) // found a non-zero coefficient?
        {
          // yes, then swap it with the above
          float[] tmp = new float[rowCount + 1];
          for (int i = 0; i < rowCount + 1; i++) { tmp[i] = M[swapRow, i]; M[swapRow, i] = M[col, i]; M[col, i] = tmp[i]; }
        } else return default(Line); // no, then the matrix has no unique solution
      }

    // elimination
    for (int sourceRow = 0; sourceRow + 1 < rowCount; sourceRow++) {
      for (int destRow = sourceRow + 1; destRow < rowCount; destRow++) {
        float df = M[sourceRow, sourceRow];
        float sf = M[destRow, sourceRow];
        for (int i = 0; i < rowCount + 1; i++)
          M[destRow, i] = M[destRow, i] * df - M[sourceRow, i] * sf;
      }
    }

    // back-insertion
    for (int row = rowCount - 1; row >= 0; row--) {
      float f = M[row, row];
      if (f == 0) return default(Line);

      for (int i = 0; i < rowCount + 1; i++) M[row, i] /= f;
      for (int destRow = 0; destRow < row; destRow++) { M[destRow, rowCount] -= M[destRow, row] * M[row, rowCount]; M[destRow, row] = 0; }
    }
    float n = M[0, 2];
    float m = M[1, 2];
    float3 i1 = new float3 { x = p1.x + (m * d1.x), y = p1.y + (m * d1.y), z = p1.z + (m * d1.z) };
    float3 i2 = new float3 { x = p3.x + (n * d2.x), y = p3.y + (n * d2.y), z = p3.z + (n * d2.z) };
    float3 i1Clamped = this.ClampToLine(i1);
    float3 i2Clamped = line.ClampToLine(i2);
    return new Line((float3)i1Clamped, (float3)i2Clamped);
  }

}