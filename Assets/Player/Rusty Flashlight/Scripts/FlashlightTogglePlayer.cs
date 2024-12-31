using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightTogglePlayer : MonoBehaviour
{
    public GameObject lightGO;
    private bool isOn = false;
    public float batteryLife = 100f;
    public float batteryDrainRate = 2f;
    private float nextDrainTime = 0f;

    private Light flashlight;
    public float flickerThreshold = 15f;
    public float flickerInterval = 0.2f;
    private bool isFlickering = false;
    private Coroutine flickerCoroutine;

    void Start()
    {
        lightGO.SetActive(false);
        flashlight = lightGO.GetComponent<Light>();
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
                TurnOffFlashlight();
            }
        }

        if (isOn && batteryLife <= flickerThreshold)
        {
            if (!isFlickering)
            {
                flickerCoroutine = StartCoroutine(FlickerLight());
            }
        }
        else if (isFlickering)
        {
            StopFlickering();
        }
    }

    private void TurnOffFlashlight()
    {
        isOn = false;
        lightGO.SetActive(false);
        if (isFlickering)
        {
            StopFlickering();
        }
    }

    private void StopFlickering()
    {
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
        isFlickering = false;
        lightGO.SetActive(false);
    }

    IEnumerator FlickerLight()
    {
        isFlickering = true;
        while (batteryLife > 0 && isOn && batteryLife <= flickerThreshold)
        {
            lightGO.SetActive(!lightGO.activeSelf);
            yield return new WaitForSeconds(flickerInterval);
        }
        lightGO.SetActive(true);
        isFlickering = false;
    }

    public void RechargeBattery()
    {
        batteryLife = 100f;
        isOn = true;
        lightGO.SetActive(true);
        if (isFlickering)
        {
            StopFlickering();
        }
    }

    public float getBatteryLife()
    {
        return batteryLife;
    }
}
