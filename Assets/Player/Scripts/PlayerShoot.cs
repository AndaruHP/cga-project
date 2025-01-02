using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;
    [SerializeField] private KeyCode reloadKey;

    private void Update()
    {
        if (!Pause.paused) // Pastikan hanya bisa menembak saat game tidak dalam mode pause
        {
            if (Input.GetMouseButton(0))
                shootInput?.Invoke();

            if (Input.GetKeyDown(reloadKey))
                reloadInput?.Invoke();
        }
    }
}
