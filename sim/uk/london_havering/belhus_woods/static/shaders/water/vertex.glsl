
// uniform sampler2D bumpTexture;
// uniform float bumpScale;

varying vec2 vUv;

void main()
{
	// vec4 bumpData = texture2D( bumpTexture, (uv * fudge_multiplier) + fudge_offset);

	// float vertical_displacement = bumpData.r; // assuming map is grayscale it doesn't matter if you use r, g, or b.

	// // move the position along the normal
    // vec3 newPosition = position + normal * bumpScale * (vertical_displacement - 0.3);
    vec3 newPosition = position + normal;
    newPosition.z = 0.0;

	gl_Position = projectionMatrix * modelViewMatrix * vec4(newPosition, 1.0);

    vUv = uv;
}
