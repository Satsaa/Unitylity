
void CalculateLevelOfDetail_float(in Texture2D t, sampler s, float2 uv, out float Out) {
  #if defined(SHADERGRAPH_PREVIEW)
      Out = 0;
  #else
    #if defined(SHADER_API_D3D11) || defined(SHADER_API_GLCORE)
      Out = t.CalculateLevelOfDetail(s, uv);
    #else
      Out = 0;
    #endif
  #endif
}

void CalculateLevelOfDetail_half(in Texture2D t, sampler s, half2 uv, out half Out) {
  #if defined(SHADERGRAPH_PREVIEW)
      Out = 0;
  #else
    #if defined(SHADER_API_D3D11) || defined(SHADER_API_GLCORE)
      Out = t.CalculateLevelOfDetail(s, uv);
    #else
      Out = 0;
    #endif
  #endif
}