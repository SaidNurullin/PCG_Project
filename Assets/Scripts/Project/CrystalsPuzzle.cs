using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalsPuzzle : Puzzle
{
    public GameObject crystalPrefab; // The prefab for the crystal
    public int numCrystals = 10; // Number of crystals to spawn
    public Vector2 spawnAreaSize = new Vector2(10f, 10f); // Size of the spawning area
    public Vector2 spawnAreaCenter = Vector2.zero; // Center of the spawning area
    public Text text;
    public GameObject image;

    private List<GameObject> crystals = new();

    void Start()
    {
        SpawnCrystals();
    }

    public void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            image.SetActive(true);
        }
        int counter = 0;
        foreach (GameObject crystal in crystals)
        {
            if (!crystal.activeInHierarchy)
            {
                counter++;
            }
        }

        text.text = "" + counter + "/" + crystals.Count;

        if(counter == crystals.Count)
        {
            image.SetActive(false);
            description_text.text = "";
            Complete();
        }
    }

    void SpawnCrystals()
    {
        for (int i = 0; i < numCrystals; i++)
        {
            Vector3 randomPosition = RandomPointInArea(); // Get a random position within the area

            GameObject crystal = Instantiate(crystalPrefab, randomPosition, Quaternion.identity, transform); // Spawn the crystal
            crystals.Add(crystal);
        }
    }

    Vector3 RandomPointInArea()
    {
        float randomX = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2f, spawnAreaCenter.x + spawnAreaSize.x / 2f);
        float randomZ = Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2f, spawnAreaCenter.y + spawnAreaSize.y / 2f);
        return new Vector3(randomX, 0.2f, randomZ);
    }
}
