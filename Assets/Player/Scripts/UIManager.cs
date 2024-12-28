using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public GameObject grabCrosshair;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("UIManager initialized");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Find the GrabCrosshair if not assigned
        if (grabCrosshair == null)
        {
            grabCrosshair = GameObject.Find("GrabCrosshair");
            if (grabCrosshair == null)
            {
                Debug.LogError("GrabCrosshair not found in scene! Make sure it exists and is named correctly.");
            }
            else
            {
                Debug.Log("GrabCrosshair found and assigned");
            }
        }
    }

    // Add this method to help with debugging
    public void ToggleGrabCrosshair(bool show)
    {
        if (grabCrosshair != null)
        {
            grabCrosshair.SetActive(show);
            Debug.Log($"GrabCrosshair visibility set to: {show}");
        }
        else
        {
            Debug.LogError("Attempted to toggle GrabCrosshair but it's not assigned!");
        }
    }
} 