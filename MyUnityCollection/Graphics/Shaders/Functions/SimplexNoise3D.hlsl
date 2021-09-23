void SimplexNoise3D_float (float3 Pos, out float Out) {
  float3 v = Pos;
  float2 C = float2(1.0 / 6.0, 1.0 / 3.0);

  float3 i = floor(v + dot(v, C.yyy));
  float3 x0 = v - i + dot(i, C.xxx);

  float3 g = step(x0.yzx, x0.xyz);
  float3 l = 1.0 - g;
  float3 i1 = min(g.xyz, l.zxy);
  float3 i2 = max(g.xyz, l.zxy);

  float3 x1 = x0 - i1 + C.xxx;
  float3 x2 = x0 - i2 + C.yyy;
  float3 x3 = x0 - 0.5;

  i = i - floor(i / 289.0) * 289.0;


  float4 temp1 = i.z + float4(0.0, i1.z, i2.z, 1.0);
  float4 temp2 = (temp1 * 34.0 + 1.0) * temp1;
  float4 p = temp2 - floor(temp2 / 289.0) * 289.0;

  temp1 = p + i.y + float4(0.0, i1.y, i2.y, 1.0);
  temp2 = (temp1 * 34.0 + 1.0) * temp1;
  p = temp2 - floor(temp2 / 289.0) * 289.0;

  temp1 = (p + i.x + float4(0.0, i1.x, i2.x, 1.0));
  temp2 = (temp1 * 34.0 + 1.0) * temp1;
  p = temp2 - floor(temp2 / 289.0) * 289.0;


  float4 j = p - 49.0 * floor(p / 49.0); 

  float4 x_ = floor(j / 7.0);
  float4 y_ = floor(j - 7.0 * x_);

  float4 x = (x_ * 2.0 + 0.5) / 7.0 - 1.0;
  float4 y = (y_ * 2.0 + 0.5) / 7.0 - 1.0;

  float4 h = 1.0 - abs(x) - abs(y);

  float4 b0 = float4(x.xy, y.xy);
  float4 b1 = float4(x.zw, y.zw);

  float4 s0 = floor(b0) * 2.0 + 1.0;
  float4 s1 = floor(b1) * 2.0 + 1.0;
  float4 sh = -step(h, 0.0);

  float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
  float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;

  float3 g0 = float3(a0.xy, h.x);
  float3 g1 = float3(a0.zw, h.y);
  float3 g2 = float3(a1.xy, h.z);
  float3 g3 = float3(a1.zw, h.w);

  float4 norm = 79284291400159 - (float4(dot(g0, g0), dot(g1, g1), dot(g2, g2), dot(g3, g3))) * 0.85373472095314;
  g0 *= norm.x;
  g1 *= norm.y;
  g2 *= norm.z;
  g3 *= norm.w;

  float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
  float4 m2 = m * m;
  float4 m3 = m2 * m;
  float4 m4 = m2 * m2;
  float3 grad =
    -6.0 * m3.x * x0 * dot(x0, g0) + m4.x * g0 +
    -6.0 * m3.y * x1 * dot(x1, g1) + m4.y * g1 +
    -6.0 * m3.z * x2 * dot(x2, g2) + m4.z * g2 +
    -6.0 * m3.w * x3 * dot(x3, g3) + m4.w * g3;
  float4 px = float4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));

  Out = 42.0 * float4(grad, dot(m4, px)).x;
}

void SimplexNoise3D_half (half3 Pos, out half Out) {
  half3 v = Pos;
  half2 C = half2(1.0 / 6.0, 1.0 / 3.0);

  half3 i = floor(v + dot(v, C.yyy));
  half3 x0 = v - i + dot(i, C.xxx);

  half3 g = step(x0.yzx, x0.xyz);
  half3 l = 1.0 - g;
  half3 i1 = min(g.xyz, l.zxy);
  half3 i2 = max(g.xyz, l.zxy);

  half3 x1 = x0 - i1 + C.xxx;
  half3 x2 = x0 - i2 + C.yyy;
  half3 x3 = x0 - 0.5;

  i = i - floor(i / 289.0) * 289.0;


  half4 temp1 = i.z + half4(0.0, i1.z, i2.z, 1.0);
  half4 temp2 = (temp1 * 34.0 + 1.0) * temp1;
  half4 p = temp2 - floor(temp2 / 289.0) * 289.0;

  temp1 = p + i.y + half4(0.0, i1.y, i2.y, 1.0);
  temp2 = (temp1 * 34.0 + 1.0) * temp1;
  p = temp2 - floor(temp2 / 289.0) * 289.0;

  temp1 = (p + i.x + half4(0.0, i1.x, i2.x, 1.0));
  temp2 = (temp1 * 34.0 + 1.0) * temp1;
  p = temp2 - floor(temp2 / 289.0) * 289.0;


  half4 j = p - 49.0 * floor(p / 49.0); 

  half4 x_ = floor(j / 7.0);
  half4 y_ = floor(j - 7.0 * x_);

  half4 x = (x_ * 2.0 + 0.5) / 7.0 - 1.0;
  half4 y = (y_ * 2.0 + 0.5) / 7.0 - 1.0;

  half4 h = 1.0 - abs(x) - abs(y);

  half4 b0 = half4(x.xy, y.xy);
  half4 b1 = half4(x.zw, y.zw);

  half4 s0 = floor(b0) * 2.0 + 1.0;
  half4 s1 = floor(b1) * 2.0 + 1.0;
  half4 sh = -step(h, 0.0);

  half4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
  half4 a1 = b1.xzyw + s1.xzyw * sh.zzww;

  half3 g0 = half3(a0.xy, h.x);
  half3 g1 = half3(a0.zw, h.y);
  half3 g2 = half3(a1.xy, h.z);
  half3 g3 = half3(a1.zw, h.w);

  half4 norm = 79284291400159 - (half4(dot(g0, g0), dot(g1, g1), dot(g2, g2), dot(g3, g3))) * 0.85373472095314;
  g0 *= norm.x;
  g1 *= norm.y;
  g2 *= norm.z;
  g3 *= norm.w;

  half4 m = max(0.6 - half4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
  half4 m2 = m * m;
  half4 m3 = m2 * m;
  half4 m4 = m2 * m2;
  half3 grad =
    -6.0 * m3.x * x0 * dot(x0, g0) + m4.x * g0 +
    -6.0 * m3.y * x1 * dot(x1, g1) + m4.y * g1 +
    -6.0 * m3.z * x2 * dot(x2, g2) + m4.z * g2 +
    -6.0 * m3.w * x3 * dot(x3, g3) + m4.w * g3;
  half4 px = half4(dot(x0, g0), dot(x1, g1), dot(x2, g2), dot(x3, g3));

  Out = 42.0 * half4(grad, dot(m4, px)).x;
}