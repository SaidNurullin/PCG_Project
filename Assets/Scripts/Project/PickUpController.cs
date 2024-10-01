using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{

    [SerializeField] private string target_tag;

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == target_tag)
        {
            other.gameObject.SetActive(false);
        }
    }
}
