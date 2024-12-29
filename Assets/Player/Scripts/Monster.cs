using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 10f; // Radius to detect the player
    public float maxShakeDuration = 0.5f; // Maximum duration of the shake
    public float maxShakeMagnitude = 0.3f; // Maximum intensity of the shake

    private Transform playerTransform;

    private void Start()
    {
        // Find the player by tag. Ensure the player GameObject is tagged as "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogError("Player object not found! Ensure the player is tagged as 'Player'.");
    }

    private void Update()
    {
        if (playerTransform == null)
            return;

        // Calculate distance between monster and player
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= detectionRadius)
        {
            // Calculate shake intensity based on distance
            float normalizedDistance = Mathf.Clamp01(distance / detectionRadius); // 0 (close) to 1 (far)
            float shakeIntensity = (1 - normalizedDistance) * maxShakeMagnitude;
            float shakeTime = (1 - normalizedDistance) * maxShakeDuration;

            // Trigger camera shake with calculated intensity and duration
            CameraShake.Instance.TriggerShake(shakeIntensity, shakeTime);
        }
    }
}