using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Puzzle[] AllPuzzles; // List of all puzzles in the game
    public GameObject Elevator; // The escape elevator


    private void Start()
    {

        AllPuzzles = FindObjectsOfType<Puzzle>();
    }

    private void Update()
    {
        int currentIteration = GetCurrentSnowflakeIteration(); // Get the player's current snowflake iteration (you'll need to implement this function)
        foreach (Puzzle puzzle in AllPuzzles)
        {
            if (puzzle.RequiredIteration == currentIteration && !puzzle.IsCompleted)
            {
                puzzle.Activate(); // Activate puzzle if conditions are met
            }
            else if (puzzle.RequiredIteration != currentIteration || puzzle.IsCompleted)
            {
                puzzle.Deactivate(); // Deactivate puzzle if conditions are no longer met 
            }
        }

        // Check if all puzzles are completed
        if (AllPuzzles.All(puzzle => puzzle.IsCompleted))
        {
            // Activate the elevator
            Elevator.SetActive(true);
        }
    }

    public int GetCurrentSnowflakeIteration()
    {
        int min_iteration = 2;
        foreach (Puzzle puzzle in AllPuzzles)
        {
            if (!puzzle.IsCompleted)
            {
                if(puzzle.RequiredIteration < min_iteration)
                {
                    min_iteration = puzzle.RequiredIteration;
                }
            }
        }
        return min_iteration;
    }
}
