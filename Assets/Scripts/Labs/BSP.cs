using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public int x, y, width, height;
    public Node leftChild;
    public Node rightChild;
    public bool isLeaf => leftChild == null && rightChild == null;

    public Node(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
}

public class BSP : MonoBehaviour
{
    public int width = 100;          // Width of the BSP space
    public int height = 100;         // Height of the BSP space
    public int minSize = 20;         // Minimum size of a room
    public int maxSize = 40;         // Maximum size of a room

    private Node rootNode;           // Root of the BSP tree

    private Texture2D texture;       // Texture to draw on
    private SpriteRenderer spriteRenderer; // Sprite Renderer to display the texture

    void Start()
    {
        rootNode = new Node(0, 0, width, height);
        texture = new Texture2D(width, height);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // Set texture pixels
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.black; // Default color for the background
        }
        texture.SetPixels(pixels);
        texture.Apply();

        // Set the sprite renderer
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        SplitNode(rootNode);
        DrawMap();
    }

    private void SplitNode(Node node)
    {
        // Check if the node should be split
        if (node.width > minSize && node.height > minSize)
        {
            bool splitHorizontally;
            float rand = Random.Range(0f, 1f);
            if (rand < 0.9f)
            {
                splitHorizontally = node.height >= node.width;
            }
            else
            {
                splitHorizontally = node.height <= node.width;
            }

            if (splitHorizontally)
            {
                // Calculate random height for split
                int splitHeight = Random.Range(minSize, Mathf.Min(maxSize, node.height - minSize));
                node.leftChild = new Node(node.x, node.y, node.width, splitHeight);
                node.rightChild = new Node(node.x, node.y + splitHeight, node.width, node.height - splitHeight);
            }
            else
            {
                // Calculate random width for split
                int splitWidth = Random.Range(minSize, Mathf.Min(maxSize, node.width - minSize));
                node.leftChild = new Node(node.x, node.y, splitWidth, node.height);
                node.rightChild = new Node(node.x + splitWidth, node.y, node.width - splitWidth, node.height);
            }

            // Recursively split the children nodes
            SplitNode(node.leftChild);
            SplitNode(node.rightChild);
        }
    }

    private void DrawMap()
    {
        // Use colors to visualize rooms in the texture
        Color roomColor = Color.white;  // Color for rooms

        DrawNode(rootNode, roomColor);
        texture.Apply();
    }

    private void DrawNode(Node node, Color roomColor)
    {
        if (node.isLeaf)
        {
            // Draw the room
            for (int x = node.x; x < node.x + node.width - node.width / 3; x++)
            {
                for (int y = node.y; y < node.y + node.height - node.height / 3; y++)
                {
                    // Ensure we are within bounds
                    if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                    {
                        texture.SetPixel(x, y, roomColor);
                    }
                }
            }
        }
        else
        {
            // Recursively draw child nodes
            DrawNode(node.leftChild, roomColor);
            DrawNode(node.rightChild, roomColor);
        }
    }

    void OnDestroy()
    {
        // Clean up the texture to free memory
        Destroy(texture);
    }
}
