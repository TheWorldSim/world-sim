export default [
    // {
    //     name: "environmentMapTexture",
    //     type: "cubeTexture",
    //     path:
    //     [
    //         "textures/environmentMap/px.jpg",
    //         "textures/environmentMap/nx.jpg",
    //         "textures/environmentMap/py.jpg",
    //         "textures/environmentMap/ny.jpg",
    //         "textures/environmentMap/pz.jpg",
    //         "textures/environmentMap/nz.jpg"
    //     ]
    // },
    {
        name: "dtm_v1_texture_height_map",
        type: "texture",
        path: "textures/terrain/dtm_v2.png"
    },
    {
        name: "satellite_v2b_texture_colour",
        type: "texture",
        path: "textures/terrain/satellite_v2b.jpg"
    },
    {
        name: "terrain_vertex_shader",
        type: "shader",
        path: "shaders/terrain/vertex.glsl"
    },
    {
        name: "terrain_fragment_shader",
        type: "shader",
        path: "shaders/terrain/fragment.glsl"
    },
    {
        name: "water_vertex_shader",
        type: "shader",
        path: "shaders/water/vertex.glsl"
    },
    {
        name: "water_fragment_shader",
        type: "shader",
        path: "shaders/water/fragment.glsl"
    },
    {
        name: "grassColorTexture",
        type: "texture",
        path: "textures/dirt/color.jpg"
    },
    {
        name: "grassNormalTexture",
        type: "texture",
        path: "textures/dirt/normal.jpg"
    },
    {
        name: "foxModel",
        type: "gltfModel",
        path: "models/Fox/glTF/Fox.gltf"
    },
]
