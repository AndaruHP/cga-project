using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance;

    // Menyimpan build index dari scene terakhir yang dimainkan
    public int lastSceneIndex;

    void Awake()
    {
        // Implementasi Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            lastSceneIndex = 0; // Asumsikan Menu memiliki build index 0
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Memperbarui scene terakhir sebelum pindah ke scene baru
    public void UpdateLastScene()
    {
        lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log($"Last Scene Index updated to: {lastSceneIndex}");
    }

    // Memuat scene terakhir yang dimainkan
    public void LoadLastScene()
    {
        if (lastSceneIndex >= 0 && lastSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Loading last scene at index {lastSceneIndex}");
            SceneManager.LoadScene(lastSceneIndex);
        }
        else
        {
            Debug.LogError("Last scene index is out of range.");
        }
    }
}