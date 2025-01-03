using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public GameObject Camera4;
    public GameObject Camera5;
    public GameObject Camera6;

    void Start()
    {
        Camera1.SetActive(false);
        Camera2.SetActive(false);
        Camera3.SetActive(false);
        Camera4.SetActive(false);
        Camera5.SetActive(false);
        Camera6.SetActive(false);

        StartCoroutine(TheSequence());
    }


    IEnumerator TheSequence()
    {
        // yield return new WaitForSeconds(1);
        Camera1.SetActive(true);
        yield return new WaitForSeconds(4);
        Camera1.SetActive(false);
        Camera2.SetActive(true);
        yield return new WaitForSeconds(4);
        Camera2.SetActive(false);
        Camera3.SetActive(true);
        yield return new WaitForSeconds(4);
        Camera3.SetActive(false);
        Camera4.SetActive(true);
        yield return new WaitForSeconds(4);
        Camera4.SetActive(false);
        Camera5.SetActive(true);
        yield return new WaitForSeconds(4);
        Camera5.SetActive(false);
        Camera6.SetActive(true);
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("Menu");
    }

}
