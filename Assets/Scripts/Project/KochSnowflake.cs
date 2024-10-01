using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KochSnowflake : MonoBehaviour
{
    // Settings
    public int iterations = 4; // Number of iterations for the fractal
    public float scale = 1.0f; // Scale of the snowflake
    public Vector3 center = Vector3.zero; // Center of the snowflake
    public GameObject wallPrefab; // Prefab for the wall
    public float delta = 0.1f;
    public float increaseRadius = 2f;
    public float decreaseRadius = 4f;
    public int minScale;
    public int maxScale;
    public int deltaScale;
    public float wallHeight;
    public float currentMaxScale;

    private Dictionary<int, int> iterationScaleMap = new();
    // Internal variables
    private List<Vector3> points = new List<Vector3>();

    private List<GameObject> walls = new List<GameObject>();

    private GameObject player;

    public void Start()
    {
        GenerateSnowflake();
        RemoveDuplicatePoints(points);
        SpawnWalls();

        player = FindObjectOfType<PlayerMovement>().gameObject;

        iterationScaleMap.Add(0, minScale);
        iterationScaleMap.Add(1, (minScale+maxScale)/2);
        iterationScaleMap.Add(2, maxScale);
    }

    public void Update()
    {

        UpdateMaxScale();

        for (int i = 0; i<3; i++)
        {
            if (iterationScaleMap[i] - deltaScale <= scale && scale <= iterationScaleMap[i] + deltaScale)
            {
                iterations = i;
                GenerateSnowflake();
                RemoveDuplicatePoints(points);
                SpawnWalls();

            }
        }

        if (CheckWalls(increaseRadius) && scale < maxScale  && scale < currentMaxScale)
        {
            ChangeScale(delta);
            ScaleWalls();
        }

        if (!CheckWalls(decreaseRadius) && scale > minScale)
        {
            ChangeScale(-delta);
            ScaleWalls();
        }

        if (player.transform.position.y > 8f)
        {
            iterations = 2;
            scale = maxScale;
            GenerateSnowflake();
            RemoveDuplicatePoints(points);
            SpawnWalls();
        }
    }


    private bool CheckWalls(float radius)
    {

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius);
        foreach(Collider col in colliders)
        {
            if(col.gameObject.layer == walls[0].layer)
            {
                return true;
            }
        }

        // If no collision is found, return false
        return false;
    }

    private void RemoveDuplicatePoints(List<Vector3> points)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                if (points[i] == points[j])
                {
                    // Remove the duplicate point
                    points.RemoveAt(j);
                    j--; // Adjust the loop index after removing a point
                }
            }
        }
    }

    // Generates the points of the Koch snowflake
    private void GenerateSnowflake()
    {
        points.Clear();

        // Start with an equilateral triangle
        points.Add(new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 2f));
        points.Add(new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2f));
        points.Add(new Vector3(1, 0, 0));

        // Recursively subdivide the edges
        for (int i = 0; i < iterations; i++)
        {
            List<Vector3> newPoints = new List<Vector3>();
            for (int j = 0; j < points.Count; j++)
            {
                int k = (j + 1) % points.Count;
                Vector3 p1 = points[j];
                Vector3 p2 = points[k];

                // Calculate points for the new Koch curve
                Vector3 p3 = p1 + (p2 - p1) / 3f;
                Vector3 p4 = p1 + 2f * (p2 - p1) / 3f;
                Vector3 p5 = new Vector3(p4.x + (p3.x - p4.x) * Mathf.Cos(60f * Mathf.Deg2Rad) - (p3.z - p4.z) * Mathf.Sin(60f * Mathf.Deg2Rad),
                                           0,
                                           p4.z + (p3.x - p4.x) * Mathf.Sin(60f * Mathf.Deg2Rad) + (p3.z - p4.z) * Mathf.Cos(60f * Mathf.Deg2Rad));

                newPoints.Add(p1);
                newPoints.Add(p3);
                newPoints.Add(p5);
                newPoints.Add(p4);
                newPoints.Add(p2);
            }
            points = newPoints;
        }
    }


    private void SpawnWalls()
    {
        if(walls.Count > 0)
        {
            foreach(GameObject g in walls)
            {
                Destroy(g);
            }

            walls.Clear();
        }
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 p1 = center + points[i % points.Count] * scale;
            Vector3 p2 = center + points[(i + 1) % points.Count] * scale;

            // Calculate the position and rotation of the wall
            Vector3 wallPosition = (p1 + p2) / 2f + Vector3.up * wallHeight/2;
            Quaternion wallRotation = Quaternion.LookRotation(p2 - p1, Vector3.up);

            // Instantiate a wall between each pair of points
            GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation);
            wall.transform.localScale = new Vector3(0.2f, wallHeight, Vector3.Distance(p1, p2));
            walls.Add(wall);
        }

    }

    private void ScaleWalls()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 p1 = center + points[i % points.Count] * scale;
            Vector3 p2 = center + points[(i + 1) % points.Count] * scale;

            // Calculate the position and rotation of the wall
            Vector3 wallPosition = (p1 + p2) / 2f + Vector3.up * wallHeight/2;
            Quaternion wallRotation = Quaternion.LookRotation(p2 - p1, Vector3.up);

            // Instantiate a wall between each pair of points
            GameObject wall = walls[i];
            wall.transform.position = wallPosition;
            wall.transform.rotation = wallRotation;
            wall.transform.localScale = new Vector3(0.2f, wallHeight, Vector3.Distance(p1, p2));
        }
    }

    private void ChangeScale(float delta)
    {
        scale += delta;
    }

    private void UpdateMaxScale()
    {
        PuzzleManager manager = FindObjectOfType<PuzzleManager>();
        if(manager != null)
        {
            int iter = manager.GetCurrentSnowflakeIteration();
            currentMaxScale = iterationScaleMap[iter];
        }

    }
}