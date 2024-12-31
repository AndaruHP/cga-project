using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKeyUI : MonoBehaviour
{
    public PlayerInventory inventory;
    public TextMeshProUGUI keyStatusText;

    void Update()
    {
        UpdateKeyStatus();
    }

    private void UpdateKeyStatus()
    {
        if (inventory != null && keyStatusText != null)
        {
            keyStatusText.text = "Key: " + (inventory.HasKey ? "True" : "False");
        }
    }
}
