using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    private bool inReach;
    private PlayerInventory inventory;

    void Awake()
    {
        // Find the PlayerInventory component on the First Person Controller
        GameObject player = GameObject.Find("FirstPersonController");
        if (player != null)
        {
            inventory = player.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogError("PlayerInventory component not found on First Person Controller!");
            }
        }
        else
        {
            Debug.LogError("First Person Controller not found in scene!");
        }
    }

    void Start()
    {
        inReach = false;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ToggleGrabCrosshair(false);
        }
        else
        {
            Debug.LogError("UIManager not found in scene!");
        }

        // Double-check inventory reference
        if (inventory == null)
        {
            inventory = FindObjectOfType<PlayerInventory>();
            Debug.LogWarning("Attempting to find PlayerInventory through FindObjectOfType as fallback.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = true;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ToggleGrabCrosshair(true);
                Debug.Log("Player entered key pickup range");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ToggleGrabCrosshair(false);
                Debug.Log("Player exited key pickup range");
            }
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
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ToggleGrabCrosshair(false);
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Cannot pick up key: PlayerInventory reference is missing!");
            }
        }
    }
}