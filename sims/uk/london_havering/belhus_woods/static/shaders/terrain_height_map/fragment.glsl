
precision mediump float;

varying vec2 vUv;
varying float vHeight;

float calc_strength(float d)
{
    float distance_theshold = 0.95;
    float d2 = max(0.0, d - distance_theshold) * (1.0 / (1.0 - distance_theshold));

    return 1.0 - d2;
}

void main()
{
    // vec4 colourFromTexture = (texture2D(uTextureMap, vUv) - uTerrainColourMin) / uTerrainColourRange;

    vec4 colourFromTexture = vec4(vHeight/1.5, 0.5 + vHeight/2.0, 0.5, 1.0);

    // As we approach the edge of the terrain, the colour will become more transparent
    vec2 vUv_0_centered = vUv - vec2(0.5);  // get uv in the range [-0.5, 0.5]
    vec2 vUv_abs = abs(vUv_0_centered) * 2.0;  // get uv in the range [0, 1]
    float x = calc_strength(vUv_abs.x);
    float y = calc_strength(vUv_abs.y);

    float strength = x * y;
    colourFromTexture.a = strength;

	gl_FragColor = colourFromTexture;
}
