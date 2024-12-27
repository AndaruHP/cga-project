using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public Gun gun;
    public TextMeshProUGUI ammoText;

    void Update()
    {
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (gun != null && ammoText != null)
        {
            int currentAmmo = gun.GetCurrentAmmo();
            ammoText.text = "Ammo: " + currentAmmo.ToString();
        }
    }
}
