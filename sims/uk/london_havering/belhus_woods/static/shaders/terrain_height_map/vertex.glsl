
uniform sampler2D uTerrainWatershedInputHeightMap;
uniform float uBumpScale;

varying vec2 vUv;
varying float vHeight;

void main()
{
    // These fudge factors are used to minimse the very high or low values in
    // the bump map round the edges by moving the UV coordinates inwards.
    float fudge_offset = 0.001;
    float fudge_multiplier = 0.999;
	vec4 heightData = texture2D(uTerrainWatershedInputHeightMap, (uv * fudge_multiplier) + fudge_offset);

	vHeight = heightData.r; // map is redscale so we use r

	// move the position along the normal
    vec3 newPosition = position + normal * uBumpScale * vHeight;
    // newPosition.z -= (uBumpScale / 5.0);

	gl_Position = projectionMatrix * modelViewMatrix * vec4( newPosition, 1.0 );

    vUv = uv;
}
