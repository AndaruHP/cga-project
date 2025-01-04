using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;

    public GameObject pauseMenuUI;

    // Referensi ke skrip FirstPersonLook yang akan dinonaktifkan saat pause
    public FirstPersonLook firstPersonLookScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;

        // Menyembunyikan kursor dan mengunci
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Mengaktifkan kembali skrip FirstPersonLook
        if (firstPersonLookScript != null)
        {
            firstPersonLookScript.enabled = true;
            Debug.Log("FirstPersonLook diaktifkan kembali.");
        }
        else
        {
            Debug.LogWarning("Referensi FirstPersonLook tidak diatur di Inspector!");
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;

        // Menampilkan kursor dan membuka kunci
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Menonaktifkan skrip FirstPersonLook
        if (firstPersonLookScript != null)
        {
            firstPersonLookScript.enabled = false;
            Debug.Log("FirstPersonLook dinonaktifkan.");
        }
        else
        {
            Debug.LogWarning("Referensi FirstPersonLook tidak diatur di Inspector!");
        }
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

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

        // Jika Anda sedang dalam editor, gunakan ini
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}