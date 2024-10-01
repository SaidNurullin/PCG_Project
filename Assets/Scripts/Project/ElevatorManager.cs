using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject elevator;
    [SerializeField] private int speed;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxHeight;
    [SerializeField] private GameObject endScreen;

    private GameObject player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(DistanceToPlayer() < minDistance && elevator.transform.position.y < maxHeight)
        {
            Move(speed);
        }
        else if(DistanceToPlayer() > minDistance && elevator.transform.position.y > 0f)
        {
            Move(-speed);
        }

        if(player.transform.position.y > maxHeight)
        {
            endScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Move(int speed)
    {
        elevator.transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(elevator.transform.position, player.transform.position);
    }
}
