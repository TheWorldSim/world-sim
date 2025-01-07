
uniform sampler2D uTerrainHeightMap;
uniform float uBumpScale;

varying vec2 vUv;

#include <calculate_new_position_from_height_map>

void main()
{
    vec3 newPosition = calculateNewPosition(uBumpScale, uTerrainHeightMap);

	gl_Position = projectionMatrix * modelViewMatrix * vec4( newPosition, 1.0 );

    vUv = uv;
}
