using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFlashLight : MonoBehaviour
{
    public GameObject PickUpText;
    public GameObject FlashLightPlayerDrop;

    void Start()
    {
        PickUpText.SetActive(false);
        FlashLightPlayerDrop.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickUpText.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);
                FlashLightPlayerDrop.SetActive(true);
                PickUpText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickUpText.SetActive(false);
    }
}
