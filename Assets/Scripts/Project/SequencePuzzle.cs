using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequencePuzzle : Puzzle
{

    public List<GameObject> symbols; // List of sprites for the symbols
    public List<GameObject> player_symbols;
    public GameObject inputPanel; // The panel with buttons for symbol input
    public int sequenceLength;
    public List<GameObject> buttons;
    public GameObject activationButton;
    public GameObject resetButton;
    public GameObject acceptButton;

    private List<int> sequence = new List<int>(); // Stores the correct symbol sequence
    private List<int> playerInput = new List<int>(); // Stores the player's input sequence

    // Start is called before the first frame update
    void Start()
    {
        // Generate a random symbol sequence
        GenerateSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckClick(activationButton))
        {
            StartCoroutine(FlashSymbols());
        }

        for(int i = 0; i < buttons.Count; i++)
        {
            if (CheckClick(buttons[i]))
            {
                playerInput.Add(i);
                DisplaySymbol(i);
            }
        }

        if (CheckClick(resetButton))
        {
            ResetSequence();
        }

        if (CheckClick(acceptButton))
        {
            if (CheckSequence())
            {
                Complete();
            }
        }
    }

    private void GenerateSequence()
    {
        sequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            sequence.Add(Random.Range(0, symbols.Count)); // Generate random indices for symbols
        }
    }

    private IEnumerator FlashSymbols()
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            symbols[sequence[i]].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            symbols[sequence[i]].SetActive(false);
            yield return new WaitForSeconds(0.5f); // Wait for half a second
        }
    }

    private void DisplaySymbol(int idx)
    {
        for (int i = 0; i < player_symbols.Count; i++)
        {
            player_symbols[i].SetActive(false);
        }

        player_symbols[idx].SetActive(true);
    }

    private bool CheckSequence()
    {
        if (playerInput.Count != sequence.Count) return false;
        for (int i = 0; i < sequence.Count; i++)
        {
            if (playerInput[i] != sequence[i])
            {
                return false; // Incorrect sequence
            }
        }
        return true; // Correct sequence
    }

    private bool CheckClick(GameObject targetObject)
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse click
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out hit))
            {
                if (hit.collider.gameObject == targetObject) // Check if clicked object matches the target
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ResetSequence()
    {
        playerInput.Clear();
    }
}
