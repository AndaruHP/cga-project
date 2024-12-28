using UnityEngine;

public class PickUpAmmo : MonoBehaviour
{
    private bool inReach;
    public int ammoAmount = 1;
    private Gun gun;

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

        gun = FindObjectOfType<Gun>();
        if (gun == null)
        {
            Debug.LogError("Gun object not found in the scene!");
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
                Debug.Log("Player entered pickup range");
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
                Debug.Log("Player exited pickup range");
            }
        }
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ToggleGrabCrosshair(false);
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Gun object is not assigned!");
            }
        }
    }
}
