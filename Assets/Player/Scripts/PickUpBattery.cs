using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBattery : MonoBehaviour
{
    // public GameObject PickUpText;
    public GameObject grabCrosshair;
    private bool inReach;
    public GameObject flashlight;

    void Start()
    {
        inReach = false;
        // PickUpText.SetActive(false);
        grabCrosshair.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = true;
            // PickUpText.SetActive(true);
            grabCrosshair.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = false;
            grabCrosshair.SetActive(false);
            // PickUpText.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            // Memanggil fungsi RechargeBattery di skrip FlashlightTogglePlayer
            flashlight.GetComponent<FlashlightTogglePlayer>().RechargeBattery();
            // PickUpText.SetActive(false);
            inReach = false;
            grabCrosshair.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
