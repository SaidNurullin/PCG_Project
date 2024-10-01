using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private float _scale = 20f;

    [SerializeField] private Terrain terrain;

    private float[,] _heights;
    [SerializeField] private Vector2 _offset_coefficient = new Vector2(1000, 1000);

    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private int seed;

    // Color gradient for height mapping
    public Gradient heightGradient;

    // Minimum and maximum heights for color mapping
    public float minHeight = 0f;
    public float maxHeight = 100f;

    public float[,] GenerateHeights(int _width, int _height, int _seed)
    {
        float[,] heights = new float[_width, _height];
        Vector2 offset = CalculateOffset(_seed);

        for (int x = 0; x < _width; ++x)
        {
            for (int y = 0; y < _height; ++y)
            {
                heights[x, y] = Mathf.PerlinNoise((x + offset.x) / _scale, (y + offset.y) / _scale);
            }
        }

        _heights = heights;
        return heights;
    }

    protected Vector2 CalculateOffset(int _seed)
    {
        return new Vector2(
            _seed % _offset_coefficient.x,
            _seed % _offset_coefficient.y);
    }

    public void Start()
    {
        TerrainData terrain_data = terrain.terrainData;
        terrain_data.heightmapResolution = Width + 1;
        terrain_data.size = new Vector3(Width, terrain_data.size.y, Height);

        float[,] heights = GenerateHeights(Width, Height, seed);
        terrain_data.SetHeights(0, 0, heights);

        Texture2D terrainTexture = new Texture2D(terrain_data.heightmapResolution, terrain_data.heightmapResolution);
        float[,,] splatmapData = new float[terrain_data.alphamapWidth, terrain_data.alphamapHeight, terrain_data.alphamapLayers];

        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                // Get the height at the current point
                float height = heights[x, z] * terrain_data.size.y;

                // Calculate the normalized height value between minHeight and maxHeight
                float normalizedHeight = Mathf.InverseLerp(minHeight, maxHeight, height);

                // Get the color from the gradient based on the normalized height
                Color color = heightGradient.Evaluate(normalizedHeight);

                float splatValue = color.r; // Assuming red channel represents the splat value

                // Set the splat value for the current point on the splatmap
                splatmapData[x, z, 0] = splatValue;

            }
        }
        terrain_data.SetAlphamaps(0, 0, splatmapData);
        //terrainTexture.Apply();
        //terrain_data.SetDetailResolution(terrain_data.heightmapResolution, 16);
        //terrain_data.baseMapResolution = terrainTexture.width;
        //terrain_data.terrainLayers = new TerrainLayer[] {
        //new TerrainLayer { diffuseTexture = terrainTexture } };

    }

    public float[,] Heights { get { return _heights; } }
}
