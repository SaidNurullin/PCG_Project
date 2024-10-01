using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresPuzzle : Puzzle
{
    private Transform[] wires;
    // Start is called before the first frame update
    void Start()
    {
        wires = GetComponentsInChildren<Transform>();

        foreach(Transform wire in wires)
        {
            if (wire == transform) continue;
            wire.eulerAngles = new(0, Random.Range(0, 4) * 90f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int counter = 0;
        foreach (Transform wire in wires)
        {
            if (wire == transform) continue;
            if(wire.name == "Straight")
            {
                if(Mathf.Approximately((int)wire.eulerAngles.y % 180, 0))
                {
                    counter++;
                }
            }
            else if(Mathf.Approximately((int)wire.eulerAngles.y, 0))
            {
                counter++;
            }
        }

        if(counter == wires.Length-1)
        {
            Complete();
        }

    }
}
