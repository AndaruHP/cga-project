using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlashlightTogglePlayer : MonoBehaviour
{
    public GameObject lightGO;
    private bool isOn = false;
    public float batteryLife = 100f;
    public float batteryDrainRate = 2f;
    private float nextDrainTime = 0f;

    void Start()
    {
        lightGO.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (batteryLife > 0)
            {
                isOn = !isOn;
                lightGO.SetActive(isOn);
            }
        }

        if (isOn && Time.time >= nextDrainTime)
        {
            batteryLife -= batteryDrainRate;
            nextDrainTime = Time.time + 1f;

            if (batteryLife <= 0)
            {
                batteryLife = 0;
                lightGO.SetActive(false);
                isOn = false;
            }
        }
    }

    public void RechargeBattery()
    {
        batteryLife = 100f;
    }

    public float getBatteryLife()
    {
        return batteryLife;
    }
}
