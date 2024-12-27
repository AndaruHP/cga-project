using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashlightUI : MonoBehaviour
{
    public FlashlightTogglePlayer flashlightTogglePlayer;
    public TextMeshProUGUI batteryText;

    void Update()
    {
        UpdateBatteryText();
    }

    private void UpdateBatteryText()
    {
        if (flashlightTogglePlayer != null && batteryText != null)
        {
            float batteryLife = flashlightTogglePlayer.getBatteryLife();
            batteryText.text = "Battery: " + batteryLife.ToString("F0") + "%";
        }
    }
}
