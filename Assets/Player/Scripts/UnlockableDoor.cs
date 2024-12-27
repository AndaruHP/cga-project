using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{
    public GameObject grabCrosshair;
    private bool inReach;
    public PlayerInventory inventory; 
    private bool isUnlocked = false;
    private Animator animator; 
    
    void Start()
    {
        inReach = false;
        grabCrosshair.SetActive(false);
        animator = GetComponent<Animator>();

        // Find the PlayerInventory script in the scene
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
            if (inventory != null && inventory.HasKey)
            {
                UnlockDoor();
            }
            else
            {
                Debug.Log("You need a key to unlock this door.");
            }
        }
    }

    void UnlockDoor()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            Debug.Log("Door Unlocked!");
            // Play unlock animation if available
            if (animator != null)
            {
                animator.SetTrigger("Unlock");
            }
            // Optionally, disable the collider to allow passage
            GetComponent<Collider>().enabled = false;
        }
    }
}
