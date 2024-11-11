uniform sampler2D uMinWatershedHeightMap;
uniform sampler2D uMaxWatershedHeightMap;
uniform sampler2D uTerrainHeightMap;
uniform float uBumpScale;
uniform float uHeightOffset;
uniform float uHeightMinMaxMix;

varying float vOpacity;
varying vec2 vUv;

void main() {
    vec4 minHeightData = texture2D(uMinWatershedHeightMap, uv);
    vec4 maxHeightData = texture2D(uMaxWatershedHeightMap, uv);
    float vHeight = mix(minHeightData.r, maxHeightData.r, uHeightMinMaxMix) + uHeightOffset;
    // float vHeight = minHeightData.r + uHeightOffset;
    vec3 newPosition = position + normal * uBumpScale * vHeight; // Scale height
    gl_Position = projectionMatrix * modelViewMatrix * vec4(newPosition, 1.0);

    vec4 groundMinHeightData = texture2D(uTerrainHeightMap, uv);
    float vHeightAboveGround = vHeight - groundMinHeightData.r;
    // vOpacity = step(0.0, vHeightAboveGround);
    vOpacity = smoothstep(0.0, 0.02, vHeightAboveGround);

    vUv = uv;
}
