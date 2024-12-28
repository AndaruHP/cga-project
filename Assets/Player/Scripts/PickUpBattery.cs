using UnityEngine;

public class PickUpBattery : MonoBehaviour
{
    [SerializeField] private GameObject flashlight;
    private bool inReach;

    void Start()
    {
        inReach = false;
        if (UIManager.Instance != null && UIManager.Instance.grabCrosshair != null)
        {
            UIManager.Instance.grabCrosshair.SetActive(false);
        }

        if (flashlight == null)
        {
            flashlight = GameObject.Find("Flashlight Player");
            if (flashlight == null)
            {
                Debug.LogWarning("Flashlight Player was not assigned in inspector, attempting to find in scene.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = true;
            if (UIManager.Instance != null && UIManager.Instance.grabCrosshair != null)
            {
                UIManager.Instance.grabCrosshair.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = false;
            if (UIManager.Instance != null && UIManager.Instance.grabCrosshair != null)
            {
                UIManager.Instance.grabCrosshair.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            if (flashlight != null)
            {
                flashlight.GetComponent<FlashlightTogglePlayer>().RechargeBattery();
                if (UIManager.Instance != null && UIManager.Instance.grabCrosshair != null)
                {
                    UIManager.Instance.grabCrosshair.SetActive(false);
                }
                inReach = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
