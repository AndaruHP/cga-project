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

    void Start()
    {
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        FirstPersonMovement movement = GetComponent<FirstPersonMovement>();
        if (movement != null && movement.IsRunning)
        {
            DrainEnergy();
        }
        else if (canRecharge)
        {
            RechargeEnergy();
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
