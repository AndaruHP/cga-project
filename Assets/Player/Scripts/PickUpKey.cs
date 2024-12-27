using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    public GameObject grabCrosshair;
    private bool inReach;
    public PlayerInventory inventory; 

    void Start()
    {
        inReach = false;
        grabCrosshair.SetActive(false);

        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("PlayerInventory not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = true;
            grabCrosshair.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = false;
            grabCrosshair.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null)
            {
                inventory.HasKey = true;
                Debug.Log("Key picked up!");
                grabCrosshair.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}