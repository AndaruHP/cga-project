using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{   
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Play()
    {
        // Pastikan GameManager tidak null sebelum berpindah scene
        if (GameManager.Instance != null)
        {
            // Perbarui scene terakhir sebelum memuat scene berikutnya
            GameManager.Instance.UpdateLastScene();
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }

        // Muat scene berikutnya (Test 3) berdasarkan build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Retry()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadLastScene();
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }
    }

    public void Quit()
    {
        // Ini hanya berfungsi di versi build game
        Debug.Log("Player has quit the game");
        Application.Quit();
    }

    public void QuitBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}