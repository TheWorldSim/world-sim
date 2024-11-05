
uniform sampler2D uBumpTexture;
uniform float uBumpScale;

// varying float vAmount;
varying vec2 vUv;

void main()
{
    // These fudge factors are used to minimse the very high or low values in
    // the bump map round the edges by moving the UV coordinates inwards.
    float fudge_offset = 0.001;
    float fudge_multiplier = 0.999;
	vec4 bumpData = texture2D( uBumpTexture, (uv * fudge_multiplier) + fudge_offset);

	float vertical_displacement = bumpData.r; // assuming map is grayscale it doesn't matter if you use r, g, or b.

	// move the position along the normal
    vec3 newPosition = position + normal * uBumpScale * vertical_displacement;
    newPosition.z -= (uBumpScale / 5.0);

	gl_Position = projectionMatrix * modelViewMatrix * vec4( newPosition, 1.0 );

    vUv = uv;
}


// #define PHONG

// varying vec3 vViewPosition;

// #include <common>
// #include <batching_pars_vertex>
// #include <uv_pars_vertex>
// #include <displacementmap_pars_vertex>
// #include <envmap_pars_vertex>
// #include <color_pars_vertex>
// #include <fog_pars_vertex>
// #include <normal_pars_vertex>
// #include <morphtarget_pars_vertex>
// #include <skinning_pars_vertex>
// #include <shadowmap_pars_vertex>
// #include <logdepthbuf_pars_vertex>
// #include <clipping_planes_pars_vertex>

// void main() {

// 	#include <uv_vertex>
// 	#include <color_vertex>
// 	#include <morphcolor_vertex>
// 	#include <batching_vertex>

// 	#include <beginnormal_vertex>
// 	#include <morphinstance_vertex>
// 	#include <morphnormal_vertex>
// 	#include <skinbase_vertex>
// 	#include <skinnormal_vertex>
// 	#include <defaultnormal_vertex>
// 	#include <normal_vertex>

// 	#include <begin_vertex>
// 	#include <morphtarget_vertex>
// 	#include <skinning_vertex>
// 	#include <displacementmap_vertex>
// 	#include <project_vertex>
// 	#include <logdepthbuf_vertex>
// 	#include <clipping_planes_vertex>

// 	vViewPosition = - mvPosition.xyz;

// 	#include <worldpos_vertex>
// 	#include <envmap_vertex>
// 	#include <shadowmap_vertex>
// 	#include <fog_vertex>

// }
