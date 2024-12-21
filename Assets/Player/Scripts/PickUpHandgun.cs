using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHandgun : MonoBehaviour
{
    public GameObject PickUpText;
    public GameObject HandgunPlayerDrop;
    public GameObject HandUI;
    private bool inReach;

    void Start()
    {
        PickUpText.SetActive(false);
        HandgunPlayerDrop.SetActive(false);
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
            HandUI.SetActive(false);
            HandgunPlayerDrop.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
