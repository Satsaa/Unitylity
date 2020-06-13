

namespace Muc.Types.Extensions {

  using System.Runtime.CompilerServices;

  using UnityEngine;
  using static UnityEngine.Mathf;
  using V2 = UnityEngine.Vector2;
  using V3 = UnityEngine.Vector3;
  using V4 = UnityEngine.Vector4;


  public static class VectorExtensions {

    // Swizzles generated using:
    // https://www.mathsisfun.com/combinatorics/combinations-permutations-calculator.html


    // *********************** Vector2 *********************** //


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLongerThan(this V2 v, V2 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetLen(this V2 v, float length) => v.normalized * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetLenSafe(this V2 v, float length) => (v == V2.zero ? V2.right : v.normalized) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetLenSafe(this V2 v, float length, V2 fallbackTarget) => (v == V2.zero ? fallbackTarget : v).normalized * length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddLen(this V2 v, float addition) => v.normalized * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddLenSafe(this V2 v, float addition) => (v == V2.zero ? V2.right : v.normalized) * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddLenSafe(this V2 v, float addition, V2 fallbackTarget) => (v == V2.zero ? fallbackTarget : v).normalized * (v.magnitude + addition);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetDir(this V2 v, V2 d) => d.normalized * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetDirSafe(this V2 v, V2 d) => (d == V2.zero ? V2.right : d.normalized) * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetDirSafe(this V2 v, V2 d, V2 fallbackTarget) => (d == V2.zero ? fallbackTarget : d).normalized * v.magnitude;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetAngle(this V2 v, float degrees) => Quaternion.Euler(0, 0, degrees) * new V2(v.magnitude, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Angle(this V2 v) => Rad2Deg * Atan2(v.x, v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Add(this V2 v, float b) => new V2(v.x + b, v.y + b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Ray2D RayTo(this V2 v, V2 vector) => new Ray2D(v, (vector - v).normalized);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xx(this V2 v) => new V2(v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xy(this V2 v) => new V2(v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yx(this V2 v) => new V2(v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yy(this V2 v) => new V2(v.y, v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxx(this V2 v) => new V3(v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxy(this V2 v) => new V3(v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyx(this V2 v) => new V3(v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyy(this V2 v) => new V3(v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxx(this V2 v) => new V3(v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxy(this V2 v) => new V3(v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyx(this V2 v) => new V3(v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyy(this V2 v) => new V3(v.y, v.y, v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxx(this V2 v) => new V4(v.x, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxy(this V2 v) => new V4(v.x, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyx(this V2 v) => new V4(v.x, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyy(this V2 v) => new V4(v.x, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxx(this V2 v) => new V4(v.x, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxy(this V2 v) => new V4(v.x, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyx(this V2 v) => new V4(v.x, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyy(this V2 v) => new V4(v.x, v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxx(this V2 v) => new V4(v.y, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxy(this V2 v) => new V4(v.y, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyx(this V2 v) => new V4(v.y, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyy(this V2 v) => new V4(v.y, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxx(this V2 v) => new V4(v.y, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxy(this V2 v) => new V4(v.y, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyx(this V2 v) => new V4(v.y, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyy(this V2 v) => new V4(v.y, v.y, v.y, v.y);



    // *********************** Vector3 *********************** //


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLongerThan(this V3 v, V3 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetLen(this V3 v, float length) => v.normalized * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetLenSafe(this V3 v, float length) => (v == V3.zero ? V3.right : v.normalized) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetLenSafe(this V3 v, float length, V3 fallbackTarget) => (v == V3.zero ? fallbackTarget : v).normalized * length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddLen(this V3 v, float addition) => v.normalized * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddLenSafe(this V3 v, float addition) => (v == V3.zero ? V3.right : v) * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddLenSafe(this V3 v, float addition, V3 fallbackTarget) => (v == V3.zero ? fallbackTarget : v) * (v.magnitude + addition);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetDir(this V3 v, V3 d) => d.normalized * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetDirSafe(this V3 v, V3 d) => (d == V3.zero ? V3.right : d.normalized) * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetDirSafe(this V3 v, V3 d, V3 fallbackTarget) => (d == V3.zero ? fallbackTarget : d).normalized * v.magnitude;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V3 v, float b) => new V3(v.x + b, v.y + b, v.z + b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Ray RayTo(this V3 v, V3 vector) => new Ray(v, (vector - v).normalized);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xx(this V3 v) => new V2(v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xy(this V3 v) => new V2(v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xz(this V3 v) => new V2(v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yx(this V3 v) => new V2(v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yy(this V3 v) => new V2(v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yz(this V3 v) => new V2(v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zx(this V3 v) => new V2(v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zy(this V3 v) => new V2(v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zz(this V3 v) => new V2(v.z, v.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxx(this V3 v) => new V3(v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxy(this V3 v) => new V3(v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxz(this V3 v) => new V3(v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyx(this V3 v) => new V3(v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyy(this V3 v) => new V3(v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyz(this V3 v) => new V3(v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzx(this V3 v) => new V3(v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzy(this V3 v) => new V3(v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzz(this V3 v) => new V3(v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxx(this V3 v) => new V3(v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxy(this V3 v) => new V3(v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxz(this V3 v) => new V3(v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyx(this V3 v) => new V3(v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyy(this V3 v) => new V3(v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyz(this V3 v) => new V3(v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzx(this V3 v) => new V3(v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzy(this V3 v) => new V3(v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzz(this V3 v) => new V3(v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxx(this V3 v) => new V3(v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxy(this V3 v) => new V3(v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxz(this V3 v) => new V3(v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyx(this V3 v) => new V3(v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyy(this V3 v) => new V3(v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyz(this V3 v) => new V3(v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzx(this V3 v) => new V3(v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzy(this V3 v) => new V3(v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzz(this V3 v) => new V3(v.z, v.z, v.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxx(this V3 v) => new V4(v.x, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxy(this V3 v) => new V4(v.x, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxz(this V3 v) => new V4(v.x, v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyx(this V3 v) => new V4(v.x, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyy(this V3 v) => new V4(v.x, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyz(this V3 v) => new V4(v.x, v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzx(this V3 v) => new V4(v.x, v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzy(this V3 v) => new V4(v.x, v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzz(this V3 v) => new V4(v.x, v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxx(this V3 v) => new V4(v.x, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxy(this V3 v) => new V4(v.x, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxz(this V3 v) => new V4(v.x, v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyx(this V3 v) => new V4(v.x, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyy(this V3 v) => new V4(v.x, v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyz(this V3 v) => new V4(v.x, v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzx(this V3 v) => new V4(v.x, v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzy(this V3 v) => new V4(v.x, v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzz(this V3 v) => new V4(v.x, v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxx(this V3 v) => new V4(v.x, v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxy(this V3 v) => new V4(v.x, v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxz(this V3 v) => new V4(v.x, v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyx(this V3 v) => new V4(v.x, v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyy(this V3 v) => new V4(v.x, v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyz(this V3 v) => new V4(v.x, v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzx(this V3 v) => new V4(v.x, v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzy(this V3 v) => new V4(v.x, v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzz(this V3 v) => new V4(v.x, v.z, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxx(this V3 v) => new V4(v.y, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxy(this V3 v) => new V4(v.y, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxz(this V3 v) => new V4(v.y, v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyx(this V3 v) => new V4(v.y, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyy(this V3 v) => new V4(v.y, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyz(this V3 v) => new V4(v.y, v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzx(this V3 v) => new V4(v.y, v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzy(this V3 v) => new V4(v.y, v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzz(this V3 v) => new V4(v.y, v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxx(this V3 v) => new V4(v.y, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxy(this V3 v) => new V4(v.y, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxz(this V3 v) => new V4(v.y, v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyx(this V3 v) => new V4(v.y, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyy(this V3 v) => new V4(v.y, v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyz(this V3 v) => new V4(v.y, v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzx(this V3 v) => new V4(v.y, v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzy(this V3 v) => new V4(v.y, v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzz(this V3 v) => new V4(v.y, v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxx(this V3 v) => new V4(v.y, v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxy(this V3 v) => new V4(v.y, v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxz(this V3 v) => new V4(v.y, v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyx(this V3 v) => new V4(v.y, v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyy(this V3 v) => new V4(v.y, v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyz(this V3 v) => new V4(v.y, v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzx(this V3 v) => new V4(v.y, v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzy(this V3 v) => new V4(v.y, v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzz(this V3 v) => new V4(v.y, v.z, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxx(this V3 v) => new V4(v.z, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxy(this V3 v) => new V4(v.z, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxz(this V3 v) => new V4(v.z, v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyx(this V3 v) => new V4(v.z, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyy(this V3 v) => new V4(v.z, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyz(this V3 v) => new V4(v.z, v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzx(this V3 v) => new V4(v.z, v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzy(this V3 v) => new V4(v.z, v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzz(this V3 v) => new V4(v.z, v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxx(this V3 v) => new V4(v.z, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxy(this V3 v) => new V4(v.z, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxz(this V3 v) => new V4(v.z, v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyx(this V3 v) => new V4(v.z, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyy(this V3 v) => new V4(v.z, v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyz(this V3 v) => new V4(v.z, v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzx(this V3 v) => new V4(v.z, v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzy(this V3 v) => new V4(v.z, v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzz(this V3 v) => new V4(v.z, v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxx(this V3 v) => new V4(v.z, v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxy(this V3 v) => new V4(v.z, v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxz(this V3 v) => new V4(v.z, v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyx(this V3 v) => new V4(v.z, v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyy(this V3 v) => new V4(v.z, v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyz(this V3 v) => new V4(v.z, v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzx(this V3 v) => new V4(v.z, v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzy(this V3 v) => new V4(v.z, v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzz(this V3 v) => new V4(v.z, v.z, v.z, v.z);



    // *********************** Vector4 *********************** //


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLongerThan(this V4 v, V4 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetLen(this V4 v, float length) => v.normalized * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetLenSafe(this V4 v, float length) => (v == V4.zero ? new V4(1, 0, 0, 0) : v.normalized) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetLenSafe(this V4 v, float length, V4 fallbackTarget) => (v == V4.zero ? fallbackTarget : v).normalized * length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 AddLen(this V4 v, float addition) => v.normalized * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 AddLenSafe(this V4 v, float addition) => (v == V4.zero ? new V4(1, 0, 0, 0) : v) * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 AddLenSafe(this V4 v, float addition, V4 fallbackTarget) => (v == V4.zero ? fallbackTarget : v) * (v.magnitude + addition);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetDir(this V4 v, V4 d) => d.normalized * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetDirSafe(this V4 v, V4 d) => (d == V4.zero ? new V4(1, 0, 0, 0) : d.normalized) * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetDirSafe(this V4 v, V4 d, V4 fallbackTarget) => (d == V4.zero ? fallbackTarget : d).normalized * v.magnitude;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Add(this V4 v, float b) => new V4(v.x + b, v.y + b, v.z + b, v.w + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xx(this V4 v) => new V2(v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xy(this V4 v) => new V2(v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xz(this V4 v) => new V2(v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xw(this V4 v) => new V2(v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yx(this V4 v) => new V2(v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yy(this V4 v) => new V2(v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yz(this V4 v) => new V2(v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yw(this V4 v) => new V2(v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zx(this V4 v) => new V2(v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zy(this V4 v) => new V2(v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zz(this V4 v) => new V2(v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zw(this V4 v) => new V2(v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 wx(this V4 v) => new V2(v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 wy(this V4 v) => new V2(v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 wz(this V4 v) => new V2(v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 ww(this V4 v) => new V2(v.w, v.w);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxx(this V4 v) => new V3(v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxy(this V4 v) => new V3(v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxz(this V4 v) => new V3(v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxw(this V4 v) => new V3(v.x, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyx(this V4 v) => new V3(v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyy(this V4 v) => new V3(v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyz(this V4 v) => new V3(v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyw(this V4 v) => new V3(v.x, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzx(this V4 v) => new V3(v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzy(this V4 v) => new V3(v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzz(this V4 v) => new V3(v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzw(this V4 v) => new V3(v.x, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xwx(this V4 v) => new V3(v.x, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xwy(this V4 v) => new V3(v.x, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xwz(this V4 v) => new V3(v.x, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xww(this V4 v) => new V3(v.x, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxx(this V4 v) => new V3(v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxy(this V4 v) => new V3(v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxz(this V4 v) => new V3(v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxw(this V4 v) => new V3(v.y, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyx(this V4 v) => new V3(v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyy(this V4 v) => new V3(v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyz(this V4 v) => new V3(v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyw(this V4 v) => new V3(v.y, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzx(this V4 v) => new V3(v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzy(this V4 v) => new V3(v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzz(this V4 v) => new V3(v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzw(this V4 v) => new V3(v.y, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ywx(this V4 v) => new V3(v.y, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ywy(this V4 v) => new V3(v.y, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ywz(this V4 v) => new V3(v.y, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yww(this V4 v) => new V3(v.y, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxx(this V4 v) => new V3(v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxy(this V4 v) => new V3(v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxz(this V4 v) => new V3(v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxw(this V4 v) => new V3(v.z, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyx(this V4 v) => new V3(v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyy(this V4 v) => new V3(v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyz(this V4 v) => new V3(v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyw(this V4 v) => new V3(v.z, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzx(this V4 v) => new V3(v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzy(this V4 v) => new V3(v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzz(this V4 v) => new V3(v.z, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzw(this V4 v) => new V3(v.z, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zwx(this V4 v) => new V3(v.z, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zwy(this V4 v) => new V3(v.z, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zwz(this V4 v) => new V3(v.z, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zww(this V4 v) => new V3(v.z, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wxx(this V4 v) => new V3(v.w, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wxy(this V4 v) => new V3(v.w, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wxz(this V4 v) => new V3(v.w, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wxw(this V4 v) => new V3(v.w, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wyx(this V4 v) => new V3(v.w, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wyy(this V4 v) => new V3(v.w, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wyz(this V4 v) => new V3(v.w, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wyw(this V4 v) => new V3(v.w, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wzx(this V4 v) => new V3(v.w, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wzy(this V4 v) => new V3(v.w, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wzz(this V4 v) => new V3(v.w, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wzw(this V4 v) => new V3(v.w, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wwx(this V4 v) => new V3(v.w, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wwy(this V4 v) => new V3(v.w, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wwz(this V4 v) => new V3(v.w, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 www(this V4 v) => new V3(v.w, v.w, v.w);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxx(this V4 v) => new V4(v.x, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxy(this V4 v) => new V4(v.x, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxz(this V4 v) => new V4(v.x, v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxw(this V4 v) => new V4(v.x, v.x, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyx(this V4 v) => new V4(v.x, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyy(this V4 v) => new V4(v.x, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyz(this V4 v) => new V4(v.x, v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyw(this V4 v) => new V4(v.x, v.x, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzx(this V4 v) => new V4(v.x, v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzy(this V4 v) => new V4(v.x, v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzz(this V4 v) => new V4(v.x, v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzw(this V4 v) => new V4(v.x, v.x, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxwx(this V4 v) => new V4(v.x, v.x, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxwy(this V4 v) => new V4(v.x, v.x, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxwz(this V4 v) => new V4(v.x, v.x, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxww(this V4 v) => new V4(v.x, v.x, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxx(this V4 v) => new V4(v.x, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxy(this V4 v) => new V4(v.x, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxz(this V4 v) => new V4(v.x, v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxw(this V4 v) => new V4(v.x, v.y, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyx(this V4 v) => new V4(v.x, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyy(this V4 v) => new V4(v.x, v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyz(this V4 v) => new V4(v.x, v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyw(this V4 v) => new V4(v.x, v.y, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzx(this V4 v) => new V4(v.x, v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzy(this V4 v) => new V4(v.x, v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzz(this V4 v) => new V4(v.x, v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzw(this V4 v) => new V4(v.x, v.y, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xywx(this V4 v) => new V4(v.x, v.y, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xywy(this V4 v) => new V4(v.x, v.y, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xywz(this V4 v) => new V4(v.x, v.y, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyww(this V4 v) => new V4(v.x, v.y, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxx(this V4 v) => new V4(v.x, v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxy(this V4 v) => new V4(v.x, v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxz(this V4 v) => new V4(v.x, v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxw(this V4 v) => new V4(v.x, v.z, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyx(this V4 v) => new V4(v.x, v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyy(this V4 v) => new V4(v.x, v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyz(this V4 v) => new V4(v.x, v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyw(this V4 v) => new V4(v.x, v.z, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzx(this V4 v) => new V4(v.x, v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzy(this V4 v) => new V4(v.x, v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzz(this V4 v) => new V4(v.x, v.z, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzw(this V4 v) => new V4(v.x, v.z, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzwx(this V4 v) => new V4(v.x, v.z, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzwy(this V4 v) => new V4(v.x, v.z, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzwz(this V4 v) => new V4(v.x, v.z, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzww(this V4 v) => new V4(v.x, v.z, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwxx(this V4 v) => new V4(v.x, v.w, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwxy(this V4 v) => new V4(v.x, v.w, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwxz(this V4 v) => new V4(v.x, v.w, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwxw(this V4 v) => new V4(v.x, v.w, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwyx(this V4 v) => new V4(v.x, v.w, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwyy(this V4 v) => new V4(v.x, v.w, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwyz(this V4 v) => new V4(v.x, v.w, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwyw(this V4 v) => new V4(v.x, v.w, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwzx(this V4 v) => new V4(v.x, v.w, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwzy(this V4 v) => new V4(v.x, v.w, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwzz(this V4 v) => new V4(v.x, v.w, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwzw(this V4 v) => new V4(v.x, v.w, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwwx(this V4 v) => new V4(v.x, v.w, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwwy(this V4 v) => new V4(v.x, v.w, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwwz(this V4 v) => new V4(v.x, v.w, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwww(this V4 v) => new V4(v.x, v.w, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxx(this V4 v) => new V4(v.y, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxy(this V4 v) => new V4(v.y, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxz(this V4 v) => new V4(v.y, v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxw(this V4 v) => new V4(v.y, v.x, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyx(this V4 v) => new V4(v.y, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyy(this V4 v) => new V4(v.y, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyz(this V4 v) => new V4(v.y, v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyw(this V4 v) => new V4(v.y, v.x, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzx(this V4 v) => new V4(v.y, v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzy(this V4 v) => new V4(v.y, v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzz(this V4 v) => new V4(v.y, v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzw(this V4 v) => new V4(v.y, v.x, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxwx(this V4 v) => new V4(v.y, v.x, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxwy(this V4 v) => new V4(v.y, v.x, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxwz(this V4 v) => new V4(v.y, v.x, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxww(this V4 v) => new V4(v.y, v.x, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxx(this V4 v) => new V4(v.y, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxy(this V4 v) => new V4(v.y, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxz(this V4 v) => new V4(v.y, v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxw(this V4 v) => new V4(v.y, v.y, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyx(this V4 v) => new V4(v.y, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyy(this V4 v) => new V4(v.y, v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyz(this V4 v) => new V4(v.y, v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyw(this V4 v) => new V4(v.y, v.y, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzx(this V4 v) => new V4(v.y, v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzy(this V4 v) => new V4(v.y, v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzz(this V4 v) => new V4(v.y, v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzw(this V4 v) => new V4(v.y, v.y, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yywx(this V4 v) => new V4(v.y, v.y, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yywy(this V4 v) => new V4(v.y, v.y, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yywz(this V4 v) => new V4(v.y, v.y, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyww(this V4 v) => new V4(v.y, v.y, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxx(this V4 v) => new V4(v.y, v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxy(this V4 v) => new V4(v.y, v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxz(this V4 v) => new V4(v.y, v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxw(this V4 v) => new V4(v.y, v.z, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyx(this V4 v) => new V4(v.y, v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyy(this V4 v) => new V4(v.y, v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyz(this V4 v) => new V4(v.y, v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyw(this V4 v) => new V4(v.y, v.z, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzx(this V4 v) => new V4(v.y, v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzy(this V4 v) => new V4(v.y, v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzz(this V4 v) => new V4(v.y, v.z, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzw(this V4 v) => new V4(v.y, v.z, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzwx(this V4 v) => new V4(v.y, v.z, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzwy(this V4 v) => new V4(v.y, v.z, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzwz(this V4 v) => new V4(v.y, v.z, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzww(this V4 v) => new V4(v.y, v.z, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywxx(this V4 v) => new V4(v.y, v.w, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywxy(this V4 v) => new V4(v.y, v.w, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywxz(this V4 v) => new V4(v.y, v.w, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywxw(this V4 v) => new V4(v.y, v.w, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywyx(this V4 v) => new V4(v.y, v.w, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywyy(this V4 v) => new V4(v.y, v.w, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywyz(this V4 v) => new V4(v.y, v.w, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywyw(this V4 v) => new V4(v.y, v.w, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywzx(this V4 v) => new V4(v.y, v.w, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywzy(this V4 v) => new V4(v.y, v.w, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywzz(this V4 v) => new V4(v.y, v.w, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywzw(this V4 v) => new V4(v.y, v.w, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywwx(this V4 v) => new V4(v.y, v.w, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywwy(this V4 v) => new V4(v.y, v.w, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywwz(this V4 v) => new V4(v.y, v.w, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywww(this V4 v) => new V4(v.y, v.w, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxx(this V4 v) => new V4(v.z, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxy(this V4 v) => new V4(v.z, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxz(this V4 v) => new V4(v.z, v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxw(this V4 v) => new V4(v.z, v.x, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyx(this V4 v) => new V4(v.z, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyy(this V4 v) => new V4(v.z, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyz(this V4 v) => new V4(v.z, v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyw(this V4 v) => new V4(v.z, v.x, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzx(this V4 v) => new V4(v.z, v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzy(this V4 v) => new V4(v.z, v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzz(this V4 v) => new V4(v.z, v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzw(this V4 v) => new V4(v.z, v.x, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxwx(this V4 v) => new V4(v.z, v.x, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxwy(this V4 v) => new V4(v.z, v.x, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxwz(this V4 v) => new V4(v.z, v.x, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxww(this V4 v) => new V4(v.z, v.x, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxx(this V4 v) => new V4(v.z, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxy(this V4 v) => new V4(v.z, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxz(this V4 v) => new V4(v.z, v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxw(this V4 v) => new V4(v.z, v.y, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyx(this V4 v) => new V4(v.z, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyy(this V4 v) => new V4(v.z, v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyz(this V4 v) => new V4(v.z, v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyw(this V4 v) => new V4(v.z, v.y, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzx(this V4 v) => new V4(v.z, v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzy(this V4 v) => new V4(v.z, v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzz(this V4 v) => new V4(v.z, v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzw(this V4 v) => new V4(v.z, v.y, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zywx(this V4 v) => new V4(v.z, v.y, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zywy(this V4 v) => new V4(v.z, v.y, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zywz(this V4 v) => new V4(v.z, v.y, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyww(this V4 v) => new V4(v.z, v.y, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxx(this V4 v) => new V4(v.z, v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxy(this V4 v) => new V4(v.z, v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxz(this V4 v) => new V4(v.z, v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxw(this V4 v) => new V4(v.z, v.z, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyx(this V4 v) => new V4(v.z, v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyy(this V4 v) => new V4(v.z, v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyz(this V4 v) => new V4(v.z, v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyw(this V4 v) => new V4(v.z, v.z, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzx(this V4 v) => new V4(v.z, v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzy(this V4 v) => new V4(v.z, v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzz(this V4 v) => new V4(v.z, v.z, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzw(this V4 v) => new V4(v.z, v.z, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzwx(this V4 v) => new V4(v.z, v.z, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzwy(this V4 v) => new V4(v.z, v.z, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzwz(this V4 v) => new V4(v.z, v.z, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzww(this V4 v) => new V4(v.z, v.z, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwxx(this V4 v) => new V4(v.z, v.w, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwxy(this V4 v) => new V4(v.z, v.w, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwxz(this V4 v) => new V4(v.z, v.w, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwxw(this V4 v) => new V4(v.z, v.w, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwyx(this V4 v) => new V4(v.z, v.w, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwyy(this V4 v) => new V4(v.z, v.w, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwyz(this V4 v) => new V4(v.z, v.w, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwyw(this V4 v) => new V4(v.z, v.w, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwzx(this V4 v) => new V4(v.z, v.w, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwzy(this V4 v) => new V4(v.z, v.w, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwzz(this V4 v) => new V4(v.z, v.w, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwzw(this V4 v) => new V4(v.z, v.w, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwwx(this V4 v) => new V4(v.z, v.w, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwwy(this V4 v) => new V4(v.z, v.w, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwwz(this V4 v) => new V4(v.z, v.w, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwww(this V4 v) => new V4(v.z, v.w, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxxx(this V4 v) => new V4(v.w, v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxxy(this V4 v) => new V4(v.w, v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxxz(this V4 v) => new V4(v.w, v.x, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxxw(this V4 v) => new V4(v.w, v.x, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxyx(this V4 v) => new V4(v.w, v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxyy(this V4 v) => new V4(v.w, v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxyz(this V4 v) => new V4(v.w, v.x, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxyw(this V4 v) => new V4(v.w, v.x, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxzx(this V4 v) => new V4(v.w, v.x, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxzy(this V4 v) => new V4(v.w, v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxzz(this V4 v) => new V4(v.w, v.x, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxzw(this V4 v) => new V4(v.w, v.x, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxwx(this V4 v) => new V4(v.w, v.x, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxwy(this V4 v) => new V4(v.w, v.x, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxwz(this V4 v) => new V4(v.w, v.x, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxww(this V4 v) => new V4(v.w, v.x, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyxx(this V4 v) => new V4(v.w, v.y, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyxy(this V4 v) => new V4(v.w, v.y, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyxz(this V4 v) => new V4(v.w, v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyxw(this V4 v) => new V4(v.w, v.y, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyyx(this V4 v) => new V4(v.w, v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyyy(this V4 v) => new V4(v.w, v.y, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyyz(this V4 v) => new V4(v.w, v.y, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyyw(this V4 v) => new V4(v.w, v.y, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyzx(this V4 v) => new V4(v.w, v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyzy(this V4 v) => new V4(v.w, v.y, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyzz(this V4 v) => new V4(v.w, v.y, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyzw(this V4 v) => new V4(v.w, v.y, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wywx(this V4 v) => new V4(v.w, v.y, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wywy(this V4 v) => new V4(v.w, v.y, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wywz(this V4 v) => new V4(v.w, v.y, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyww(this V4 v) => new V4(v.w, v.y, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzxx(this V4 v) => new V4(v.w, v.z, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzxy(this V4 v) => new V4(v.w, v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzxz(this V4 v) => new V4(v.w, v.z, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzxw(this V4 v) => new V4(v.w, v.z, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzyx(this V4 v) => new V4(v.w, v.z, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzyy(this V4 v) => new V4(v.w, v.z, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzyz(this V4 v) => new V4(v.w, v.z, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzyw(this V4 v) => new V4(v.w, v.z, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzzx(this V4 v) => new V4(v.w, v.z, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzzy(this V4 v) => new V4(v.w, v.z, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzzz(this V4 v) => new V4(v.w, v.z, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzzw(this V4 v) => new V4(v.w, v.z, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzwx(this V4 v) => new V4(v.w, v.z, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzwy(this V4 v) => new V4(v.w, v.z, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzwz(this V4 v) => new V4(v.w, v.z, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzww(this V4 v) => new V4(v.w, v.z, v.w, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwxx(this V4 v) => new V4(v.w, v.w, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwxy(this V4 v) => new V4(v.w, v.w, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwxz(this V4 v) => new V4(v.w, v.w, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwxw(this V4 v) => new V4(v.w, v.w, v.x, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwyx(this V4 v) => new V4(v.w, v.w, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwyy(this V4 v) => new V4(v.w, v.w, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwyz(this V4 v) => new V4(v.w, v.w, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwyw(this V4 v) => new V4(v.w, v.w, v.y, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwzx(this V4 v) => new V4(v.w, v.w, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwzy(this V4 v) => new V4(v.w, v.w, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwzz(this V4 v) => new V4(v.w, v.w, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwzw(this V4 v) => new V4(v.w, v.w, v.z, v.w);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwwx(this V4 v) => new V4(v.w, v.w, v.w, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwwy(this V4 v) => new V4(v.w, v.w, v.w, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwwz(this V4 v) => new V4(v.w, v.w, v.w, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwww(this V4 v) => new V4(v.w, v.w, v.w, v.w);
  }
}