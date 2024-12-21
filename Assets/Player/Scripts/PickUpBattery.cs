using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBattery : MonoBehaviour
{
    public GameObject PickUpText;
    public GameObject HandUI;
    private bool inReach;
    public GameObject flashlight;

    void Start()
    {
        inReach = false;
        PickUpText.SetActive(false);
        HandUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = true;
            PickUpText.SetActive(true);
            HandUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = false;
            HandUI.SetActive(false);
            PickUpText.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact"))
        {
            // Memanggil fungsi RechargeBattery di skrip FlashlightTogglePlayer
            flashlight.GetComponent<FlashlightTogglePlayer>().RechargeBattery();
            PickUpText.SetActive(false);
            inReach = false;
            HandUI.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
