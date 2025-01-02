using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public GameObject grabCrosshair;
    private Canvas uiCanvas;
    private static bool isInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (!isInitialized)
            {
                SetupUI();
                isInitialized = true;
            }
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("UIManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupUI()
    {
        // Create a persistent UI parent object
        GameObject persistentUI = new GameObject("Persistent UI");
        DontDestroyOnLoad(persistentUI);
        
        // Create Canvas
        GameObject canvasObj = new GameObject("UI Canvas");
        uiCanvas = canvasObj.AddComponent<Canvas>();
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.transform.SetParent(persistentUI.transform);
        
        // Create GrabCrosshair
        grabCrosshair = new GameObject("GrabCrosshair");
        var image = grabCrosshair.AddComponent<UnityEngine.UI.Image>();
        
        // Load and apply the HandUI texture
        Sprite handSprite = Resources.Load<Sprite>("HandUI");
        if (handSprite != null)
        {
            image.sprite = handSprite;
            image.color = Color.white;
            // Set size based on the sprite's size
            image.rectTransform.sizeDelta = new Vector2(32, 32); // Adjust size as needed
            image.preserveAspect = true; // This will maintain the image's aspect ratio
        }
        else
        {
            Debug.LogError("HandUI sprite not found! Make sure it's in a Resources folder.");
            // Fallback to white square if texture not found
            image.color = Color.white;
            image.rectTransform.sizeDelta = new Vector2(32, 32);
        }
        
        // Setup crosshair transform
        grabCrosshair.transform.SetParent(canvasObj.transform, false);
        grabCrosshair.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        Debug.Log("UI Setup Complete");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: Checking UI elements");
        
        // If somehow our UI elements are lost, recreate them
        if (uiCanvas == null || grabCrosshair == null)
        {
            isInitialized = false;
            SetupUI();
        }
    }

    public void ToggleGrabCrosshair(bool show)
    {
        if (grabCrosshair == null)
        {
            Debug.LogWarning("GrabCrosshair missing - attempting to recreate UI");
            SetupUI();
        }

        if (grabCrosshair != null)
        {
            grabCrosshair.SetActive(show);
            Debug.Log($"GrabCrosshair visibility set to: {show}");
        }
        else
        {
            Debug.LogError("Failed to create or find GrabCrosshair!");
        }
    }
} 