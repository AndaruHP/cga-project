using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Singleton instance
    public static CameraShake Instance;

    // Default shake parameters
    public float defaultShakeDuration = 0.5f;
    public float defaultShakeMagnitude = 0.1f;
    public float dampingSpeed = 1.0f;

    private Vector3 initialPosition;
    private float currentShakeDuration = 0f;
    private float currentShakeMagnitude = 0f;

    private bool isPaused = false; // Track pause status

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Store the initial position of the camera
        initialPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        // Subscribe to pause event
        Pause.PauseGameEvent += OnPause; // Assuming PauseGameEvent is invoked in Pause script
    }

    private void OnDisable()
    {
        // Unsubscribe from pause event
        Pause.PauseGameEvent -= OnPause;
    }

    private void OnPause(bool isGamePaused)
    {
        isPaused = isGamePaused;

        if (isPaused)
        {
            StopShake(); // Stop shaking when paused
        }
    }

    /// <summary>
    /// Initiates the camera shake effect with specified magnitude and duration.
    /// </summary>
    /// <param name="magnitude">Intensity of the shake.</param>
    /// <param name="duration">Duration of the shake.</param>
    public void TriggerShake(float magnitude, float duration)
    {
        if (!isPaused) // Only trigger shake if not paused
        {
            currentShakeDuration = duration;
            currentShakeMagnitude = magnitude;
        }
    }

    /// <summary>
    /// Immediately stops any ongoing camera shake and resets the camera position.
    /// </summary>
    public void StopShake()
    {
        currentShakeDuration = 0f;
        currentShakeMagnitude = 0f;
        transform.localPosition = initialPosition;
    }

    private void Update()
    {
        if (currentShakeDuration > 0 && !isPaused)
        {
            // Apply random displacement within a sphere scaled by currentShakeMagnitude
            transform.localPosition = initialPosition + Random.insideUnitSphere * currentShakeMagnitude;

            // Decrease the shake duration over time
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else if (!isPaused)
        {
            // Reset to the initial position when shaking is done
            transform.localPosition = initialPosition;
        }
    }
}
