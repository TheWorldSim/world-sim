
import Experience from "./Experience/Experience.js"

const experience = new Experience(document.querySelector("canvas.webgl"))

function abc ()
{
    // import * as THREE from "three"
    // import { OrbitControls } from "three/examples/jsm/controls/OrbitControls.js"
    // import GUI from "lil-gui"
    // import { Timer } from "three/addons/misc/Timer.js"
    // import ground_vertex_shader from "./shaders/ground/vertex.glsl"
    // import ground_fragment_shader from "./shaders/ground/fragment.glsl"

    /**
     * Debug
     */
    const gui = new GUI()
    const debugObject = {}

    /**
     * Base
     */
    // Canvas
    const canvas = document.querySelector("canvas.webgl")

    // Scene
    const scene = new THREE.Scene()

    /**
     * Textures
     */
    const textureLoader = new THREE.TextureLoader()

    // const dtm_v1_texture_height_map = textureLoader.load("./textures/terrain/dtm_v2.png")
    // const satellite_v2b_texture = textureLoader.load("./textures/terrain/satellite_v2b.jpg")
    // satellite_v2b_texture.colorSpace = THREE.SRGBColorSpace

    // const bumpTexture = dtm_v1_texture_height_map
    // bumpTexture.wrapS = bumpTexture.wrapT = THREE.RepeatWrapping;

    // magnitude of normal displacement
    debugObject.bumpScale = 30.0
    debugObject.uTerrainColourMin = -0.018
    debugObject.uTerrainColourRange = 0.52

    const customUniforms = {
        textureMap:  { value: satellite_v2b_texture },
        bumpTexture: { value: bumpTexture },
        bumpScale:   { value: debugObject.bumpScale },
        uTerrainColourMin:   { value: debugObject.uTerrainColourMin },
        uTerrainColourRange: { value: debugObject.uTerrainColourRange },
    }

    const gui_folder_terrain = gui.addFolder("Terrain")

    gui_folder_terrain
        .add(debugObject, "bumpScale")
        .min(1).max(230).step(1).name("Scale")
        .onChange(() => {
            water.position.y = calc_water_y()
            customUniforms.bumpScale.value = debugObject.bumpScale
        })
    gui_folder_terrain
        .add(debugObject, "uTerrainColourMin")
        .min(-0.1).max(0.1).step(0.001).name("Colour Min")
        .onChange(() => {
            customUniforms.uTerrainColourMin.value = debugObject.uTerrainColourMin
        })
    gui_folder_terrain
        .add(debugObject, "uTerrainColourRange")
        .min(0).max(1.0).step(0.01).name("Colour Range")
        .onChange(() => {
            customUniforms.uTerrainColourRange.value = debugObject.uTerrainColourRange
        })


    const groundMaterial = new THREE.ShaderMaterial({
        uniforms: customUniforms,
        vertexShader: ground_vertex_shader,
        fragmentShader: ground_fragment_shader,
        // side: THREE.DoubleSide,
    })

    // // Make a ground material and set to the text file in static/textures/terrain/satellite_v2b.jpg
    // const groundMaterial = new THREE.Material({
    //     map: satellite_v2b_texture,
    //     bumpMap: bumpTexture,
    //     bumpScale: debugObject.bumpScale,
    //     side: THREE.DoubleSide,
    // })
    // const ground_plane_geometry = new THREE.PlaneGeometry(100, 100, 1000, 1000)
    // ground_plane_geometry.rotateX(-Math.PI / 2)
    // debugger
    // // deform the plane according to the height map in the bump texture
    // for (let i = 0; i < ground_plane_geometry.attributes.position.count; i++)
    // {
    //     const x = ground_plane_geometry.attributes.position.getX(i)
    //     const z = ground_plane_geometry.attributes.position.getZ(i)
    //     const y = bumpTexture.data[z * bumpTexture.width + x] / 255 * debugObject.bumpScale
    //     ground_plane_geometry.attributes.position.setY(i, y)
    // }


    const ground_plane = new THREE.Mesh(
        ground_plane_geometry,
        groundMaterial,
    )
    ground_plane.rotateX(-Math.PI / 2)
    ground_plane.position.y = 0
    scene.add(ground_plane)


    // const cube = new THREE.Mesh(
    //     new THREE.BoxGeometry(10, 10, 10),
    //     new THREE.MeshPhongMaterial({ color: 0x555555 })
    // )
    // scene.add(cube)

    /**
     * Simulate the sun moving across the sky
     */
    debugObject.sunColour = 0xf6f6f4

    const sun = new THREE.Vector3()
    let sunAngle = Math.PI / 4
    const sunDistance = 100
    const sunHeight = 20
    const sunSpeed = 0.01
    const sunLight = new THREE.DirectionalLight(new THREE.Color(debugObject.sunColour), 10)
    sunLight.position.set(0, 0, 0)
    scene.add(sunLight)

    gui.addColor(debugObject, "sunColour")
        .onChange(() => {
            sunLight.color.set(debugObject.sunColour)
        })

    const update_sun = () => {
        sun.x = Math.cos(sunAngle) * sunDistance
        sun.z = Math.sin(sunAngle) * sunDistance
        sun.y = sunHeight
        sunLight.position.copy(sun)
        sunAngle += sunSpeed
    }
    update_sun()

    /**
     * Water level
     */
    debugObject.waterLevel = 2
    debugObject.waterColour = 0x2589b2

    function calc_water_y ()
    {
        return ((debugObject.waterLevel - 13) / 100) * debugObject.bumpScale
    }

    gui.add(debugObject, "waterLevel")
        .min(0).max(11).step(0.01).name("Water Level")
        .onChange(() => {
            water.position.y = calc_water_y()
        })
    gui.addColor(debugObject, "waterColour")
        .onChange(() => {
            water.material.color.set(debugObject.waterColour)
        })

    const water = new THREE.Mesh(
        new THREE.PlaneGeometry(100, 100, 1000, 1000),
        new THREE.MeshStandardMaterial({
            color: new THREE.Color(debugObject.waterColour),
            transparent: true,
            opacity: 0.9,
        })
    )
    water.rotateX(-Math.PI / 2)
    water.position.y = calc_water_y()
    scene.add(water)


    /**
     * Lights
     */
    // const ambientLight = new THREE.AmbientLight(0xffffff, 0.5)
    // scene.add(ambientLight)

    // const directionalLight = new THREE.DirectionalLight(0xffffff, 0.5)
    // directionalLight.position.set(2, 2, 2)
    // scene.add(directionalLight)

    // /**
    //  * Mark the centre of the scene
    //  */
    // const sphere = new THREE.Mesh(
    //     new THREE.SphereGeometry(1, 4, 4),
    //     new THREE.MeshStandardMaterial({ color: 0x113355 })
    // )
    // scene.add(sphere)


    /**
     * Sizes
     */
    const sizes = {
        width: window.innerWidth,
        height: window.innerHeight
    }

    window.addEventListener('resize', () =>
    {
        // Update sizes
        sizes.width = window.innerWidth
        sizes.height = window.innerHeight

        // Update camera
        camera.aspect = sizes.width / sizes.height
        camera.updateProjectionMatrix()

        // Update renderer
        renderer.setSize(sizes.width, sizes.height)
        renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2))
    })

    /**
     * Camera
     */
    // Base camera
    const camera = new THREE.PerspectiveCamera(75, sizes.width / sizes.height, 0.1, 300)
    camera.position.x = 20
    camera.position.y = 20
    camera.position.z = 30
    scene.add(camera)

    // Controls
    const controls = new OrbitControls(camera, canvas)
    controls.enableDamping = true

    /**
     * Renderer
     */
    const renderer = new THREE.WebGLRenderer({
        canvas: canvas
    })
    renderer.setSize(sizes.width, sizes.height)
    renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2))

    /**
     * Animate
     */
    // const clock = new THREE.Clock()
    const timer = new Timer()

    const tick = () =>
    {
        timer.update()
        const elapsedTime = timer.getElapsed()

        update_sun()

        // Update controls
        controls.update()

        // Render
        renderer.render(scene, camera)

        // Call tick again on the next frame
        window.requestAnimationFrame(tick)
    }

    tick()
}