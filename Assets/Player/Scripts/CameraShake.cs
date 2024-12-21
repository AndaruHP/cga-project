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

    /// <summary>
    /// Initiates the camera shake effect with specified magnitude and duration.
    /// </summary>
    /// <param name="magnitude">Intensity of the shake.</param>
    /// <param name="duration">Duration of the shake.</param>
    public void TriggerShake(float magnitude, float duration)
    {
        currentShakeDuration = duration;
        currentShakeMagnitude = magnitude;
    }

    private void Update()
    {
        if (currentShakeDuration > 0)
        {
            // Apply random displacement within a sphere scaled by currentShakeMagnitude
            transform.localPosition = initialPosition + Random.insideUnitSphere * currentShakeMagnitude;

            // Decrease the shake duration over time
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // Reset to the initial position when shaking is done
            currentShakeDuration = 0f;
            currentShakeMagnitude = 0f;
            transform.localPosition = initialPosition;
        }
    }
}