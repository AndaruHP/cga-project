using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health;

    private float maxHealth;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private Renderer[] renderers;
    private Collider[] colliders;

    private bool isRespawning = false;

    // Reference to the Monster script
    private Monster monsterScript;

    private void Start()
    {
        maxHealth = health;
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        // Retrieve all Renderer and Collider components in children
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        // Get the Monster script component
        monsterScript = GetComponent<Monster>();
        if (monsterScript == null)
        {
            Debug.LogError("Monster script not found on the GameObject. Ensure the Monster script is attached.");
        }
    }

    public void TakeDamage(float damage)
    {
        if (isRespawning)
            return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth); // Clamp health between 0 and maxHealth

        if (health <= 0)
        {
            isRespawning = true;
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        // Disable the Monster script to stop chasing
        if (monsterScript != null)
            monsterScript.enabled = false;

        // Disable all Renderer and Collider components to "hide" the monster
        foreach (Renderer rend in renderers)
            rend.enabled = false;
        foreach (Collider col in colliders)
            col.enabled = false;

        // Optionally, trigger death animations or effects here
        // For example:
        // Animator animator = GetComponent<Animator>();
        // if (animator != null)
        //     animator.SetTrigger("Die");

        // Reset any ongoing camera shake
        if (CameraShake.Instance != null)
            CameraShake.Instance.StopShake();

        // Wait for 60 seconds before respawning
        yield return new WaitForSeconds(10f); // Set to 60f for a 1-minute delay

        // Reset health to maximum
        health = maxHealth;

        // Reset position and rotation if needed
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;

        // Re-enable all Renderer and Collider components to "respawn" the monster
        foreach (Renderer rend in renderers)
            rend.enabled = true;
        foreach (Collider col in colliders)
            col.enabled = true;

        // Re-enable the Monster script to allow chasing again
        if (monsterScript != null)
            monsterScript.enabled = true;

        // Optionally, reset animations or trigger respawn effects here
        // For example:
        // if (animator != null)
        //     animator.ResetTrigger("Die");

        isRespawning = false;
    }
}