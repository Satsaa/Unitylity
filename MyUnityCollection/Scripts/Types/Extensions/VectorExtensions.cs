using UnityEngine;
using Unity.Mathematics;
using System.Runtime.CompilerServices;

namespace Muc.Types.Extensions {

  public static class VectorExtensions {

    // *********************** float2 *********************** //


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 xo(this float2 v) => new float2(v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 ox(this float2 v) => new float2(0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 oy(this float2 v) => new float2(0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 yo(this float2 v) => new float2(v.y, 0);
    // [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 oo(this float2 v) => new float2(0, 0);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oxx(this float2 v) => new float3(0, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xox(this float2 v) => new float3(v.x, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xxo(this float2 v) => new float3(v.x, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oxo(this float2 v) => new float3(0, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oox(this float2 v) => new float3(0, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xoo(this float2 v) => new float3(v.x, 0, 0);
    // [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ooo(this float2 v) => new float3(0, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oyy(this float2 v) => new float3(0, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yoy(this float2 v) => new float3(v.y, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yyo(this float2 v) => new float3(v.y, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oyo(this float2 v) => new float3(0, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ooy(this float2 v) => new float3(0, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yoo(this float2 v) => new float3(v.y, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xyo(this float2 v) => new float3(v.x, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oxy(this float2 v) => new float3(0, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yox(this float2 v) => new float3(v.y, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oyx(this float2 v) => new float3(0, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xoy(this float2 v) => new float3(v.x, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yxo(this float2 v) => new float3(v.y, v.x, 0);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 Add(this float2 v, Vector3 d) => new float2(v.x + d.x, v.y + d.y);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 AddX(this float2 v, float b) => new float2(v.x + b, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 AddY(this float2 v, float b) => new float2(v.x, v.y + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 SetX(this float2 v, float b) => new float2(b, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 SetY(this float2 v, float b) => new float2(v.x, b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool LongerThan(this float2 v, float2 smaller) => math.lengthsq(v) > math.lengthsq(smaller);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 SetLen(this float2 v, float length) => math.normalize(v) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 SetLenSafe(this float2 v, float length, float2 fallbackTarget = default(float2)) => math.normalizesafe(v, fallbackTarget) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 AddLen(this float2 v, float addition) => math.normalize(v) * (math.length(v) + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 AddLenSafe(this float2 v, float addition, float2 fallbackTarget = default(float2)) => math.normalizesafe(v, fallbackTarget) * (math.length(v) + addition);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 SetAngle(this float2 v, float degrees) => math.rotate(quaternion.EulerXYZ(0, 0, math.radians(degrees)), new float3(math.length(v), 0, 0)).xy;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 SetAngleRadians(this float2 v, float radians) => math.rotate(quaternion.EulerXYZ(0, 0, radians), new float3(math.length(v), 0, 0)).xy;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Angle(this float2 v) => math.degrees(math.atan2(v.x, v.y));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float AngleRad(this float2 v) => math.atan2(v.x, v.y);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 SetDir(this float2 v, float2 d) => math.normalize(d * math.length(v));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 SetDirSafe(this float2 v, float2 d, float2 fallbackTarget = default(float2)) => math.normalizesafe(d, fallbackTarget) * math.length(v);


    // *********************** float3 *********************** //


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 xo(this float3 v) => new float2(v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 ox(this float3 v) => new float2(0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 oy(this float3 v) => new float2(0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 yo(this float3 v) => new float2(v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 zo(this float3 v) => new float2(v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 oz(this float3 v) => new float2(0, v.z);
    // [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float2 oo(this float3 v) => new float2(0, 0);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oxx(this float3 v) => new float3(0, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xox(this float3 v) => new float3(v.x, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xxo(this float3 v) => new float3(v.x, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oxo(this float3 v) => new float3(0, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oox(this float3 v) => new float3(0, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xoo(this float3 v) => new float3(v.x, 0, 0);
    // [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ooo(this float3 v) => new float3(0, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oyy(this float3 v) => new float3(0, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yoy(this float3 v) => new float3(v.y, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yyo(this float3 v) => new float3(v.y, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oyo(this float3 v) => new float3(0, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ooy(this float3 v) => new float3(0, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yoo(this float3 v) => new float3(v.y, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ozz(this float3 v) => new float3(0, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 zoz(this float3 v) => new float3(v.z, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 zzo(this float3 v) => new float3(v.z, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ozo(this float3 v) => new float3(0, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ooz(this float3 v) => new float3(0, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 zoo(this float3 v) => new float3(v.z, 0, 0);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xyo(this float3 v) => new float3(v.x, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oxy(this float3 v) => new float3(0, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yox(this float3 v) => new float3(v.y, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oyx(this float3 v) => new float3(0, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xoy(this float3 v) => new float3(v.x, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yxo(this float3 v) => new float3(v.y, v.x, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xzo(this float3 v) => new float3(v.x, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oxz(this float3 v) => new float3(0, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 zox(this float3 v) => new float3(v.z, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ozx(this float3 v) => new float3(0, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 xoz(this float3 v) => new float3(v.x, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 zxo(this float3 v) => new float3(v.y, v.x, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 zyo(this float3 v) => new float3(v.z, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 ozy(this float3 v) => new float3(0, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yoz(this float3 v) => new float3(v.y, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 oyz(this float3 v) => new float3(0, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 zoy(this float3 v) => new float3(v.z, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 yzo(this float3 v) => new float3(v.y, v.z, 0);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 Add(this float3 v, Vector2 d) => new float3(v.x + d.x, v.y + d.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 Add(this float3 v, Vector3 d) => new float3(v.x + d.x, v.y + d.y, v.z + d.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 Add(this float3 v, float2 b) => new float3(v.x + b.x, v.y + b.y, v.z);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddX(this float3 v, float b) => new float3(v.x + b, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddY(this float3 v, float b) => new float3(v.x, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddZ(this float3 v, float b) => new float3(v.x, v.y, v.z + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddXY(this float3 v, float2 b) => new float3(v.x + b.x, v.y + b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddYX(this float3 v, float2 b) => new float3(v.x + b.y, v.y + b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddYZ(this float3 v, float2 b) => new float3(v.x, v.y + b.x, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddZY(this float3 v, float2 b) => new float3(v.x, v.y + b.y, v.z + b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddXZ(this float3 v, float2 b) => new float3(v.x + b.x, v.y, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddZX(this float3 v, float2 b) => new float3(v.x + b.y, v.y, v.z + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddXY(this float3 v, Vector2 b) => new float3(v.x + b.x, v.y + b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddYX(this float3 v, Vector2 b) => new float3(v.x + b.y, v.y + b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddYZ(this float3 v, Vector2 b) => new float3(v.x, v.y + b.x, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddZY(this float3 v, Vector2 b) => new float3(v.x, v.y + b.y, v.z + b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddXZ(this float3 v, Vector2 b) => new float3(v.x + b.x, v.y, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddZX(this float3 v, Vector2 b) => new float3(v.x + b.y, v.y, v.z + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddXY(this float3 v, float b) => new float3(v.x + b, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddYX(this float3 v, float b) => new float3(v.x + b, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddYZ(this float3 v, float b) => new float3(v.x, v.y + b, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddZY(this float3 v, float b) => new float3(v.x, v.y + b, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddXZ(this float3 v, float b) => new float3(v.x + b, v.y, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddZX(this float3 v, float b) => new float3(v.x + b, v.y, v.z + b);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetX(this float3 v, float b) => new float3(b, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetY(this float3 v, float b) => new float3(v.x, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZ(this float3 v, float b) => new float3(v.x, v.y, b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetXY(this float3 v, float2 b) => new float3(b.x, b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetYX(this float3 v, float2 b) => new float3(b.y, b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetYZ(this float3 v, float2 b) => new float3(v.x, b.x, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZY(this float3 v, float2 b) => new float3(v.x, b.y, b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetXZ(this float3 v, float2 b) => new float3(b.x, v.y, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZX(this float3 v, float2 b) => new float3(b.y, v.y, b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetXY(this float3 v, Vector2 b) => new float3(b.x, b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetYX(this float3 v, Vector2 b) => new float3(b.y, b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetYZ(this float3 v, Vector2 b) => new float3(v.x, b.x, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZY(this float3 v, Vector2 b) => new float3(v.x, b.y, b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetXZ(this float3 v, Vector2 b) => new float3(b.x, v.y, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZX(this float3 v, Vector2 b) => new float3(b.y, v.y, b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetXY(this float3 v, float b) => new float3(b, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetYX(this float3 v, float b) => new float3(b, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetYZ(this float3 v, float b) => new float3(v.x, b, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZY(this float3 v, float b) => new float3(v.x, b, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetXZ(this float3 v, float b) => new float3(b, v.y, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZX(this float3 v, float b) => new float3(b, v.y, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetXY(this float3 v, float b, float c) => new float3(b, c, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetYX(this float3 v, float b, float c) => new float3(b, c, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetYZ(this float3 v, float b, float c) => new float3(v.x, b, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZY(this float3 v, float b, float c) => new float3(v.x, b, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetXZ(this float3 v, float b, float c) => new float3(b, v.y, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetZX(this float3 v, float b, float c) => new float3(b, v.y, c);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool LongerThan(this float3 v, float3 smaller) => math.lengthsq(v) > math.lengthsq(smaller);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetLen(this float3 v, float length) => math.normalize(v) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetLenSafe(this float3 v, float length, float3 fallbackTarget = default(float3)) => math.normalizesafe(v, fallbackTarget) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddLen(this float3 v, float addition) => math.normalize(v) * (math.length(v) + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 AddLenSafe(this float3 v, float addition, float3 fallbackTarget = default(float3)) => math.normalizesafe(v, fallbackTarget) * (math.length(v) + addition);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetDir(this float3 v, float3 d) => math.normalize(d * math.length(v));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float3 SetDirSafe(this float3 v, float3 d, float3 fallbackTarget = default(float3)) => math.normalizesafe(d, fallbackTarget) * math.length(v);

    // *********************** Vector2 *********************** //


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool LongerThan(this Vector2 v, Vector2 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 SetLen(this Vector2 v, float length) => v.normalized * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 SetLenSafe(this Vector2 v, float length, Vector2 fallbackTarget) => math.normalizesafe(v, fallbackTarget) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddLen(this Vector2 v, float addition) => v.normalized * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddLenSafe(this Vector2 v, float addition, Vector2 fallbackTarget = default(Vector2)) => math.normalizesafe(v, fallbackTarget) * (v.magnitude + addition);


#pragma warning disable CS0618
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 SetAngle(this Vector2 v, float degrees) => Quaternion.Euler(0, 0, degrees) * new Vector2(v.magnitude, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 SetAngleRadians(this Vector2 v, float radians) => Quaternion.EulerAngles(0, 0, radians) * new Vector2(v.magnitude, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float Angle(this Vector2 v) => math.degrees(v.AngleRad());
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static float AngleRad(this Vector2 v) => math.atan2(v.x, v.y);
#pragma warning restore CS0618


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 SetDir(this Vector2 v, Vector2 d) => d.normalized * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 SetDirSafe(this Vector2 v, Vector2 d) => math.normalizesafe(d * math.length(v));



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 yx(this Vector2 v) => new Vector2(v.y, v.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xxx(this Vector2 v) => new Vector3(v.x, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yyy(this Vector2 v) => new Vector3(v.y, v.y, v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xxy(this Vector2 v) => new Vector3(v.x, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xyy(this Vector2 v) => new Vector3(v.x, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yyx(this Vector2 v) => new Vector3(v.y, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xyx(this Vector2 v) => new Vector3(v.x, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yxx(this Vector2 v) => new Vector3(v.y, v.x, v.x);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 xo(this Vector2 v) => new Vector2(v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 ox(this Vector2 v) => new Vector2(0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 oy(this Vector2 v) => new Vector2(0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 yo(this Vector2 v) => new Vector2(v.y, 0);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oxx(this Vector2 v) => new Vector3(0, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xox(this Vector2 v) => new Vector3(v.x, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xxo(this Vector2 v) => new Vector3(v.x, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oxo(this Vector2 v) => new Vector3(0, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oox(this Vector2 v) => new Vector3(0, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xoo(this Vector2 v) => new Vector3(v.x, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oyy(this Vector2 v) => new Vector3(0, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yoy(this Vector2 v) => new Vector3(v.y, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yyo(this Vector2 v) => new Vector3(v.y, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oyo(this Vector2 v) => new Vector3(0, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 ooy(this Vector2 v) => new Vector3(0, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yoo(this Vector2 v) => new Vector3(v.y, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xyo(this Vector2 v) => new Vector3(v.x, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oxy(this Vector2 v) => new Vector3(0, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yox(this Vector2 v) => new Vector3(v.y, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oyx(this Vector2 v) => new Vector3(0, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xoy(this Vector2 v) => new Vector3(v.x, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yxo(this Vector2 v) => new Vector3(v.y, v.x, 0);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 Add(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 Add(this Vector2 v, float2 b) => new Vector2(v.x + b.x, v.y + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 Add(this Vector2 v, float3 b) => new Vector3(v.x + b.x, v.y + b.y, b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 Add(this Vector2 v, Vector3 b) => new Vector3(v.x + b.x, v.y + b.y, b.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddX(this Vector2 v, float b) => new Vector2(v.x + b, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddY(this Vector2 v, float b) => new Vector2(v.x, v.y + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddXY(this Vector2 v, Vector2 b) => new Vector2(v.x + b.x, v.y + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddYX(this Vector2 v, Vector2 b) => new Vector2(v.x + b.y, v.y + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddXY(this Vector2 v, float2 b) => new Vector2(v.x + b.x, v.y + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddYX(this Vector2 v, float2 b) => new Vector2(v.x + b.y, v.y + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddXY(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 AddYX(this Vector2 v, float b) => new Vector2(v.x + b, v.y + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 SetX(this Vector2 v, float b) => new Vector2(b, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 SetY(this Vector2 v, float b) => new Vector2(v.x, b);

    // *********************** Vector3 *********************** //

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool LongerThan(this Vector3 v, Vector3 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetLen(this Vector3 v, float length) => v.normalized * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetLenSafe(this Vector3 v, float length, Vector3 fallbackTarget = default(Vector3)) => math.normalizesafe(v, fallbackTarget) * length;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddLen(this Vector3 v, float addition) => v.normalized * (v.magnitude + addition);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddLenSafe(this Vector3 v, float addition) => math.normalizesafe(v) * (v.magnitude + addition);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetDir(this Vector3 v, Vector3 d) => d.normalized * v.magnitude;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetDirSafe(this Vector3 v, Vector3 d, Vector3 fallbackTarget = default(Vector3)) => math.normalizesafe(d, fallbackTarget) * math.length(v);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 xy(this Vector3 v) => new Vector2(v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 xz(this Vector3 v) => new Vector2(v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 yz(this Vector3 v) => new Vector2(v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 yx(this Vector3 v) => new Vector2(v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 zx(this Vector3 v) => new Vector2(v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 zy(this Vector3 v) => new Vector2(v.z, v.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xzy(this Vector3 v) => new Vector3(v.x, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yzx(this Vector3 v) => new Vector3(v.y, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yxz(this Vector3 v) => new Vector3(v.y, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zxy(this Vector3 v) => new Vector3(v.z, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zyx(this Vector3 v) => new Vector3(v.z, v.y, v.x);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 xo(this Vector3 v) => new Vector2(v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 ox(this Vector3 v) => new Vector2(0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 oy(this Vector3 v) => new Vector2(0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 yo(this Vector3 v) => new Vector2(v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 zo(this Vector3 v) => new Vector2(v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector2 oz(this Vector3 v) => new Vector2(0, v.z);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oxx(this Vector3 v) => new Vector3(0, v.x, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xox(this Vector3 v) => new Vector3(v.x, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xxo(this Vector3 v) => new Vector3(v.x, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oxo(this Vector3 v) => new Vector3(0, v.x, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oox(this Vector3 v) => new Vector3(0, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xoo(this Vector3 v) => new Vector3(v.x, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oyy(this Vector3 v) => new Vector3(0, v.y, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yoy(this Vector3 v) => new Vector3(v.y, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yyo(this Vector3 v) => new Vector3(v.y, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oyo(this Vector3 v) => new Vector3(0, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 ooy(this Vector3 v) => new Vector3(0, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yoo(this Vector3 v) => new Vector3(v.y, 0, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 ozz(this Vector3 v) => new Vector3(0, v.z, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zoz(this Vector3 v) => new Vector3(v.z, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zzo(this Vector3 v) => new Vector3(v.z, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 ozo(this Vector3 v) => new Vector3(0, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 ooz(this Vector3 v) => new Vector3(0, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zoo(this Vector3 v) => new Vector3(v.z, 0, 0);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xyo(this Vector3 v) => new Vector3(v.x, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oxy(this Vector3 v) => new Vector3(0, v.x, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yox(this Vector3 v) => new Vector3(v.y, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oyx(this Vector3 v) => new Vector3(0, v.y, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xoy(this Vector3 v) => new Vector3(v.x, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yxo(this Vector3 v) => new Vector3(v.y, v.x, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xzo(this Vector3 v) => new Vector3(v.x, v.z, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oxz(this Vector3 v) => new Vector3(0, v.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zox(this Vector3 v) => new Vector3(v.z, 0, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 ozx(this Vector3 v) => new Vector3(0, v.z, v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 xoz(this Vector3 v) => new Vector3(v.x, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zxo(this Vector3 v) => new Vector3(v.y, v.x, 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zyo(this Vector3 v) => new Vector3(v.z, v.y, 0);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 ozy(this Vector3 v) => new Vector3(0, v.z, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yoz(this Vector3 v) => new Vector3(v.y, 0, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 oyz(this Vector3 v) => new Vector3(0, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 zoy(this Vector3 v) => new Vector3(v.z, 0, v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 yzo(this Vector3 v) => new Vector3(v.y, v.z, 0);



    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 Add(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 Add(this Vector3 v, Vector2 b) => new Vector3(v.x + b.x, v.y + b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 Add(this Vector3 v, Vector3 b) => new Vector3(v.x + b.x, v.y + b.y, v.z + b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 Add(this Vector3 v, float2 b) => new Vector3(v.x + b.x, v.y + b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 Add(this Vector3 v, float3 b) => new Vector3(v.x + b.x, v.y + b.y, v.z + b.z);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddX(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddY(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZ(this Vector3 v, float b) => new Vector3(v.x, v.y, v.z + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXY(this Vector3 v, Vector2 b) => new Vector3(v.x + b.x, v.y + b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYX(this Vector3 v, Vector2 b) => new Vector3(v.x + b.y, v.y + b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYZ(this Vector3 v, Vector2 b) => new Vector3(v.x, v.y + b.x, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZY(this Vector3 v, Vector2 b) => new Vector3(v.x, v.y + b.y, v.z + b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXZ(this Vector3 v, Vector2 b) => new Vector3(v.x + b.x, v.y, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZX(this Vector3 v, Vector2 b) => new Vector3(v.x + b.y, v.y, v.z + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXY(this Vector3 v, float2 b) => new Vector3(v.x + b.x, v.y + b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYX(this Vector3 v, float2 b) => new Vector3(v.x + b.y, v.y + b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYZ(this Vector3 v, float2 b) => new Vector3(v.x, v.y + b.x, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZY(this Vector3 v, float2 b) => new Vector3(v.x, v.y + b.y, v.z + b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXZ(this Vector3 v, float2 b) => new Vector3(v.x + b.x, v.y, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZX(this Vector3 v, float2 b) => new Vector3(v.x + b.y, v.y, v.z + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXY(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYX(this Vector3 v, float b) => new Vector3(v.x + b, v.y + b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYZ(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZY(this Vector3 v, float b) => new Vector3(v.x, v.y + b, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXZ(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z + b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZX(this Vector3 v, float b) => new Vector3(v.x + b, v.y, v.z + b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXZY(this Vector3 v, Vector3 b) => new Vector3(v.x + b.x, v.y + b.z, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYZX(this Vector3 v, Vector3 b) => new Vector3(v.x + b.y, v.y + b.z, v.z + b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYXZ(this Vector3 v, Vector3 b) => new Vector3(v.x + b.y, v.y + b.x, v.z + b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZXY(this Vector3 v, Vector3 b) => new Vector3(v.x + b.z, v.y + b.x, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZYX(this Vector3 v, Vector3 b) => new Vector3(v.x + b.z, v.y + b.y, v.z + b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXYZ(this Vector3 v, float3 b) => new Vector3(v.x + b.x, v.y + b.y, v.z + b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddXZY(this Vector3 v, float3 b) => new Vector3(v.x + b.x, v.y + b.z, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYZX(this Vector3 v, float3 b) => new Vector3(v.x + b.y, v.y + b.z, v.z + b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddYXZ(this Vector3 v, float3 b) => new Vector3(v.x + b.y, v.y + b.x, v.z + b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZXY(this Vector3 v, float3 b) => new Vector3(v.x + b.z, v.y + b.x, v.z + b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 AddZYX(this Vector3 v, float3 b) => new Vector3(v.x + b.z, v.y + b.y, v.z + b.x);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetX(this Vector3 v, float b) => new Vector3(b, v.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetY(this Vector3 v, float b) => new Vector3(v.x, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZ(this Vector3 v, float b) => new Vector3(v.x, v.y, b);


    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetXY(this Vector3 v, Vector2 b) => new Vector3(b.x, b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetYX(this Vector3 v, Vector2 b) => new Vector3(b.y, b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetYZ(this Vector3 v, Vector2 b) => new Vector3(v.x, b.x, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZY(this Vector3 v, Vector2 b) => new Vector3(v.x, b.y, b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetXZ(this Vector3 v, Vector2 b) => new Vector3(b.x, v.y, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZX(this Vector3 v, Vector2 b) => new Vector3(b.y, v.y, b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetXY(this Vector3 v, float2 b) => new Vector3(b.x, b.y, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetYX(this Vector3 v, float2 b) => new Vector3(b.y, b.x, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetYZ(this Vector3 v, float2 b) => new Vector3(v.x, b.x, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZY(this Vector3 v, float2 b) => new Vector3(v.x, b.y, b.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetXZ(this Vector3 v, float2 b) => new Vector3(b.x, v.y, b.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZX(this Vector3 v, float2 b) => new Vector3(b.y, v.y, b.x);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetXY(this Vector3 v, float b) => new Vector3(b, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetYX(this Vector3 v, float b) => new Vector3(b, b, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetYZ(this Vector3 v, float b) => new Vector3(v.x, b, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZY(this Vector3 v, float b) => new Vector3(v.x, b, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetXZ(this Vector3 v, float b) => new Vector3(b, v.y, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZX(this Vector3 v, float b) => new Vector3(b, v.y, b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetXY(this Vector3 v, float b, float c) => new Vector3(b, c, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetYX(this Vector3 v, float b, float c) => new Vector3(b, c, v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetYZ(this Vector3 v, float b, float c) => new Vector3(v.x, b, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZY(this Vector3 v, float b, float c) => new Vector3(v.x, b, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetXZ(this Vector3 v, float b, float c) => new Vector3(b, v.y, c);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3 SetZX(this Vector3 v, float b, float c) => new Vector3(b, v.y, c);

  }

}