using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 100f;
    private Antagonis antagonis;

    private void Awake()
    {
        antagonis = GetComponent<Antagonis>();
        if (antagonis == null)
        {
            Debug.LogError("Antagonis component not found on the GameObject.");
        }
    }

    public void TakeDamage(float damage)
    {
        if (antagonis == null)
        {
            Debug.LogWarning("Cannot take damage because Antagonis component is missing.");
            return;
        }

        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {health}");

        if (health <= 0)
        {
            health = 0;
            antagonis.OnDeath(); // Memicu metode OnDeath pada Antagonis.cs
        }
    }

    public void ResetHealth()
    {
        health = 100f;
    }
}