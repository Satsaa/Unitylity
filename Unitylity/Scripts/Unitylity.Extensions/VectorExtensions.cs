
namespace Unitylity.Extensions {

	using System;
	using System.Runtime.CompilerServices;
	using UnityEngine;
	using V2 = UnityEngine.Vector2;
	using V2I = UnityEngine.Vector2Int;
	using V3 = UnityEngine.Vector3;
	using V3I = UnityEngine.Vector3Int;
	using V4 = UnityEngine.Vector4;

	// using V4I = UnityEngine.Vector4Int;


	public static class VectorExtensions {

		// Swizzles generated using:
		// https://www.mathsisfun.com/combinatorics/combinations-permutations-calculator.html

		// Shout out to the people at Unity for making the Mathematics library instead of expanding on the normal vectors!

		#region float
		#region Vector2

		// Util
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLongerThan(this V2 v, V2 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Ray2D RayTo(this V2 v, V2 vector) => new(v, (vector - v).normalized);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 x0y(this V2 v) => new(v.x, 0, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Expand(this V2 v, float z) => new(v.x, v.y, z);


		// Math
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I FloorInt(this V2 v) => new(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I RoundInt(this V2 v) => new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I CeilInt(this V2 v) => new(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Floor(this V2 v) => new(Mathf.Floor(v.x), Mathf.Floor(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Round(this V2 v) => new(Mathf.Round(v.x), Mathf.Round(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Ceil(this V2 v) => new(Mathf.Ceil(v.x), Mathf.Ceil(v.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sin(this V2 v) => new(Mathf.Sin(v.x), Mathf.Sin(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Cos(this V2 v) => new(Mathf.Cos(v.x), Mathf.Cos(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Tan(this V2 v) => new(Mathf.Tan(v.x), Mathf.Tan(v.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Asin(this V2 v) => new(Mathf.Asin(v.x), Mathf.Asin(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Acos(this V2 v) => new(Mathf.Acos(v.x), Mathf.Acos(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Atan(this V2 v) => new(Mathf.Atan(v.x), Mathf.Atan(v.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Abs(this V2 v) => new(Mathf.Abs(v.x), Mathf.Abs(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Clamp01(this V2 v) => new(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Exp(this V2 v) => new(Mathf.Exp(v.x), Mathf.Exp(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Log(this V2 v) => new(Mathf.Log(v.x), Mathf.Log(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Log10(this V2 v) => new(Mathf.Log10(v.x), Mathf.Log10(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sign(this V2 v) => new(Mathf.Sign(v.x), Mathf.Sign(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I SignInt(this V2 v) => new(Math.Sign(v.x), Math.Sign(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sqrt(this V2 v) => new(Mathf.Sqrt(v.x), Mathf.Sqrt(v.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Atan2(this V2 v, float b) => new(Mathf.Atan2(v.x, b), Mathf.Atan2(v.y, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Atan2(this V2 v, V2 b) => new(Mathf.Atan2(v.x, b.x), Mathf.Atan2(v.y, b.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Clamp(this V2 v, float min, float max) => new(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Clamp(this V2 v, V2 min, V2 max) => new(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Pow(this V2 v, float p) => new(Mathf.Pow(v.x, p), Mathf.Pow(v.y, p));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Pow(this V2 v, V2 p) => new(Mathf.Pow(v.x, p.x), Mathf.Pow(v.y, p.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2 v, float a) => new(Mathf.Min(v.x, a), Mathf.Min(v.y, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2 v, float a) => new(Mathf.Max(v.x, a), Mathf.Max(v.y, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2 v, float a, float b) => new(Mathf.Min(v.x, a, b), Mathf.Min(v.y, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2 v, float a, float b) => new(Mathf.Max(v.x, a, b), Mathf.Max(v.y, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2 v, float a, float b, float c) => new(Mathf.Min(v.x, a, b, c), Mathf.Min(v.y, a, b, c));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2 v, float a, float b, float c) => new(Mathf.Max(v.x, a, b, c), Mathf.Max(v.y, a, b, c));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2 v, V2 a) => new(Mathf.Min(v.x, a.x), Mathf.Min(v.y, a.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2 v, V2 a) => new(Mathf.Max(v.x, a.x), Mathf.Max(v.y, a.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2 v, V2 a, V2 b) => new(Mathf.Min(v.x, a.x, b.x), Mathf.Min(v.y, a.y, b.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2 v, V2 a, V2 b) => new(Mathf.Max(v.x, a.x, b.x), Mathf.Max(v.y, a.y, b.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2 v, V2 a, V2 b, V2 c) => new(Mathf.Min(v.x, a.x, b.x, c.x), Mathf.Min(v.y, a.y, b.y, c.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2 v, V2 a, V2 b, V2 c) => new(Mathf.Max(v.x, a.x, b.x, c.x), Mathf.Max(v.y, a.y, b.y, c.y));



		// Vector on vector action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Add(this V2 v, V2 b) => v + b; // new (v.x + b.x, v.y + b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sub(this V2 v, V2 b) => v - b; // new (v.x - b.x, v.y - b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Div(this V2 v, V2 b) => v / b; // new (v.x / b.x, v.y / b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Mul(this V2 v, V2 b) => v * b; // new (v.x * b.x, v.y * b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Rem(this V2 v, V2 b) => new(v.x % b.x, v.y % b.y);

		// Vector on float action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Add(this V2 v, float b) => new(v.x + b, v.y + b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sub(this V2 v, float b) => new(v.x - b, v.y - b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Div(this V2 v, float b) => v / b; // new (v.x / b, v.y / b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Mul(this V2 v, float b) => v * b; // new (v.x * b, v.y * b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Rem(this V2 v, float b) => new(v.x % b, v.y % b);


		// Set component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetX(this V2 v, float value) => new(value, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 SetY(this V2 v, float value) => new(v.x, value);

		// Add to component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddX(this V2 v, float value) => new(v.x + value, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 AddY(this V2 v, float value) => new(v.x, v.y + value);


		// Swizzling
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xx(this V2 v) => new(v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xy(this V2 v) => new(v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yx(this V2 v) => new(v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yy(this V2 v) => new(v.y, v.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxx(this V2 v) => new(v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxy(this V2 v) => new(v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyx(this V2 v) => new(v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyy(this V2 v) => new(v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxx(this V2 v) => new(v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxy(this V2 v) => new(v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyx(this V2 v) => new(v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyy(this V2 v) => new(v.y, v.y, v.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxx(this V2 v) => new(v.x, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxy(this V2 v) => new(v.x, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyx(this V2 v) => new(v.x, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyy(this V2 v) => new(v.x, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxx(this V2 v) => new(v.x, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxy(this V2 v) => new(v.x, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyx(this V2 v) => new(v.x, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyy(this V2 v) => new(v.x, v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxx(this V2 v) => new(v.y, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxy(this V2 v) => new(v.y, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyx(this V2 v) => new(v.y, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyy(this V2 v) => new(v.y, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxx(this V2 v) => new(v.y, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxy(this V2 v) => new(v.y, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyx(this V2 v) => new(v.y, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyy(this V2 v) => new(v.y, v.y, v.y, v.y);

		#endregion



		#region Vector3

		// Util
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLongerThan(this V3 v, V3 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Ray RayTo(this V3 v, V3 vector) => new(v, (vector - v).normalized);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 x0z(this V3 v) => new(v.x, 0, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Expand(this V3 v, float w) => new(v.x, v.y, v.z, w);


		// Math
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I FloorInt(this V3 v) => new(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y), Mathf.FloorToInt(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I RoundInt(this V3 v) => new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I CeilInt(this V3 v) => new(Mathf.CeilToInt(v.x), Mathf.CeilToInt(v.y), Mathf.CeilToInt(v.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Floor(this V3 v) => new(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Round(this V3 v) => new(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Ceil(this V3 v) => new(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sin(this V3 v) => new(Mathf.Sin(v.x), Mathf.Sin(v.y), Mathf.Sin(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Cos(this V3 v) => new(Mathf.Cos(v.x), Mathf.Cos(v.y), Mathf.Cos(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Tan(this V3 v) => new(Mathf.Tan(v.x), Mathf.Tan(v.y), Mathf.Tan(v.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Asin(this V3 v) => new(Mathf.Asin(v.x), Mathf.Asin(v.y), Mathf.Asin(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Acos(this V3 v) => new(Mathf.Acos(v.x), Mathf.Acos(v.y), Mathf.Acos(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Atan(this V3 v) => new(Mathf.Atan(v.x), Mathf.Atan(v.y), Mathf.Atan(v.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Abs(this V3 v) => new(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Clamp01(this V3 v) => new(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y), Mathf.Clamp01(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Exp(this V3 v) => new(Mathf.Exp(v.x), Mathf.Exp(v.y), Mathf.Exp(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Log(this V3 v) => new(Mathf.Log(v.x), Mathf.Log(v.y), Mathf.Log(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Log10(this V3 v) => new(Mathf.Log10(v.x), Mathf.Log10(v.y), Mathf.Log10(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sign(this V3 v) => new(Mathf.Sign(v.x), Mathf.Sign(v.y), Mathf.Sign(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I SignInt(this V3 v) => new(Math.Sign(v.x), Math.Sign(v.y), Math.Sign(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sqrt(this V3 v) => new(Mathf.Sqrt(v.x), Mathf.Sqrt(v.y), Mathf.Sqrt(v.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Atan2(this V3 v, float b) => new(Mathf.Atan2(v.x, b), Mathf.Atan2(v.y, b), Mathf.Atan2(v.z, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Atan2(this V3 v, V3 b) => new(Mathf.Atan2(v.x, b.x), Mathf.Atan2(v.y, b.y), Mathf.Atan2(v.z, b.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Clamp(this V3 v, float min, float max) => new(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Clamp(this V3 v, V3 min, V3 max) => new(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.x, max.x), Mathf.Clamp(v.z, min.x, max.x));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Pow(this V3 v, float p) => new(Mathf.Pow(v.x, p), Mathf.Pow(v.y, p), Mathf.Pow(v.z, p));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Pow(this V3 v, V3 p) => new(Mathf.Pow(v.x, p.x), Mathf.Pow(v.y, p.y), Mathf.Pow(v.z, p.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3 v, float a) => new(Mathf.Min(v.x, a), Mathf.Min(v.y, a), Mathf.Min(v.z, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3 v, float a) => new(Mathf.Max(v.x, a), Mathf.Max(v.y, a), Mathf.Max(v.z, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3 v, float a, float b) => new(Mathf.Min(v.x, a, b), Mathf.Min(v.y, a, b), Mathf.Min(v.z, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3 v, float a, float b) => new(Mathf.Max(v.x, a, b), Mathf.Max(v.y, a, b), Mathf.Max(v.z, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3 v, float a, float b, float c) => new(Mathf.Min(v.x, a, b, c), Mathf.Min(v.y, a, b, c), Mathf.Min(v.z, a, b, c));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3 v, float a, float b, float c) => new(Mathf.Max(v.x, a, b, c), Mathf.Max(v.y, a, b, c), Mathf.Max(v.z, a, b, c));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3 v, V3 a) => new(Mathf.Min(v.x, a.x), Mathf.Min(v.y, a.y), Mathf.Min(v.z, a.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3 v, V3 a) => new(Mathf.Max(v.x, a.x), Mathf.Max(v.y, a.y), Mathf.Max(v.z, a.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3 v, V3 a, V3 b) => new(Mathf.Min(v.x, a.x, b.x), Mathf.Min(v.y, a.y, b.y), Mathf.Min(v.z, a.z, b.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3 v, V3 a, V3 b) => new(Mathf.Max(v.x, a.x, b.x), Mathf.Max(v.y, a.y, b.y), Mathf.Max(v.z, a.z, b.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3 v, V3 a, V3 b, V3 c) => new(Mathf.Min(v.x, a.x, b.x, c.x), Mathf.Min(v.y, a.y, b.y, c.y), Mathf.Min(v.z, a.z, b.z, c.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3 v, V3 a, V3 b, V3 c) => new(Mathf.Max(v.x, a.x, b.x, c.x), Mathf.Max(v.y, a.y, b.y, c.y), Mathf.Max(v.z, a.z, b.z, c.z));


		// Vector on vector action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V3 v, V3 b) => v + b; // new (v.x + b.x, v.y + b.y, v.z + b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sub(this V3 v, V3 b) => v - b; // new (v.x - b.x, v.y - b.y, v.z - b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Div(this V3 v, V3 b) => new(v.x / b.x, v.y / b.y, v.z / b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Mul(this V3 v, V3 b) => new(v.x * b.x, v.y * b.y, v.z * b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Rem(this V3 v, V3 b) => new(v.x % b.x, v.y % b.y, v.z % b.z);

		// Vector on float action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V3 v, float b) => new(v.x + b, v.y + b, v.z + b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sub(this V3 v, float b) => new(v.x - b, v.y - b, v.z - b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Div(this V3 v, float b) => v / b; // new (v.x / b, v.y / b, v.z / b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Mul(this V3 v, float b) => v * b; // new (v.x * b, v.y * b, v.z * b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Rem(this V3 v, float b) => new(v.x % b, v.y % b, v.z % b);


		// Set component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetX(this V3 v, float value) => new(value, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetY(this V3 v, float value) => new(v.x, value, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZ(this V3 v, float value) => new(v.x, v.y, value);

		// Add to component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddX(this V3 v, float value) => new(v.x + value, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddY(this V3 v, float value) => new(v.x, v.y + value, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 AddZ(this V3 v, float value) => new(v.x, v.y, v.z + value);


		// Swizzling
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xx(this V3 v) => new(v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xy(this V3 v) => new(v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xz(this V3 v) => new(v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yx(this V3 v) => new(v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yy(this V3 v) => new(v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yz(this V3 v) => new(v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zx(this V3 v) => new(v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zy(this V3 v) => new(v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zz(this V3 v) => new(v.z, v.z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxx(this V3 v) => new(v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxy(this V3 v) => new(v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxz(this V3 v) => new(v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyx(this V3 v) => new(v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyy(this V3 v) => new(v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyz(this V3 v) => new(v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzx(this V3 v) => new(v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzy(this V3 v) => new(v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzz(this V3 v) => new(v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxx(this V3 v) => new(v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxy(this V3 v) => new(v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxz(this V3 v) => new(v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyx(this V3 v) => new(v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyy(this V3 v) => new(v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyz(this V3 v) => new(v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzx(this V3 v) => new(v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzy(this V3 v) => new(v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzz(this V3 v) => new(v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxx(this V3 v) => new(v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxy(this V3 v) => new(v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxz(this V3 v) => new(v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyx(this V3 v) => new(v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyy(this V3 v) => new(v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyz(this V3 v) => new(v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzx(this V3 v) => new(v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzy(this V3 v) => new(v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzz(this V3 v) => new(v.z, v.z, v.z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxx(this V3 v) => new(v.x, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxy(this V3 v) => new(v.x, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxz(this V3 v) => new(v.x, v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyx(this V3 v) => new(v.x, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyy(this V3 v) => new(v.x, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyz(this V3 v) => new(v.x, v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzx(this V3 v) => new(v.x, v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzy(this V3 v) => new(v.x, v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzz(this V3 v) => new(v.x, v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxx(this V3 v) => new(v.x, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxy(this V3 v) => new(v.x, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxz(this V3 v) => new(v.x, v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyx(this V3 v) => new(v.x, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyy(this V3 v) => new(v.x, v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyz(this V3 v) => new(v.x, v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzx(this V3 v) => new(v.x, v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzy(this V3 v) => new(v.x, v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzz(this V3 v) => new(v.x, v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxx(this V3 v) => new(v.x, v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxy(this V3 v) => new(v.x, v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxz(this V3 v) => new(v.x, v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyx(this V3 v) => new(v.x, v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyy(this V3 v) => new(v.x, v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyz(this V3 v) => new(v.x, v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzx(this V3 v) => new(v.x, v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzy(this V3 v) => new(v.x, v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzz(this V3 v) => new(v.x, v.z, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxx(this V3 v) => new(v.y, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxy(this V3 v) => new(v.y, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxz(this V3 v) => new(v.y, v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyx(this V3 v) => new(v.y, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyy(this V3 v) => new(v.y, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyz(this V3 v) => new(v.y, v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzx(this V3 v) => new(v.y, v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzy(this V3 v) => new(v.y, v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzz(this V3 v) => new(v.y, v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxx(this V3 v) => new(v.y, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxy(this V3 v) => new(v.y, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxz(this V3 v) => new(v.y, v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyx(this V3 v) => new(v.y, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyy(this V3 v) => new(v.y, v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyz(this V3 v) => new(v.y, v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzx(this V3 v) => new(v.y, v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzy(this V3 v) => new(v.y, v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzz(this V3 v) => new(v.y, v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxx(this V3 v) => new(v.y, v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxy(this V3 v) => new(v.y, v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxz(this V3 v) => new(v.y, v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyx(this V3 v) => new(v.y, v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyy(this V3 v) => new(v.y, v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyz(this V3 v) => new(v.y, v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzx(this V3 v) => new(v.y, v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzy(this V3 v) => new(v.y, v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzz(this V3 v) => new(v.y, v.z, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxx(this V3 v) => new(v.z, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxy(this V3 v) => new(v.z, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxz(this V3 v) => new(v.z, v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyx(this V3 v) => new(v.z, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyy(this V3 v) => new(v.z, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyz(this V3 v) => new(v.z, v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzx(this V3 v) => new(v.z, v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzy(this V3 v) => new(v.z, v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzz(this V3 v) => new(v.z, v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxx(this V3 v) => new(v.z, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxy(this V3 v) => new(v.z, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxz(this V3 v) => new(v.z, v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyx(this V3 v) => new(v.z, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyy(this V3 v) => new(v.z, v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyz(this V3 v) => new(v.z, v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzx(this V3 v) => new(v.z, v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzy(this V3 v) => new(v.z, v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzz(this V3 v) => new(v.z, v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxx(this V3 v) => new(v.z, v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxy(this V3 v) => new(v.z, v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxz(this V3 v) => new(v.z, v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyx(this V3 v) => new(v.z, v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyy(this V3 v) => new(v.z, v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyz(this V3 v) => new(v.z, v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzx(this V3 v) => new(v.z, v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzy(this V3 v) => new(v.z, v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzz(this V3 v) => new(v.z, v.z, v.z, v.z);

		#endregion



		#region Vector4

		// Util
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLongerThan(this V4 v, V4 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;


		// Math
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Floor(this V4 v) => new(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z), Mathf.Floor(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Round(this V4 v) => new(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z), Mathf.Round(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Ceil(this V4 v) => new(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z), Mathf.Ceil(v.w));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Sin(this V4 v) => new(Mathf.Sin(v.x), Mathf.Sin(v.y), Mathf.Sin(v.z), Mathf.Sin(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Cos(this V4 v) => new(Mathf.Cos(v.x), Mathf.Cos(v.y), Mathf.Cos(v.z), Mathf.Cos(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Tan(this V4 v) => new(Mathf.Tan(v.x), Mathf.Tan(v.y), Mathf.Tan(v.z), Mathf.Tan(v.w));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Asin(this V4 v) => new(Mathf.Asin(v.x), Mathf.Asin(v.y), Mathf.Asin(v.z), Mathf.Asin(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Acos(this V4 v) => new(Mathf.Acos(v.x), Mathf.Acos(v.y), Mathf.Acos(v.z), Mathf.Acos(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Atan(this V4 v) => new(Mathf.Atan(v.x), Mathf.Atan(v.y), Mathf.Atan(v.z), Mathf.Atan(v.w));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Abs(this V4 v) => new(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z), Mathf.Abs(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Clamp01(this V4 v) => new(Mathf.Clamp01(v.x), Mathf.Clamp01(v.y), Mathf.Clamp01(v.z), Mathf.Clamp01(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Exp(this V4 v) => new(Mathf.Exp(v.x), Mathf.Exp(v.y), Mathf.Exp(v.z), Mathf.Exp(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Log(this V4 v) => new(Mathf.Log(v.x), Mathf.Log(v.y), Mathf.Log(v.z), Mathf.Log(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Log10(this V4 v) => new(Mathf.Log10(v.x), Mathf.Log10(v.y), Mathf.Log10(v.z), Mathf.Log10(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Sign(this V4 v) => new(Mathf.Sign(v.x), Mathf.Sign(v.y), Mathf.Sign(v.z), Mathf.Sign(v.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Sqrt(this V4 v) => new(Mathf.Sqrt(v.x), Mathf.Sqrt(v.y), Mathf.Sqrt(v.z), Mathf.Sqrt(v.w));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Atan2(this V4 v, float b) => new(Mathf.Atan2(v.x, b), Mathf.Atan2(v.y, b), Mathf.Atan2(v.z, b), Mathf.Atan2(v.w, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Atan2(this V4 v, V4 b) => new(Mathf.Atan2(v.x, b.x), Mathf.Atan2(v.y, b.y), Mathf.Atan2(v.z, b.z), Mathf.Atan2(v.w, b.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Clamp(this V4 v, float min, float max) => new(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max), Mathf.Clamp(v.w, min, max));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Clamp(this V4 v, V4 min, V4 max) => new(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y), Mathf.Clamp(v.z, min.z, max.z), Mathf.Clamp(v.w, min.w, max.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Pow(this V4 v, float p) => new(Mathf.Pow(v.x, p), Mathf.Pow(v.y, p), Mathf.Pow(v.z, p), Mathf.Pow(v.w, p));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Pow(this V4 v, V4 p) => new(Mathf.Pow(v.x, p.x), Mathf.Pow(v.y, p.y), Mathf.Pow(v.z, p.z), Mathf.Pow(v.w, p.w));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Min(this V4 v, float a) => new(Mathf.Min(v.x, a), Mathf.Min(v.y, a), Mathf.Min(v.z, a), Mathf.Min(v.w, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Max(this V4 v, float a) => new(Mathf.Max(v.x, a), Mathf.Max(v.y, a), Mathf.Max(v.z, a), Mathf.Max(v.w, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Min(this V4 v, float a, float b) => new(Mathf.Min(v.x, a, b), Mathf.Min(v.y, a, b), Mathf.Min(v.z, a, b), Mathf.Min(v.w, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Max(this V4 v, float a, float b) => new(Mathf.Max(v.x, a, b), Mathf.Max(v.y, a, b), Mathf.Max(v.z, a, b), Mathf.Max(v.w, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Min(this V4 v, float a, float b, float c) => new(Mathf.Min(v.x, a, b, c), Mathf.Min(v.y, a, b, c), Mathf.Min(v.z, a, b, c), Mathf.Min(v.w, a, b, c));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Max(this V4 v, float a, float b, float c) => new(Mathf.Max(v.x, a, b, c), Mathf.Max(v.y, a, b, c), Mathf.Max(v.z, a, b, c), Mathf.Max(v.w, a, b, c));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Min(this V4 v, V4 a) => new(Mathf.Min(v.x, a.x), Mathf.Min(v.y, a.y), Mathf.Min(v.z, a.z), Mathf.Min(v.w, a.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Max(this V4 v, V4 a) => new(Mathf.Max(v.x, a.x), Mathf.Max(v.y, a.y), Mathf.Max(v.z, a.z), Mathf.Max(v.w, a.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Min(this V4 v, V4 a, V4 b) => new(Mathf.Min(v.x, a.x, b.x), Mathf.Min(v.y, a.y, b.y), Mathf.Min(v.z, a.z, b.z), Mathf.Min(v.w, a.w, b.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Max(this V4 v, V4 a, V4 b) => new(Mathf.Max(v.x, a.x, b.x), Mathf.Max(v.y, a.y, b.y), Mathf.Max(v.z, a.z, b.z), Mathf.Max(v.w, a.w, b.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Min(this V4 v, V4 a, V4 b, V4 c) => new(Mathf.Min(v.x, a.x, b.x, c.x), Mathf.Min(v.y, a.y, b.y, c.y), Mathf.Min(v.z, a.z, b.z, c.z), Mathf.Min(v.w, a.w, b.w, c.w));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Max(this V4 v, V4 a, V4 b, V4 c) => new(Mathf.Max(v.x, a.x, b.x, c.x), Mathf.Max(v.y, a.y, b.y, c.y), Mathf.Max(v.z, a.z, b.z, c.z), Mathf.Max(v.w, a.w, b.w, c.w));


		// Vector on vector action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Add(this V4 v, V4 b) => v + b; // new (v.x + b.x, v.y + b.y, v.z + b.z, v.w + b.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Sub(this V4 v, V4 b) => v - b; // new (v.x - b.x, v.y - b.y, v.z - b.z, v.w - b.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Div(this V4 v, V4 b) => new(v.x / b.x, v.y / b.y, v.z / b.z, v.w / b.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Mul(this V4 v, V4 b) => new(v.x * b.x, v.y * b.y, v.z * b.z, v.w * b.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Rem(this V4 v, V4 b) => new(v.x % b.x, v.y % b.y, v.z % b.z, v.w % b.w);

		// Vector on float action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Add(this V4 v, float b) => new(v.x + b, v.y + b, v.z + b, v.w + b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Sub(this V4 v, float b) => new(v.x - b, v.y - b, v.z - b, v.w - b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Div(this V4 v, float b) => v / b; // new (v.x / b, v.y / b, v.z / b, v.w / b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Mul(this V4 v, float b) => v * b; // new (v.x * b, v.y * b, v.z * b, v.w * b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Rem(this V4 v, float b) => new(v.x % b, v.y % b, v.z % b, v.w % b);

		// Vector on int action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Add(this V4 v, int b) => new(v.x + b, v.y + b, v.z + b, v.w + b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Sub(this V4 v, int b) => new(v.x - b, v.y - b, v.z - b, v.w - b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Div(this V4 v, int b) => v / b; // new (v.x / b, v.y / b, v.z / b, v.w / b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Mul(this V4 v, int b) => v * b; // new (v.x * b, v.y * b, v.z * b, v.w * b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 Rem(this V4 v, int b) => new(v.x % b, v.y % b, v.z % b, v.w % b);


		// Set component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetX(this V4 v, float value) => new(value, v.y, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetY(this V4 v, float value) => new(v.x, value, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetZ(this V4 v, float value) => new(v.x, v.y, value, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 SetW(this V4 v, float value) => new(v.x, v.y, v.z, value);

		// Add to component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 AddX(this V4 v, float value) => new(v.x + value, v.y, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 AddY(this V4 v, float value) => new(v.x, v.y + value, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 AddZ(this V4 v, float value) => new(v.x, v.y, v.z + value, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 AddW(this V4 v, float value) => new(v.x, v.y, v.z, v.w + value);


		// Swizzling
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xx(this V4 v) => new(v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xy(this V4 v) => new(v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xz(this V4 v) => new(v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 xw(this V4 v) => new(v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yx(this V4 v) => new(v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yy(this V4 v) => new(v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yz(this V4 v) => new(v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 yw(this V4 v) => new(v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zx(this V4 v) => new(v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zy(this V4 v) => new(v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zz(this V4 v) => new(v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 zw(this V4 v) => new(v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 wx(this V4 v) => new(v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 wy(this V4 v) => new(v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 wz(this V4 v) => new(v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 ww(this V4 v) => new(v.w, v.w);


		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxx(this V4 v) => new(v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxy(this V4 v) => new(v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxz(this V4 v) => new(v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xxw(this V4 v) => new(v.x, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyx(this V4 v) => new(v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyy(this V4 v) => new(v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyz(this V4 v) => new(v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xyw(this V4 v) => new(v.x, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzx(this V4 v) => new(v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzy(this V4 v) => new(v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzz(this V4 v) => new(v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xzw(this V4 v) => new(v.x, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xwx(this V4 v) => new(v.x, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xwy(this V4 v) => new(v.x, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xwz(this V4 v) => new(v.x, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 xww(this V4 v) => new(v.x, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxx(this V4 v) => new(v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxy(this V4 v) => new(v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxz(this V4 v) => new(v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yxw(this V4 v) => new(v.y, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyx(this V4 v) => new(v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyy(this V4 v) => new(v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyz(this V4 v) => new(v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yyw(this V4 v) => new(v.y, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzx(this V4 v) => new(v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzy(this V4 v) => new(v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzz(this V4 v) => new(v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yzw(this V4 v) => new(v.y, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ywx(this V4 v) => new(v.y, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ywy(this V4 v) => new(v.y, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 ywz(this V4 v) => new(v.y, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 yww(this V4 v) => new(v.y, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxx(this V4 v) => new(v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxy(this V4 v) => new(v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxz(this V4 v) => new(v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zxw(this V4 v) => new(v.z, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyx(this V4 v) => new(v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyy(this V4 v) => new(v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyz(this V4 v) => new(v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zyw(this V4 v) => new(v.z, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzx(this V4 v) => new(v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzy(this V4 v) => new(v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzz(this V4 v) => new(v.z, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zzw(this V4 v) => new(v.z, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zwx(this V4 v) => new(v.z, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zwy(this V4 v) => new(v.z, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zwz(this V4 v) => new(v.z, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 zww(this V4 v) => new(v.z, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wxx(this V4 v) => new(v.w, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wxy(this V4 v) => new(v.w, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wxz(this V4 v) => new(v.w, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wxw(this V4 v) => new(v.w, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wyx(this V4 v) => new(v.w, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wyy(this V4 v) => new(v.w, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wyz(this V4 v) => new(v.w, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wyw(this V4 v) => new(v.w, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wzx(this V4 v) => new(v.w, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wzy(this V4 v) => new(v.w, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wzz(this V4 v) => new(v.w, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wzw(this V4 v) => new(v.w, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wwx(this V4 v) => new(v.w, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wwy(this V4 v) => new(v.w, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 wwz(this V4 v) => new(v.w, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 www(this V4 v) => new(v.w, v.w, v.w);


		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxx(this V4 v) => new(v.x, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxy(this V4 v) => new(v.x, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxz(this V4 v) => new(v.x, v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxxw(this V4 v) => new(v.x, v.x, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyx(this V4 v) => new(v.x, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyy(this V4 v) => new(v.x, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyz(this V4 v) => new(v.x, v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxyw(this V4 v) => new(v.x, v.x, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzx(this V4 v) => new(v.x, v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzy(this V4 v) => new(v.x, v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzz(this V4 v) => new(v.x, v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxzw(this V4 v) => new(v.x, v.x, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxwx(this V4 v) => new(v.x, v.x, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxwy(this V4 v) => new(v.x, v.x, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxwz(this V4 v) => new(v.x, v.x, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xxww(this V4 v) => new(v.x, v.x, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxx(this V4 v) => new(v.x, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxy(this V4 v) => new(v.x, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxz(this V4 v) => new(v.x, v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyxw(this V4 v) => new(v.x, v.y, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyx(this V4 v) => new(v.x, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyy(this V4 v) => new(v.x, v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyz(this V4 v) => new(v.x, v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyyw(this V4 v) => new(v.x, v.y, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzx(this V4 v) => new(v.x, v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzy(this V4 v) => new(v.x, v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzz(this V4 v) => new(v.x, v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyzw(this V4 v) => new(v.x, v.y, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xywx(this V4 v) => new(v.x, v.y, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xywy(this V4 v) => new(v.x, v.y, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xywz(this V4 v) => new(v.x, v.y, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xyww(this V4 v) => new(v.x, v.y, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxx(this V4 v) => new(v.x, v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxy(this V4 v) => new(v.x, v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxz(this V4 v) => new(v.x, v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzxw(this V4 v) => new(v.x, v.z, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyx(this V4 v) => new(v.x, v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyy(this V4 v) => new(v.x, v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyz(this V4 v) => new(v.x, v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzyw(this V4 v) => new(v.x, v.z, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzx(this V4 v) => new(v.x, v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzy(this V4 v) => new(v.x, v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzz(this V4 v) => new(v.x, v.z, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzzw(this V4 v) => new(v.x, v.z, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzwx(this V4 v) => new(v.x, v.z, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzwy(this V4 v) => new(v.x, v.z, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzwz(this V4 v) => new(v.x, v.z, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xzww(this V4 v) => new(v.x, v.z, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwxx(this V4 v) => new(v.x, v.w, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwxy(this V4 v) => new(v.x, v.w, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwxz(this V4 v) => new(v.x, v.w, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwxw(this V4 v) => new(v.x, v.w, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwyx(this V4 v) => new(v.x, v.w, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwyy(this V4 v) => new(v.x, v.w, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwyz(this V4 v) => new(v.x, v.w, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwyw(this V4 v) => new(v.x, v.w, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwzx(this V4 v) => new(v.x, v.w, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwzy(this V4 v) => new(v.x, v.w, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwzz(this V4 v) => new(v.x, v.w, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwzw(this V4 v) => new(v.x, v.w, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwwx(this V4 v) => new(v.x, v.w, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwwy(this V4 v) => new(v.x, v.w, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwwz(this V4 v) => new(v.x, v.w, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 xwww(this V4 v) => new(v.x, v.w, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxx(this V4 v) => new(v.y, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxy(this V4 v) => new(v.y, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxz(this V4 v) => new(v.y, v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxxw(this V4 v) => new(v.y, v.x, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyx(this V4 v) => new(v.y, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyy(this V4 v) => new(v.y, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyz(this V4 v) => new(v.y, v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxyw(this V4 v) => new(v.y, v.x, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzx(this V4 v) => new(v.y, v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzy(this V4 v) => new(v.y, v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzz(this V4 v) => new(v.y, v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxzw(this V4 v) => new(v.y, v.x, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxwx(this V4 v) => new(v.y, v.x, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxwy(this V4 v) => new(v.y, v.x, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxwz(this V4 v) => new(v.y, v.x, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yxww(this V4 v) => new(v.y, v.x, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxx(this V4 v) => new(v.y, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxy(this V4 v) => new(v.y, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxz(this V4 v) => new(v.y, v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyxw(this V4 v) => new(v.y, v.y, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyx(this V4 v) => new(v.y, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyy(this V4 v) => new(v.y, v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyz(this V4 v) => new(v.y, v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyyw(this V4 v) => new(v.y, v.y, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzx(this V4 v) => new(v.y, v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzy(this V4 v) => new(v.y, v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzz(this V4 v) => new(v.y, v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyzw(this V4 v) => new(v.y, v.y, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yywx(this V4 v) => new(v.y, v.y, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yywy(this V4 v) => new(v.y, v.y, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yywz(this V4 v) => new(v.y, v.y, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yyww(this V4 v) => new(v.y, v.y, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxx(this V4 v) => new(v.y, v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxy(this V4 v) => new(v.y, v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxz(this V4 v) => new(v.y, v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzxw(this V4 v) => new(v.y, v.z, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyx(this V4 v) => new(v.y, v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyy(this V4 v) => new(v.y, v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyz(this V4 v) => new(v.y, v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzyw(this V4 v) => new(v.y, v.z, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzx(this V4 v) => new(v.y, v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzy(this V4 v) => new(v.y, v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzz(this V4 v) => new(v.y, v.z, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzzw(this V4 v) => new(v.y, v.z, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzwx(this V4 v) => new(v.y, v.z, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzwy(this V4 v) => new(v.y, v.z, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzwz(this V4 v) => new(v.y, v.z, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 yzww(this V4 v) => new(v.y, v.z, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywxx(this V4 v) => new(v.y, v.w, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywxy(this V4 v) => new(v.y, v.w, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywxz(this V4 v) => new(v.y, v.w, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywxw(this V4 v) => new(v.y, v.w, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywyx(this V4 v) => new(v.y, v.w, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywyy(this V4 v) => new(v.y, v.w, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywyz(this V4 v) => new(v.y, v.w, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywyw(this V4 v) => new(v.y, v.w, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywzx(this V4 v) => new(v.y, v.w, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywzy(this V4 v) => new(v.y, v.w, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywzz(this V4 v) => new(v.y, v.w, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywzw(this V4 v) => new(v.y, v.w, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywwx(this V4 v) => new(v.y, v.w, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywwy(this V4 v) => new(v.y, v.w, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywwz(this V4 v) => new(v.y, v.w, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 ywww(this V4 v) => new(v.y, v.w, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxx(this V4 v) => new(v.z, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxy(this V4 v) => new(v.z, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxz(this V4 v) => new(v.z, v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxxw(this V4 v) => new(v.z, v.x, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyx(this V4 v) => new(v.z, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyy(this V4 v) => new(v.z, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyz(this V4 v) => new(v.z, v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxyw(this V4 v) => new(v.z, v.x, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzx(this V4 v) => new(v.z, v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzy(this V4 v) => new(v.z, v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzz(this V4 v) => new(v.z, v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxzw(this V4 v) => new(v.z, v.x, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxwx(this V4 v) => new(v.z, v.x, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxwy(this V4 v) => new(v.z, v.x, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxwz(this V4 v) => new(v.z, v.x, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zxww(this V4 v) => new(v.z, v.x, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxx(this V4 v) => new(v.z, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxy(this V4 v) => new(v.z, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxz(this V4 v) => new(v.z, v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyxw(this V4 v) => new(v.z, v.y, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyx(this V4 v) => new(v.z, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyy(this V4 v) => new(v.z, v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyz(this V4 v) => new(v.z, v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyyw(this V4 v) => new(v.z, v.y, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzx(this V4 v) => new(v.z, v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzy(this V4 v) => new(v.z, v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzz(this V4 v) => new(v.z, v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyzw(this V4 v) => new(v.z, v.y, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zywx(this V4 v) => new(v.z, v.y, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zywy(this V4 v) => new(v.z, v.y, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zywz(this V4 v) => new(v.z, v.y, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zyww(this V4 v) => new(v.z, v.y, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxx(this V4 v) => new(v.z, v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxy(this V4 v) => new(v.z, v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxz(this V4 v) => new(v.z, v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzxw(this V4 v) => new(v.z, v.z, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyx(this V4 v) => new(v.z, v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyy(this V4 v) => new(v.z, v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyz(this V4 v) => new(v.z, v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzyw(this V4 v) => new(v.z, v.z, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzx(this V4 v) => new(v.z, v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzy(this V4 v) => new(v.z, v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzz(this V4 v) => new(v.z, v.z, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzzw(this V4 v) => new(v.z, v.z, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzwx(this V4 v) => new(v.z, v.z, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzwy(this V4 v) => new(v.z, v.z, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzwz(this V4 v) => new(v.z, v.z, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zzww(this V4 v) => new(v.z, v.z, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwxx(this V4 v) => new(v.z, v.w, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwxy(this V4 v) => new(v.z, v.w, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwxz(this V4 v) => new(v.z, v.w, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwxw(this V4 v) => new(v.z, v.w, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwyx(this V4 v) => new(v.z, v.w, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwyy(this V4 v) => new(v.z, v.w, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwyz(this V4 v) => new(v.z, v.w, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwyw(this V4 v) => new(v.z, v.w, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwzx(this V4 v) => new(v.z, v.w, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwzy(this V4 v) => new(v.z, v.w, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwzz(this V4 v) => new(v.z, v.w, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwzw(this V4 v) => new(v.z, v.w, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwwx(this V4 v) => new(v.z, v.w, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwwy(this V4 v) => new(v.z, v.w, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwwz(this V4 v) => new(v.z, v.w, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 zwww(this V4 v) => new(v.z, v.w, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxxx(this V4 v) => new(v.w, v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxxy(this V4 v) => new(v.w, v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxxz(this V4 v) => new(v.w, v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxxw(this V4 v) => new(v.w, v.x, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxyx(this V4 v) => new(v.w, v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxyy(this V4 v) => new(v.w, v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxyz(this V4 v) => new(v.w, v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxyw(this V4 v) => new(v.w, v.x, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxzx(this V4 v) => new(v.w, v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxzy(this V4 v) => new(v.w, v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxzz(this V4 v) => new(v.w, v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxzw(this V4 v) => new(v.w, v.x, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxwx(this V4 v) => new(v.w, v.x, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxwy(this V4 v) => new(v.w, v.x, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxwz(this V4 v) => new(v.w, v.x, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wxww(this V4 v) => new(v.w, v.x, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyxx(this V4 v) => new(v.w, v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyxy(this V4 v) => new(v.w, v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyxz(this V4 v) => new(v.w, v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyxw(this V4 v) => new(v.w, v.y, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyyx(this V4 v) => new(v.w, v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyyy(this V4 v) => new(v.w, v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyyz(this V4 v) => new(v.w, v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyyw(this V4 v) => new(v.w, v.y, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyzx(this V4 v) => new(v.w, v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyzy(this V4 v) => new(v.w, v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyzz(this V4 v) => new(v.w, v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyzw(this V4 v) => new(v.w, v.y, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wywx(this V4 v) => new(v.w, v.y, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wywy(this V4 v) => new(v.w, v.y, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wywz(this V4 v) => new(v.w, v.y, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wyww(this V4 v) => new(v.w, v.y, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzxx(this V4 v) => new(v.w, v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzxy(this V4 v) => new(v.w, v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzxz(this V4 v) => new(v.w, v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzxw(this V4 v) => new(v.w, v.z, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzyx(this V4 v) => new(v.w, v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzyy(this V4 v) => new(v.w, v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzyz(this V4 v) => new(v.w, v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzyw(this V4 v) => new(v.w, v.z, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzzx(this V4 v) => new(v.w, v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzzy(this V4 v) => new(v.w, v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzzz(this V4 v) => new(v.w, v.z, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzzw(this V4 v) => new(v.w, v.z, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzwx(this V4 v) => new(v.w, v.z, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzwy(this V4 v) => new(v.w, v.z, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzwz(this V4 v) => new(v.w, v.z, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wzww(this V4 v) => new(v.w, v.z, v.w, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwxx(this V4 v) => new(v.w, v.w, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwxy(this V4 v) => new(v.w, v.w, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwxz(this V4 v) => new(v.w, v.w, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwxw(this V4 v) => new(v.w, v.w, v.x, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwyx(this V4 v) => new(v.w, v.w, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwyy(this V4 v) => new(v.w, v.w, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwyz(this V4 v) => new(v.w, v.w, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwyw(this V4 v) => new(v.w, v.w, v.y, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwzx(this V4 v) => new(v.w, v.w, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwzy(this V4 v) => new(v.w, v.w, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwzz(this V4 v) => new(v.w, v.w, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwzw(this V4 v) => new(v.w, v.w, v.z, v.w);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwwx(this V4 v) => new(v.w, v.w, v.w, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwwy(this V4 v) => new(v.w, v.w, v.w, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwwz(this V4 v) => new(v.w, v.w, v.w, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V4 wwww(this V4 v) => new(v.w, v.w, v.w, v.w);

		#endregion
		#endregion




		#region int
		#region Vector2Int

		// Util
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLongerThan(this V2I v, V2 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Ray2D RayTo(this V2I v, V2 vector) => new(v, (vector - v).normalized);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Expand(this V2I v, int z) => new(v.x, v.y, z);


		// Math
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sin(this V2I v) => new(Mathf.Sin(v.x), Mathf.Sin(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Cos(this V2I v) => new(Mathf.Cos(v.x), Mathf.Cos(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Tan(this V2I v) => new(Mathf.Tan(v.x), Mathf.Tan(v.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Asin(this V2I v) => new(Mathf.Asin(v.x), Mathf.Asin(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Acos(this V2I v) => new(Mathf.Acos(v.x), Mathf.Acos(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Atan(this V2I v) => new(Mathf.Atan(v.x), Mathf.Atan(v.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Abs(this V2I v) => new(Mathf.Abs(v.x), Mathf.Abs(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Clamp01(this V2I v) => new(v.x <= 0 ? 0 : (v.x >= 1 ? 1 : v.x), v.y <= 0 ? 0 : (v.y >= 1 ? 1 : v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Exp(this V2I v) => new(Mathf.Exp(v.x), Mathf.Exp(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Log(this V2I v) => new(Mathf.Log(v.x), Mathf.Log(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Log10(this V2I v) => new(Mathf.Log10(v.x), Mathf.Log10(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Sign(this V2I v) => new(Math.Sign(v.x), Math.Sign(v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sqrt(this V2I v) => new(Mathf.Sqrt(v.x), Mathf.Sqrt(v.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Atan2(this V2I v, float y) => new(Mathf.Atan2(v.x, y), Mathf.Atan2(v.y, y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Atan2(this V2I v, V2 y) => new(Mathf.Atan2(v.x, v.y), Mathf.Atan2(v.y, v.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Clamp(this V2I v, int min, int max) => new(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Clamp(this V2I v, V2I min, V2I max) => new(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Clamp(this V2I v, V2 min, V2 max) => new(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Pow(this V2I v, float p) => new(Mathf.Pow(v.x, p), Mathf.Pow(v.y, p));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Pow(this V2I v, V2 p) => new(Mathf.Pow(v.x, p.x), Mathf.Pow(v.y, p.y));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Min(this V2I v, int a) => new(Mathf.Min(v.x, a), Mathf.Min(v.y, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Max(this V2I v, int a) => new(Mathf.Max(v.x, a), Mathf.Max(v.y, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Min(this V2I v, int a, int b) => new(Mathf.Min(v.x, a, b), Mathf.Min(v.y, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Max(this V2I v, int a, int b) => new(Mathf.Max(v.x, a, b), Mathf.Max(v.y, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Min(this V2I v, int a, int b, int c) => new(Mathf.Min(v.x, a, b, c), Mathf.Min(v.y, a, b, c));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Max(this V2I v, int a, int b, int c) => new(Mathf.Max(v.x, a, b, c), Mathf.Max(v.y, a, b, c));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2I v, float a) => new(Mathf.Min(v.x, a), Mathf.Min(v.y, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2I v, float a) => new(Mathf.Max(v.x, a), Mathf.Max(v.y, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2I v, float a, float b) => new(Mathf.Min(v.x, a, b), Mathf.Min(v.y, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2I v, float a, float b) => new(Mathf.Max(v.x, a, b), Mathf.Max(v.y, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Min(this V2I v, float a, float b, float c) => new(Mathf.Min(v.x, a, b, c), Mathf.Min(v.y, a, b, c));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Max(this V2I v, float a, float b, float c) => new(Mathf.Max(v.x, a, b, c), Mathf.Max(v.y, a, b, c));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Min(this V2I v, V2I a) => new(Mathf.Min(v.x, a.x), Mathf.Min(v.y, a.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Max(this V2I v, V2I a) => new(Mathf.Max(v.x, a.x), Mathf.Max(v.y, a.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Min(this V2I v, V2I a, V2I b) => new(Mathf.Min(v.x, a.x, b.x), Mathf.Min(v.y, a.y, b.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Max(this V2I v, V2I a, V2I b) => new(Mathf.Max(v.x, a.x, b.x), Mathf.Max(v.y, a.y, b.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Min(this V2I v, V2I a, V2I b, V2I c) => new(Mathf.Min(v.x, a.x, b.x, c.x), Mathf.Min(v.y, a.y, b.y, c.y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Max(this V2I v, V2I a, V2I b, V2I c) => new(Mathf.Max(v.x, a.x, b.x, c.x), Mathf.Max(v.y, a.y, b.y, c.y));


		// Vector on meme vector action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Add(this V2I v, V2I b) => v + b; // new (v.x + b.x, v.y + b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Sub(this V2I v, V2I b) => v - b; // new (v.x - b.x, v.y - b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Div(this V2I v, V2I b) => new(v.x / b.x, v.y / b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Mul(this V2I v, V2I b) => v * b; // new (v.x * b.x, v.y * b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Rem(this V2I v, V2I b) => new(v.x % b.x, v.y % b.y);

		// Vector on real vector action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Add(this V2I v, V2 b) => new(v.x + b.x, v.y + b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sub(this V2I v, V2 b) => new(v.x - b.x, v.y - b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Div(this V2I v, V2 b) => new(v.x / b.x, v.y / b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Mul(this V2I v, V2 b) => new(v.x * b.x, v.y * b.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Rem(this V2I v, V2 b) => new(v.x % b.x, v.y % b.y);

		// Vector on int action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Add(this V2I v, int b) => new(v.x + b, v.y + b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Sub(this V2I v, int b) => new(v.x - b, v.y - b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Div(this V2I v, int b) => v / b; // new (v.x / b, v.y / b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Mul(this V2I v, int b) => v * b; // new (v.x * b, v.y * b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I Rem(this V2I v, int b) => new(v.x % b, v.y % b);

		// Vector on float action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Add(this V2I v, float b) => new(v.x + b, v.y + b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Sub(this V2I v, float b) => new(v.x - b, v.y - b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Div(this V2I v, float b) => new(v.x / b, v.y / b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Mul(this V2I v, float b) => new(v.x * b, v.y * b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2 Rem(this V2I v, float b) => new(v.x % b, v.y % b);


		// Set component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I SetX(this V2I v, int value) => new(value, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I SetY(this V2I v, int value) => new(v.x, value);

		// Add to component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I AddX(this V2I v, int value) => new(v.x + value, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I AddY(this V2I v, int value) => new(v.x, v.y + value);


		// Swizzling
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I xx(this V2I v) => new(v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I xy(this V2I v) => new(v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I yx(this V2I v) => new(v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I yy(this V2I v) => new(v.y, v.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xxx(this V2I v) => new(v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xxy(this V2I v) => new(v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xyx(this V2I v) => new(v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xyy(this V2I v) => new(v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yxx(this V2I v) => new(v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yxy(this V2I v) => new(v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yyx(this V2I v) => new(v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yyy(this V2I v) => new(v.y, v.y, v.y);

		#endregion



		#region Vector3Int

		// Util
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool IsLongerThan(this V3I v, V3 smaller) => v.sqrMagnitude > smaller.sqrMagnitude;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static Ray RayTo(this V3I v, V3 vector) => new(v, (vector - v).normalized);


		// Math
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sin(this V3I v) => new(Mathf.Sin(v.x), Mathf.Sin(v.y), Mathf.Sin(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Cos(this V3I v) => new(Mathf.Cos(v.x), Mathf.Cos(v.y), Mathf.Cos(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Tan(this V3I v) => new(Mathf.Tan(v.x), Mathf.Tan(v.y), Mathf.Tan(v.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Asin(this V3I v) => new(Mathf.Asin(v.x), Mathf.Asin(v.y), Mathf.Asin(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Acos(this V3I v) => new(Mathf.Acos(v.x), Mathf.Acos(v.y), Mathf.Acos(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Atan(this V3I v) => new(Mathf.Atan(v.x), Mathf.Atan(v.y), Mathf.Atan(v.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Abs(this V3I v) => new(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Clamp01(this V3I v) => new(v.x <= 0 ? 0 : (v.x >= 1 ? 1 : v.x), v.y <= 0 ? 0 : (v.y >= 1 ? 1 : v.y), v.z <= 0 ? 0 : (v.z >= 1 ? 1 : v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Exp(this V3I v) => new(Mathf.Exp(v.x), Mathf.Exp(v.y), Mathf.Exp(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Log(this V3I v) => new(Mathf.Log(v.x), Mathf.Log(v.y), Mathf.Log(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Log10(this V3I v) => new(Mathf.Log10(v.x), Mathf.Log10(v.y), Mathf.Log10(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Sign(this V3I v) => new(Math.Sign(v.x), Math.Sign(v.y), Math.Sign(v.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sqrt(this V3I v) => new(Mathf.Sqrt(v.x), Mathf.Sqrt(v.y), Mathf.Sqrt(v.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Atan2(this V3I v, float a) => new(Mathf.Atan2(v.x, a), Mathf.Atan2(v.y, a), Mathf.Atan2(v.z, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Atan2(this V3I v, V3 a) => new(Mathf.Atan2(v.x, a.x), Mathf.Atan2(v.y, a.y), Mathf.Atan2(v.z, a.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Clamp(this V3I v, int min, int max) => new(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Clamp(this V3I v, V3I min, V3I max) => new(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y), Mathf.Clamp(v.z, min.z, max.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Clamp(this V3I v, V3 min, V3 max) => new(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y), Mathf.Clamp(v.z, min.z, max.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Pow(this V3I v, float p) => new(Mathf.Pow(v.x, p), Mathf.Pow(v.y, p), Mathf.Pow(v.z, p));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Pow(this V3I v, V3 p) => new(Mathf.Pow(v.x, p.x), Mathf.Pow(v.y, p.y), Mathf.Pow(v.z, p.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Min(this V3I v, int a) => new(Mathf.Min(v.x, a), Mathf.Min(v.y, a), Mathf.Min(v.z, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Max(this V3I v, int a) => new(Mathf.Max(v.x, a), Mathf.Max(v.y, a), Mathf.Max(v.z, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Min(this V3I v, int a, int b) => new(Mathf.Min(v.x, a, b), Mathf.Min(v.y, a, b), Mathf.Min(v.z, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Max(this V3I v, int a, int b) => new(Mathf.Max(v.x, a, b), Mathf.Max(v.y, a, b), Mathf.Max(v.z, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Min(this V3I v, int a, int b, int c) => new(Mathf.Min(v.x, a, b, c), Mathf.Min(v.y, a, b, c), Mathf.Min(v.z, a, b, c));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Max(this V3I v, int a, int b, int c) => new(Mathf.Max(v.x, a, b, c), Mathf.Max(v.y, a, b, c), Mathf.Max(v.z, a, b, c));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3I v, float a) => new(Mathf.Min(v.x, a), Mathf.Min(v.y, a), Mathf.Min(v.z, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3I v, float a) => new(Mathf.Max(v.x, a), Mathf.Max(v.y, a), Mathf.Max(v.z, a));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3I v, float a, float b) => new(Mathf.Min(v.x, a, b), Mathf.Min(v.y, a, b), Mathf.Min(v.z, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3I v, float a, float b) => new(Mathf.Max(v.x, a, b), Mathf.Max(v.y, a, b), Mathf.Max(v.z, a, b));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Min(this V3I v, float a, float b, float c) => new(Mathf.Min(v.x, a, b, c), Mathf.Min(v.y, a, b, c), Mathf.Min(v.z, a, b, c));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Max(this V3I v, float a, float b, float c) => new(Mathf.Max(v.x, a, b, c), Mathf.Max(v.y, a, b, c), Mathf.Max(v.z, a, b, c));

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Min(this V3I v, V3I a) => new(Mathf.Min(v.x, a.x), Mathf.Min(v.y, a.y), Mathf.Min(v.z, a.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Max(this V3I v, V3I a) => new(Mathf.Max(v.x, a.x), Mathf.Max(v.y, a.y), Mathf.Max(v.z, a.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Min(this V3I v, V3I a, V3I b) => new(Mathf.Min(v.x, a.x, b.x), Mathf.Min(v.y, a.y, b.y), Mathf.Min(v.z, a.z, b.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Max(this V3I v, V3I a, V3I b) => new(Mathf.Max(v.x, a.x, b.x), Mathf.Max(v.y, a.y, b.y), Mathf.Max(v.z, a.z, b.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Min(this V3I v, V3I a, V3I b, V3I c) => new(Mathf.Min(v.x, a.x, b.x, c.x), Mathf.Min(v.y, a.y, b.y, c.y), Mathf.Min(v.z, a.z, b.z, c.z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Max(this V3I v, V3I a, V3I b, V3I c) => new(Mathf.Max(v.x, a.x, b.x, c.x), Mathf.Max(v.y, a.y, b.y, c.y), Mathf.Max(v.z, a.z, b.z, c.z));


		// Vector on vector action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Add(this V3I v, V3I b) => v + b; // new (v.x + b.x, v.y + b.y, v.z + b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Sub(this V3I v, V3I b) => v - b; // new (v.x - b.x, v.y - b.y, v.z - b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Div(this V3I v, V3I b) => new(v.x / b.x, v.y / b.y, v.z / b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Mul(this V3I v, V3I b) => new(v.x * b.x, v.y * b.y, v.z * b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Rem(this V3I v, V3I b) => new(v.x % b.x, v.y % b.y, v.z % b.z);

		// Vector on real vector action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V3I v, V3 b) => new(v.x + b.x, v.y + b.y, v.z + b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sub(this V3I v, V3 b) => new(v.x - b.x, v.y - b.y, v.z - b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Div(this V3I v, V3 b) => new(v.x / b.x, v.y / b.y, v.z / b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Mul(this V3I v, V3 b) => new(v.x * b.x, v.y * b.y, v.z * b.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Rem(this V3I v, V3 b) => new(v.x % b.x, v.y % b.y, v.z % b.z);

		// Vector on int action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Add(this V3I v, int b) => new(v.x + b, v.y + b, v.z + b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Sub(this V3I v, int b) => new(v.x - b, v.y - b, v.z - b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Div(this V3I v, int b) => v / b; // new (v.x / b, v.y / b, v.z / b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Mul(this V3I v, int b) => v * b; // new (v.x * b, v.y * b, v.z * b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I Rem(this V3I v, int b) => new(v.x % b, v.y % b, v.z % b);

		// Vector on float action
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Add(this V3I v, float b) => new(v.x + b, v.y + b, v.z + b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Sub(this V3I v, float b) => new(v.x - b, v.y - b, v.z - b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Div(this V3I v, float b) => new(v.x / b, v.y / b, v.z / b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Mul(this V3I v, float b) => new(v.x * b, v.y * b, v.z * b);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 Rem(this V3I v, float b) => new(v.x % b, v.y % b, v.z % b);


		// Set component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetX(this V3 v, int value) => new(value, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetY(this V3 v, int value) => new(v.x, value, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3 SetZ(this V3 v, int value) => new(v.x, v.y, value);

		// Add to component
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I AddX(this V3I v, int value) => new(v.x + value, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I AddY(this V3I v, int value) => new(v.x, v.y + value, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I AddZ(this V3I v, int value) => new(v.x, v.y, v.z + value);


		// Swizzling
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I xx(this V3I v) => new(v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I xy(this V3I v) => new(v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I xz(this V3I v) => new(v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I yx(this V3I v) => new(v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I yy(this V3I v) => new(v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I yz(this V3I v) => new(v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I zx(this V3I v) => new(v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I zy(this V3I v) => new(v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V2I zz(this V3I v) => new(v.z, v.z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xxx(this V3I v) => new(v.x, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xxy(this V3I v) => new(v.x, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xxz(this V3I v) => new(v.x, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xyx(this V3I v) => new(v.x, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xyy(this V3I v) => new(v.x, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xyz(this V3I v) => new(v.x, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xzx(this V3I v) => new(v.x, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xzy(this V3I v) => new(v.x, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I xzz(this V3I v) => new(v.x, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yxx(this V3I v) => new(v.y, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yxy(this V3I v) => new(v.y, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yxz(this V3I v) => new(v.y, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yyx(this V3I v) => new(v.y, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yyy(this V3I v) => new(v.y, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yyz(this V3I v) => new(v.y, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yzx(this V3I v) => new(v.y, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yzy(this V3I v) => new(v.y, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I yzz(this V3I v) => new(v.y, v.z, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zxx(this V3I v) => new(v.z, v.x, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zxy(this V3I v) => new(v.z, v.x, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zxz(this V3I v) => new(v.z, v.x, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zyx(this V3I v) => new(v.z, v.y, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zyy(this V3I v) => new(v.z, v.y, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zyz(this V3I v) => new(v.z, v.y, v.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zzx(this V3I v) => new(v.z, v.z, v.x);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zzy(this V3I v) => new(v.z, v.z, v.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static V3I zzz(this V3I v) => new(v.z, v.z, v.z);

		#endregion
		#endregion

	}
}