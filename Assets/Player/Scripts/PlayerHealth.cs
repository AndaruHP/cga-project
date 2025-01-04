using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died.");

        // Pastikan GameManager tidak null
        if (GameManager.Instance != null)
        {
            // Perbarui scene terakhir sebelum memuat Game Over
            GameManager.Instance.UpdateLastScene();
            // Jangan ubah lastSceneIndex saat mati sehingga Retry akan kembali ke scene tempat mati
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }

        // Muat scene Game Over (Pastikan build index atau nama scene sesuai)
        SceneManager.LoadScene("Game Over"); // Atau gunakan build index: SceneManager.LoadScene(4);
    }


}
