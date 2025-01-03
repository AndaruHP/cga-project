using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [Header("UI Components")]
    public Image fadeImage;
    public TextMeshProUGUI levelText;
    public GameObject transitionCanvas;

    [Header("Transition Settings")]
    public float fadeDuration = 1f;
    public float displayDuration = 2f;
    public float initialDelay = 0.5f;

    private void Start()
    {
        // Pastikan komponen yang diperlukan ada
        if (fadeImage == null || levelText == null || transitionCanvas == null)
        {
            Debug.LogError("Missing required components in LevelTransition!");
            return;
        }

        // Mulai transisi
        StartCoroutine(PerformTransition());
    }

    private IEnumerator PerformTransition()
    {
        // Aktifkan canvas dan atur kondisi awal
        transitionCanvas.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 1); // Layar hitam penuh
        levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, 0); // Text transparan
        levelText.enabled = true;

        // Tunggu sebentar sebelum memulai transisi
        yield return new WaitForSeconds(initialDelay);

        // Fade in dari hitam
        yield return StartCoroutine(Fade(1f, 0f));

        // Tampilkan teks level
        SetLevelText();
        yield return StartCoroutine(FadeText(0f, 1f));

        // Tahan teks selama beberapa detik
        yield return new WaitForSeconds(displayDuration);

        // Fade out teks
        yield return StartCoroutine(FadeText(1f, 0f));
        levelText.enabled = false;

        // Nonaktifkan canvas transisi
        transitionCanvas.SetActive(false);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / fadeDuration;
            
            // Gunakan smoothstep untuk transisi yang lebih halus
            float alpha = Mathf.Lerp(startAlpha, endAlpha, SmoothStep(normalizedTime));
            
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        // Pastikan nilai akhir tepat
        color.a = endAlpha;
        fadeImage.color = color;
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = levelText.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / fadeDuration;
            
            // Gunakan smoothstep untuk transisi yang lebih halus
            float alpha = Mathf.Lerp(startAlpha, endAlpha, SmoothStep(normalizedTime));
            
            color.a = alpha;
            levelText.color = color;
            yield return null;
        }

        // Pastikan nilai akhir tepat
        color.a = endAlpha;
        levelText.color = color;
    }

    private float SmoothStep(float x)
    {
        // Implementasi smoothstep untuk transisi yang lebih halus
        return x * x * (3 - 2 * x);
    }

    private void SetLevelText()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        string levelText = "";
        
        switch (currentScene)
        {
            case 1: // Test 3
                levelText = "Level 1";
                break;
            case 2: // Test 4 
                levelText = "Level 2";
                break;
            case 3: // Test 5
                levelText = "Level 3";
                break;
            default:
                levelText = "";
                break;
        }
        
        this.levelText.text = levelText;
    }
}