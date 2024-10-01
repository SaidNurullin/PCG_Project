using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePuzzle : Puzzle
{

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            Complete();
        }
    }
}
