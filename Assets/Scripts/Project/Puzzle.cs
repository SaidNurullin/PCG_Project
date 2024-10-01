using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle: MonoBehaviour
{
    public int RequiredIteration; // The snowflake iteration this puzzle spawns at
    public Transform SpawnPoint; // The puzzle's location in the snowflake
    public GameObject PuzzleObject; // The prefab for the puzzle UI or object
    public bool IsActive; // Flag for whether the puzzle is currently active 
    public bool IsCompleted; // Flag for whether the puzzle is solved 
    public List<Puzzle> NextPuzzles; // List of puzzles to be activated after completion 
    public Text description_text;
    public string description;

    public void Activate()
    {
        IsActive = true;
        PuzzleObject.SetActive(true); // Activate the puzzle object
        description_text.text = description;
    }

    public void Deactivate()
    {
        IsActive = false;
        PuzzleObject.SetActive(false); // Deactivate the puzzle object
    }

    public void Complete()
    {
        IsCompleted = true;
        Deactivate();
        foreach (Puzzle puzzle in NextPuzzles)
        {
            puzzle.Activate(); // Activate the next puzzles
        }
    }
}
