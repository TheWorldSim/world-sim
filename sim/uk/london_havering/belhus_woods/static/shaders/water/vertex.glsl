uniform sampler2D uMinWatershedHeightMap;
uniform sampler2D uTerrainHeightMap;
uniform float uBumpScale;
uniform float uHeightOffset;

varying float vOpacity;
varying vec2 vUv;

void main() {
    vec4 heightData = texture2D(uMinWatershedHeightMap, uv);
    float vHeight = heightData.r + uHeightOffset;
    vec3 newPosition = position + normal * uBumpScale * vHeight; // Scale height
    gl_Position = projectionMatrix * modelViewMatrix * vec4(newPosition, 1.0);

    vec4 groundHeightData = texture2D(uTerrainHeightMap, uv);
    float vHeightAboveGround = vHeight - groundHeightData.r;
    vOpacity = step(0.0, vHeightAboveGround);

    vUv = uv;
}
