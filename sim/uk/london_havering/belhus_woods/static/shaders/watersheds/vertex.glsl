
varying vec2 vUv;

void main()
{
    vec3 newPosition = position + normal;
	gl_Position = projectionMatrix * modelViewMatrix * vec4( newPosition, 1.0 );

    vUv = uv;
}
