using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashlightTogglePlayer : MonoBehaviour
{
    public GameObject lightGO; //light gameObject to work with
    public TextMeshProUGUI batteryText; //text to display battery level
    public float batteryLevel = 100; //battery level
    public float batteryDrainInterval = 1f; //battery drain rate (in seconds)
    private bool isOn = false; //is flashlight on or off?
    private float nextDrainTime = 0f; //next time to drain battery

    // Use this for initialization
    void Start()
    {
        //set default off
        lightGO.SetActive(false);
        UpdateBatteryText();
    }

    // Update is called once per frame
    void Update()
    {
        //toggle flashlight on key down
        if (Input.GetKeyDown(KeyCode.X))
        {
            // Check if battery is empty, if so, prevent turning on flashlight
            if (batteryLevel <= 0 && !isOn)
            {
                return;
            }

            // Toggle flashlight
            isOn = !isOn;
            lightGO.SetActive(isOn);
        }

        // If flashlight is on, reduce battery
        if (isOn && Time.time >= nextDrainTime)
        {
            // Drain battery once per interval
            batteryLevel -= 1;
            nextDrainTime = Time.time + batteryDrainInterval;

            if (batteryLevel <= 0)
            {
                batteryLevel = 0;
                lightGO.SetActive(false); // Turn off flashlight when battery is empty
                isOn = false;
                Debug.Log("Battery is empty. Flashlight turned off.");
            }

            // Update battery UI
            UpdateBatteryText();
        }
    }

    // Function to update battery UI text
    private void UpdateBatteryText()
    {
        if (batteryText != null)
        {
            batteryText.text = "Battery: " + Mathf.Clamp(batteryLevel, 0, 100).ToString("0") + "%";
        }
    }

    // Function to recharge battery
    public void RechargeBattery()
    {
        batteryLevel += 100;
        if (batteryLevel > 100)
        {
            batteryLevel = 100;
        }

        // Update UI with recharged battery
        UpdateBatteryText();
    }
}
