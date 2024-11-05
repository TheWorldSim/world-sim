
import Experience from "./Experience/Experience.js"

new Experience(document.querySelector("canvas.webgl"))


import * as THREE from 'three';
function abc()
{
    // Example height map values (grayscale)
    const width = 256;
    const height = 256;
    const size = width * height;
    const data = new Uint8Array(size);

    // Fill the data array with height map values (0-255)
    for (let i = 0; i < size; i++) {
        data[i] = Math.random() * 255
    }

    // Create a DataTexture

    const heightMapTexture = new THREE.DataTexture(data, width, height, THREE.RedFormat, THREE.UnsignedByteType);
    heightMapTexture.needsUpdate = true;

    // Shader uniforms
    const customUniforms = {
        uHeightMap: { value: heightMapTexture }
    };

    // Vertex shader
    const vertexShader = `
        uniform sampler2D uHeightMap;
        varying float vHeight;

        void main() {
            vec4 heightData = texture2D(uHeightMap, uv);
            vHeight = heightData.r;
            vec3 newPosition = position + normal * vHeight * 10.0; // Scale height
            newPosition.z = vHeight * 0.2; // Scale height
            gl_Position = projectionMatrix * modelViewMatrix * vec4(newPosition, 1.0);
        }
    `;

    // Fragment shader
    const fragmentShader = `
        varying float vHeight;

        void main() {
            gl_FragColor = vec4(vHeight, 1.0, 1.0, 1.0); // Grayscale color based on height
        }
    `;

    // Create the material with the shaders and uniforms
    const material = new THREE.ShaderMaterial({
        uniforms: customUniforms,
        vertexShader: vertexShader,
        fragmentShader: fragmentShader
    });


    // Create a plane geometry and mesh
    const geometry = new THREE.PlaneGeometry(10, 10, width - 1, height - 1);
    const mesh = new THREE.Mesh(geometry, material);
    mesh.rotation.x = -Math.PI * 0.4;

    // Add the mesh to the scene
    const scene = new THREE.Scene();
    scene.add(mesh);

    // Set up the camera and renderer
    const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
    camera.position.z = 20;

    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth, window.innerHeight);
    document.body.appendChild(renderer.domElement);

    // Render loop
    function animate() {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
    }

    animate()
}
