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
        // we set + 1 because in our setting we have two scenes, the first one is the menu and the second one is the game
        // and its order is 0 then 1
        SceneManager.LoadScene("Test 3");
    }

    public void Retry()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Quit()
    {
        // this only work in the build version of the game
        Debug.Log("Player has quit the game");
        Application.Quit();
    }
}
