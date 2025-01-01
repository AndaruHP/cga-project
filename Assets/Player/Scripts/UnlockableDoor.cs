using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockableDoor : MonoBehaviour
{
    public GameObject grabCrosshair;
    private bool inReach;
    public PlayerInventory inventory; 
    private bool isUnlocked = false;
    private Animator animator; 
    
    void Start()
    {
        inReach = false;
        grabCrosshair.SetActive(false);
        animator = GetComponent<Animator>();

        // Cari skrip PlayerInventory di scene
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("PlayerInventory not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = true;
            grabCrosshair.SetActive(true);
            Debug.Log("Player is in reach of the door.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Reach"))
        {
            inReach = false;
            grabCrosshair.SetActive(false);
            Debug.Log("Player left the reach of the door.");
        }
    }

    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null && inventory.HasKey)
            {
                UnlockDoor();
            }
            else
            {
                Debug.Log("You need a key to unlock this door.");
            }
        }
    }

    void UnlockDoor()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            Debug.Log("Door Unlocked!");
            if (animator != null)
            {
                animator.SetTrigger("Unlock"); 
                Debug.Log("Animator Trigger 'Unlock' set.");
            }
           
            // Nonaktifkan collider untuk memungkinkan passage
            Collider doorCollider = GetComponent<Collider>();
            if (doorCollider != null)
            {
                doorCollider.enabled = false;
                Debug.Log("Door collider disabled.");
            }
            else
            {
                Debug.LogWarning("No Collider component found on the door.");
            }

            // Perbarui scene terakhir di GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UpdateLastScene();
            }
            else
            {
                Debug.LogError("GameManager instance not found.");
            }

            // Mulai coroutine untuk memuat scene berikutnya setelah 5 detik
            StartCoroutine(LoadNextSceneAfterDelay(5f));
        }
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        Debug.Log($"Starting coroutine to load next scene after {delay} seconds.");
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log($"Current Scene Index: {currentSceneIndex}");

        // Cek apakah indeks berikutnya valid
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Loading next scene at index {currentSceneIndex + 1}.");
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.LogError("Next scene index is out of range. Please check Build Settings.");
        }
    }
}