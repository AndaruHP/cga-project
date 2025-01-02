using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;
    public MonoBehaviour firstPersonLookScript;
    public GameObject firstPersonAudio; // GameObject yang berisi semua audio pemain
    public static event System.Action<bool> PauseGameEvent;

    void Start()
    {
        Time.timeScale = 1f;
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        if (firstPersonLookScript != null)
        {
            firstPersonLookScript.enabled = true;
        }

        if (firstPersonAudio != null)
        {
            firstPersonAudio.SetActive(true); // Aktifkan kembali audio
        }

        Time.timeScale = 1f;
        paused = false;
        PauseGameEvent?.Invoke(paused);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);

        if (firstPersonLookScript != null)
        {
            firstPersonLookScript.enabled = false;
        }

        if (firstPersonAudio != null)
        {
            firstPersonAudio.SetActive(false); // Nonaktifkan seluruh audio
        }

        Time.timeScale = 0f;
        paused = true;
        PauseGameEvent?.Invoke(paused);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }
}
