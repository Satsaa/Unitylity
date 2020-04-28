

namespace Muc.Types.Extensions {

  using System.Runtime.CompilerServices;

  using UnityEngine;
  using static UnityEngine.Mathf;
  using V2 = UnityEngine.Vector2;
  using V3 = UnityEngine.Vector3;


  public static class VectorExtensions {


    // *********************** Vector2 *********************** //


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool LongerThan(this V2 v, V2 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;


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



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yx(this V2 v) => new V2(v.y, v.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxx(this V2 v) => new V3(v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyy(this V2 v) => new V3(v.y, v.y, v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxy(this V2 v) => new V3(v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyy(this V2 v) => new V3(v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyx(this V2 v) => new V3(v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyx(this V2 v) => new V3(v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxx(this V2 v) => new V3(v.y, v.x, v.x);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xo(this V2 v) => new V2(v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 ox(this V2 v) => new V2(0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 oy(this V2 v) => new V2(0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yo(this V2 v) => new V2(v.y, 0);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oxx(this V2 v) => new V3(0, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xox(this V2 v) => new V3(v.x, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxo(this V2 v) => new V3(v.x, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oxo(this V2 v) => new V3(0, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oox(this V2 v) => new V3(0, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xoo(this V2 v) => new V3(v.x, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oyy(this V2 v) => new V3(0, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yoy(this V2 v) => new V3(v.y, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyo(this V2 v) => new V3(v.y, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oyo(this V2 v) => new V3(0, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ooy(this V2 v) => new V3(0, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yoo(this V2 v) => new V3(v.y, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyo(this V2 v) => new V3(v.x, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oxy(this V2 v) => new V3(0, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yox(this V2 v) => new V3(v.y, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oyx(this V2 v) => new V3(0, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xoy(this V2 v) => new V3(v.x, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxo(this V2 v) => new V3(v.y, v.x, 0);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Add(this V2 v, float b) => new V2(v.x + b, v.y + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V2 v, V3 b) => new V3(v.x + b.x, v.y + b.y, b.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddX(this V2 v, float b) => new V2(v.x + b, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddY(this V2 v, float b) => new V2(v.x, v.y + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddXY(this V2 v, V2 b) => new V2(v.x + b.x, v.y + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddYX(this V2 v, V2 b) => new V2(v.x + b.y, v.y + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddXY(this V2 v, float b) => new V2(v.x + b, v.y + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddYX(this V2 v, float b) => new V2(v.x + b, v.y + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetX(this V2 v, float b) => new V2(b, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetY(this V2 v, float b) => new V2(v.x, b);


    // *********************** Vector3 *********************** //


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool LongerThan(this V3 v, V3 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetLen(this V3 v, float length) => v.normalized * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetLenSafe(this V3 v, float length) => (v == V3.zero ? V3.right : v.normalized) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetLenSafe(this V3 v, float length, V3 fallbackTarget) => (v == V3.zero ? fallbackTarget : v).normalized * length;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddLen(this V3 v, float addition) => v.normalized * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddLenSafe(this V3 v, float addition) => (v == V3.zero ? V3.right : v) * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddLenSafe(this V3 v, float addition, V3 fallbackTarget) => (v == V3.zero ? fallbackTarget : v) * (v.magnitude + addition);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetDir(this V3 v, V3 d) => d.normalized * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetDirSafe(this V3 v, V3 d) => (d == V3.zero ? V3.right : d.normalized) * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetDirSafe(this V3 v, V3 d, V3 fallbackTarget) => (d == V3.zero ? fallbackTarget : d).normalized * v.magnitude;



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xy(this V3 v) => new V2(v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xz(this V3 v) => new V2(v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yz(this V3 v) => new V2(v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yx(this V3 v) => new V2(v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zx(this V3 v) => new V2(v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zy(this V3 v) => new V2(v.z, v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzy(this V3 v) => new V3(v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzx(this V3 v) => new V3(v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxz(this V3 v) => new V3(v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxy(this V3 v) => new V3(v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyx(this V3 v) => new V3(v.z, v.y, v.x);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xo(this V3 v) => new V2(v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 ox(this V3 v) => new V2(0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 oy(this V3 v) => new V2(0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yo(this V3 v) => new V2(v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zo(this V3 v) => new V2(v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 oz(this V3 v) => new V2(0, v.z);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oxx(this V3 v) => new V3(0, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xox(this V3 v) => new V3(v.x, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxo(this V3 v) => new V3(v.x, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oxo(this V3 v) => new V3(0, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oox(this V3 v) => new V3(0, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xoo(this V3 v) => new V3(v.x, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oyy(this V3 v) => new V3(0, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yoy(this V3 v) => new V3(v.y, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyo(this V3 v) => new V3(v.y, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oyo(this V3 v) => new V3(0, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ooy(this V3 v) => new V3(0, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yoo(this V3 v) => new V3(v.y, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ozz(this V3 v) => new V3(0, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zoz(this V3 v) => new V3(v.z, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzo(this V3 v) => new V3(v.z, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ozo(this V3 v) => new V3(0, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ooz(this V3 v) => new V3(0, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zoo(this V3 v) => new V3(v.z, 0, 0);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyo(this V3 v) => new V3(v.x, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oxy(this V3 v) => new V3(0, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yox(this V3 v) => new V3(v.y, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oyx(this V3 v) => new V3(0, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xoy(this V3 v) => new V3(v.x, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxo(this V3 v) => new V3(v.y, v.x, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzo(this V3 v) => new V3(v.x, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oxz(this V3 v) => new V3(0, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zox(this V3 v) => new V3(v.z, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ozx(this V3 v) => new V3(0, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xoz(this V3 v) => new V3(v.x, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxo(this V3 v) => new V3(v.y, v.x, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyo(this V3 v) => new V3(v.z, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ozy(this V3 v) => new V3(0, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yoz(this V3 v) => new V3(v.y, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 oyz(this V3 v) => new V3(0, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zoy(this V3 v) => new V3(v.z, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzo(this V3 v) => new V3(v.y, v.z, 0);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V3 v, float b) => new V3(v.x + b, v.y + b, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V3 v, V2 b) => new V3(v.x + b.x, v.y + b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V3 v, V3 b) => new V3(v.x + b.x, v.y + b.y, v.z + b.z);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddX(this V3 v, float b) => new V3(v.x + b, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddY(this V3 v, float b) => new V3(v.x, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddZ(this V3 v, float b) => new V3(v.x, v.y, v.z + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddXY(this V3 v, V2 b) => new V3(v.x + b.x, v.y + b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddYX(this V3 v, V2 b) => new V3(v.x + b.y, v.y + b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddYZ(this V3 v, V2 b) => new V3(v.x, v.y + b.x, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddZY(this V3 v, V2 b) => new V3(v.x, v.y + b.y, v.z + b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddXZ(this V3 v, V2 b) => new V3(v.x + b.x, v.y, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddZX(this V3 v, V2 b) => new V3(v.x + b.y, v.y, v.z + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddXY(this V3 v, float b) => new V3(v.x + b, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddYX(this V3 v, float b) => new V3(v.x + b, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddYZ(this V3 v, float b) => new V3(v.x, v.y + b, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddZY(this V3 v, float b) => new V3(v.x, v.y + b, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddXZ(this V3 v, float b) => new V3(v.x + b, v.y, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddZX(this V3 v, float b) => new V3(v.x + b, v.y, v.z + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddXZY(this V3 v, V3 b) => new V3(v.x + b.x, v.y + b.z, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddYZX(this V3 v, V3 b) => new V3(v.x + b.y, v.y + b.z, v.z + b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddYXZ(this V3 v, V3 b) => new V3(v.x + b.y, v.y + b.x, v.z + b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddZXY(this V3 v, V3 b) => new V3(v.x + b.z, v.y + b.x, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddZYX(this V3 v, V3 b) => new V3(v.x + b.z, v.y + b.y, v.z + b.x);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetX(this V3 v, float b) => new V3(b, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetY(this V3 v, float b) => new V3(v.x, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZ(this V3 v, float b) => new V3(v.x, v.y, b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetXY(this V3 v, V2 b) => new V3(b.x, b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetYX(this V3 v, V2 b) => new V3(b.y, b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetYZ(this V3 v, V2 b) => new V3(v.x, b.x, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZY(this V3 v, V2 b) => new V3(v.x, b.y, b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetXZ(this V3 v, V2 b) => new V3(b.x, v.y, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZX(this V3 v, V2 b) => new V3(b.y, v.y, b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetXY(this V3 v, float b) => new V3(b, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetYX(this V3 v, float b) => new V3(b, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetYZ(this V3 v, float b) => new V3(v.x, b, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZY(this V3 v, float b) => new V3(v.x, b, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetXZ(this V3 v, float b) => new V3(b, v.y, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZX(this V3 v, float b) => new V3(b, v.y, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetXY(this V3 v, float b, float c) => new V3(b, c, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetYX(this V3 v, float b, float c) => new V3(b, c, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetYZ(this V3 v, float b, float c) => new V3(v.x, b, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZY(this V3 v, float b, float c) => new V3(v.x, b, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetXZ(this V3 v, float b, float c) => new V3(b, v.y, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZX(this V3 v, float b, float c) => new V3(b, v.y, c);

  }

}