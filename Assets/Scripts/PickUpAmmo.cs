using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmo : MonoBehaviour
{
    public GameObject PickUpText;
    public GameObject HandUI;
    private bool inReach;
    public int ammoAmount = 1; // Jumlah ammo yang didapat saat pickup
    private Gun gun;

    void Start()
    {
        inReach = false;
        PickUpText.SetActive(false);
        HandUI.SetActive(false);

        // Mengambil referensi ke objek Gun di scene
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
            PickUpText.SetActive(true);
            HandUI.SetActive(true);
            Debug.Log("Player is in range of ammo pickup.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reach"))
        {
            inReach = false;
            HandUI.SetActive(false);
            PickUpText.SetActive(false);
            Debug.Log("Player left the range of ammo pickup.");
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact"))
        {
            if (gun != null)
            {
                gun.AddAmmo(ammoAmount);
                Destroy(gameObject); // Menghapus objek ammo
                HandUI.SetActive(false);

                Debug.Log("Ammo picked up and ammo box destroyed.");
            }
            else
            {
                Debug.LogError("Gun object is not assigned!");
            }
        }
    }
}
