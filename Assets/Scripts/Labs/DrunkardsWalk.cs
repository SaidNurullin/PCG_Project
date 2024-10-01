using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkardsWalk : MonoBehaviour
{
    // Start is called before the first frame update
    private bool[,] array;
    [SerializeField] private int size = 10;
    [SerializeField] private int steps;
    [SerializeField] private int iterations;
    public SpriteRenderer imageRenderer;
    void Start()
    {
        array = new bool[size, size];
        for (int i = 0; i< iterations; i++)
        {
            int[] coords = FindPlace();
            MakeWalk(coords[0], coords[1], steps);
        }

        CreateTexture();
    }

    public void MakeWalk(int x, int y, int steps)
    {
        array[x, y] = true;
        for (int i = 0; i<steps; i++)
        {
            int rand = Random.Range(1, 5);

            if(rand == 1) x++;
            else if(rand == 2) y++;
            else if (rand == 3) x--;
            else y--;

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > size - 1) x = size - 1;
            if (y > size - 1) y = size - 1;

            array[x, y] = true;

        }
    }

    private int[] FindPlace()
    {
        int x = size / 2;
        int y = size / 2;

        while (array[x, y])
        {
            int rand = Random.Range(1, 5);

            if (rand == 1) x++;
            else if (rand == 2) y++;
            else if (rand == 3) x--;
            else y--;

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > size - 1) x = size - 1;
            if (y > size - 1) y = size - 1;
        }

        return new int[] { x, y };
    }

    private void CreateTexture()
    {
        Texture2D texture = new Texture2D(size, size);

        // Convert the bool array to pixel data
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // Set pixel color based on the bool value
                Color pixelColor = array[x, y] ? Color.white : Color.black;
                texture.SetPixel(x, y, pixelColor);
            }
        }

        // Apply the changes to the texture
        texture.Apply();

        // Create a Sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Assign the Sprite to the SpriteRenderer
        imageRenderer.sprite = sprite;
    }



}
