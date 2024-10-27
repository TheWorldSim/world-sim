
precision mediump float;

uniform sampler2D textureMap;
uniform float uTerrainColourMin;
uniform float uTerrainColourRange;

varying vec2 vUv;

float calc_strength(float d)
{
    float distance_theshold = 0.8;
    float d2 = max(0.0, d - distance_theshold) * (1.0 / (1.0 - distance_theshold));

    return 1.0 - d2;
}

void main()
{
    vec4 colourFromTexture = (texture2D(textureMap, vUv) - uTerrainColourMin) / uTerrainColourRange;

    // As we approach the edge of the terrain, the colour will become more transparent
    vec2 vUv_0_centered = vUv - vec2(0.5);  // get uv in the range [-0.5, 0.5]
    vec2 vUv_abs = abs(vUv_0_centered) * 2.0;  // get uv in the range [0, 1]
    float x = calc_strength(vUv_abs.x);
    float y = calc_strength(vUv_abs.y);

    float strength = x * y;
    colourFromTexture.a = strength;

	gl_FragColor = colourFromTexture;
}



// #define PHONG

// uniform vec3 diffuse;
// uniform vec3 emissive;
// uniform vec3 specular;
// uniform float shininess;
// uniform float opacity;

// #include <common>
// #include <packing>
// #include <dithering_pars_fragment>
// #include <color_pars_fragment>
// #include <uv_pars_fragment>
// #include <map_pars_fragment>
// #include <alphamap_pars_fragment>
// #include <alphatest_pars_fragment>
// #include <alphahash_pars_fragment>
// #include <aomap_pars_fragment>
// #include <lightmap_pars_fragment>
// #include <emissivemap_pars_fragment>
// #include <envmap_common_pars_fragment>
// #include <envmap_pars_fragment>
// #include <fog_pars_fragment>
// #include <bsdfs>
// #include <lights_pars_begin>
// #include <normal_pars_fragment>
// #include <lights_phong_pars_fragment>
// #include <shadowmap_pars_fragment>
// #include <bumpmap_pars_fragment>
// #include <normalmap_pars_fragment>
// #include <specularmap_pars_fragment>
// #include <logdepthbuf_pars_fragment>
// #include <clipping_planes_pars_fragment>

// void main() {

// 	vec4 diffuseColor = vec4( diffuse, opacity );
// 	#include <clipping_planes_fragment>

// 	ReflectedLight reflectedLight = ReflectedLight( vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ), vec3( 0.0 ) );
// 	vec3 totalEmissiveRadiance = emissive;

// 	#include <logdepthbuf_fragment>
// 	#include <map_fragment>
// 	#include <color_fragment>
// 	#include <alphamap_fragment>
// 	#include <alphatest_fragment>
// 	#include <alphahash_fragment>
// 	#include <specularmap_fragment>
// 	#include <normal_fragment_begin>
// 	#include <normal_fragment_maps>
// 	#include <emissivemap_fragment>

// 	// accumulation
// 	#include <lights_phong_fragment>
// 	#include <lights_fragment_begin>
// 	#include <lights_fragment_maps>
// 	#include <lights_fragment_end>

// 	// modulation
// 	#include <aomap_fragment>

// 	vec3 outgoingLight = reflectedLight.directDiffuse + reflectedLight.indirectDiffuse + reflectedLight.directSpecular + reflectedLight.indirectSpecular + totalEmissiveRadiance;

// 	#include <envmap_fragment>
// 	#include <opaque_fragment>
// 	#include <tonemapping_fragment>
// 	#include <colorspace_fragment>
// 	#include <fog_fragment>
// 	#include <premultiplied_alpha_fragment>
// 	#include <dithering_fragment>

// }
