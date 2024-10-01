using UnityEngine;

public class DiamondSquareTerrainGenerator : MonoBehaviour
{
    public int size = 512;
    public float scale = 20f;
    public float roughness = 0.5f;

    public Material terrainMaterial;

    private float[,] heightMap;

    private void Start()
    {
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        // Initialize the heightmap
        heightMap = new float[size + 1, size + 1];

        // Set corner values
        heightMap[0, 0] = Random.value;
        heightMap[0, size] = Random.value;
        heightMap[size, 0] = Random.value;
        heightMap[size, size] = Random.value;

        // Diamond-Square algorithm
        int stepSize = size;
        float randomValue = 0;
        while (stepSize > 1)
        {
            // Diamond step
            for (int x = stepSize / 2; x < size; x += stepSize)
            {
                for (int y = stepSize / 2; y < size; y += stepSize)
                {
                    randomValue = Random.Range(-roughness * stepSize, roughness * stepSize);
                    heightMap[x, y] = (heightMap[x - stepSize / 2, y - stepSize / 2] +
                                     heightMap[x - stepSize / 2, y + stepSize / 2] +
                                     heightMap[x + stepSize / 2, y - stepSize / 2] +
                                     heightMap[x + stepSize / 2, y + stepSize / 2]) / 4 + randomValue;
                }
            }

            // Square step
            for (int x = 0; x < size; x += stepSize)
            {
                for (int y = 0; y < size; y += stepSize)
                {
                    if (x == 0 || y == 0 || x == size || y == size) continue; // Skip edge squares

                    randomValue = Random.Range(-roughness * stepSize, roughness * stepSize);
                    heightMap[x, y] = (heightMap[x - stepSize / 2, y] +
                                     heightMap[x + stepSize / 2, y] +
                                     heightMap[x, y - stepSize / 2] +
                                     heightMap[x, y + stepSize / 2]) / 4 + randomValue;
                }
            }

            stepSize /= 2;
            roughness *= 0.5f; // Reduce roughness for finer details
        }

        // Create a new Texture2D for the terrain
        Texture2D terrainTexture = new Texture2D(size + 1, size + 1);

        // Generate the terrain data using the heightmap
        for (int x = 0; x <= size; x++)
        {
            for (int y = 0; y <= size; y++)
            {
                // Normalize coordinates to range [0, 1]
                float normalizedX = (float)x / size;
                float normalizedY = (float)y / size;

                // Calculate the height value at the current coordinate
                float heightValue = heightMap[x, y] * scale;

                // Set the color of the pixel based on the height value
                terrainTexture.SetPixel(x, y, new Color(heightValue, heightValue, heightValue));
            }
        }

        // Apply the texture to the terrain material
        terrainMaterial.mainTexture = terrainTexture;

        // Create a new mesh for the terrain
        Mesh mesh = new Mesh();
        mesh.name = "Diamond Square Terrain";

        // Generate vertices and triangles for the mesh
        Vector3[] vertices = new Vector3[(size + 1) * (size + 1)];
        int[] triangles = new int[size * size * 6];

        for (int x = 0; x <= size; x++)
        {
            for (int y = 0; y <= size; y++)
            {
                vertices[x + y * (size + 1)] = new Vector3(x, heightMap[x, y] * scale, y);
            }
        }

        int triangleIndex = 0;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // First triangle
                triangles[triangleIndex++] = x + y * (size + 1);
                triangles[triangleIndex++] = x + (y + 1) * (size + 1);
                triangles[triangleIndex++] = (x + 1) + (y + 1) * (size + 1);

                // Second triangle
                triangles[triangleIndex++] = x + y * (size + 1);
                triangles[triangleIndex++] = (x + 1) + (y + 1) * (size + 1);
                triangles[triangleIndex++] = (x + 1) + y * (size + 1);
            }
        }

        // Set the mesh data
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate mesh bounds and normals
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // Create a new GameObject and add the mesh to it
        GameObject terrain = new GameObject("Diamond Square Terrain");
        terrain.AddComponent<MeshFilter>().mesh = mesh;
        terrain.AddComponent<MeshRenderer>().material = terrainMaterial;
    }
}