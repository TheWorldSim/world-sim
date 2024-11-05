uniform sampler2D uHeightMap;
uniform float uBumpScale;
// uniform float uBumpScale2;
uniform float uHeightOffset;

varying float vHeight;
varying vec2 vUv;

void main() {
    vec4 heightData = texture2D(uHeightMap, uv);
    vHeight = heightData.r + uHeightOffset;
    vec3 newPosition = position + normal * uBumpScale * vHeight; // Scale height
    gl_Position = projectionMatrix * modelViewMatrix * vec4(newPosition, 1.0);

    vUv = uv;
}
