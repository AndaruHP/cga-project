using UnityEngine;
using UnityEngine.UI;

public class playerEnergyUI : MonoBehaviour
{
    public PlayerEnergy playerEnergy;
    public Slider energySlider;

    void Update()
    {
        if (playerEnergy != null && energySlider != null)
        {
            energySlider.value = playerEnergy.currentEnergy / playerEnergy.maxEnergy;
        }
    }
}