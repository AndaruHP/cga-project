using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;
    [SerializeField] private KeyCode reloadKey;

    private void Awake()
    {
        // Make the PlayerShoot persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!Pause.paused) // Only allow shooting when game is not paused
        {
            if (Input.GetMouseButton(0))
                shootInput?.Invoke();

            if (Input.GetKeyDown(reloadKey))
                reloadInput?.Invoke();
        }
    }

    private void OnDestroy()
    {
        // Clean up static events when destroyed
        shootInput = null;
        reloadInput = null;
    }
}
