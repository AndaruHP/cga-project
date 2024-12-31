using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public float maxEnergy = 100f;
    public float currentEnergy;
    public float energyDrainRate = 20f;
    public float energyRegenRate = 3f;
    public bool canRecharge = true;
    private FirstPersonMovement movement;

    void Start()
    {
        currentEnergy = maxEnergy;
        movement = GetComponent<FirstPersonMovement>();
    }

    void Update()
    {
        if (movement != null)
        {
            if (movement.IsRunning && currentEnergy > 0)
            {
                DrainEnergy();
            }
            else if (canRecharge && currentEnergy < maxEnergy)
            {
                RechargeEnergy();
            }

            movement.canRun = currentEnergy > 0;
        }
    }

    void DrainEnergy()
    {
        currentEnergy -= energyDrainRate * Time.deltaTime;
        if (currentEnergy <= 0)
        {
            currentEnergy = 0;
        }
    }

    void RechargeEnergy()
    {
        currentEnergy += energyRegenRate * Time.deltaTime;
    }
}
