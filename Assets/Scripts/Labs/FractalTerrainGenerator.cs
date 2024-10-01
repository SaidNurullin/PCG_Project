using UnityEngine;

public class FractalTerrainGenerator : MonoBehaviour
{
    public int width = 512;
    public int height = 512;
    public float scale = 20f;
    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2f;

    public Material terrainMaterial;

    private void Start()
    {
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        // Create a new Texture2D for the terrain
        Texture2D terrainTexture = new Texture2D(width, height);

        // Generate the terrain data using a fractal algorithm
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Normalize coordinates to range [0, 1]
                float normalizedX = (float)x / width;
                float normalizedY = (float)y / height;

                // Calculate the fractal value at the current coordinate
                float heightValue = CalculateFractalValue(normalizedX, normalizedY);

                // Set the color of the pixel based on the height value
                terrainTexture.SetPixel(x, y, new Color(heightValue, heightValue, heightValue));
            }
        }

        // Apply the texture to the terrain material
        terrainMaterial.mainTexture = terrainTexture;

        // Create a new mesh for the terrain
        Mesh mesh = new Mesh();
        mesh.name = "Fractal Terrain";

        // Generate vertices and triangles for the mesh
        Vector3[] vertices = new Vector3[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                vertices[x + y * width] = new Vector3(x, CalculateFractalValue(x / (float)width, y / (float)height), y);
            }
        }

        int triangleIndex = 0;
        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                // First triangle
                triangles[triangleIndex++] = x + y * width;
                triangles[triangleIndex++] = x + (y + 1) * width;
                triangles[triangleIndex++] = (x + 1) + (y + 1) * width;

                // Second triangle
                triangles[triangleIndex++] = x + y * width;
                triangles[triangleIndex++] = (x + 1) + (y + 1) * width;
                triangles[triangleIndex++] = (x + 1) + y * width;
            }
        }

        // Set the mesh data
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate mesh bounds and normals
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Create a new GameObject and add the mesh to it
        GameObject terrain = new GameObject("Fractal Terrain");
        terrain.AddComponent<MeshFilter>().mesh = mesh;
        terrain.AddComponent<MeshRenderer>().material = terrainMaterial;
    }

    private float CalculateFractalValue(float x, float y)
    {
        float value = 0;
        float frequency = 1f;
        float amplitude = 1f;

        for (int i = 0; i < octaves; i++)
        {
            // Calculate the noise value at the current coordinates and frequency
            float noiseValue = Mathf.PerlinNoise((x * frequency) / scale, (y * frequency) / scale);

            // Add the noise value to the total value
            value += noiseValue * amplitude;

            // Update frequency and amplitude for the next octave
            frequency *= lacunarity;
            amplitude *= persistence;
        }

        // Return the final fractal value
        return value;
    }
}