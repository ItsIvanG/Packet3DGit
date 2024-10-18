using UnityEngine.SceneManagement;
using UnityEngine;

public class UIFadeInAndLoadScene : MonoBehaviour
{
    public CanvasGroup uiPanel;        // Reference to the CanvasGroup for fading
    public float fadeDuration = 2f;    // Duration of the fade-in effect
    public string sceneToLoad = "SceneName";  // The name of the scene to load once fade is done

    private float currentAlpha = 0f;   // Current alpha value of the UI panel
    private bool isFading = false;      // Flag to track if fade-in is happening

    void Start()
    {
        // Initialize the UI panel with zero alpha (completely invisible)
        uiPanel.alpha = 0f;
    }

    void Update()
    {
        // Check if the panel is still fading
        if (isFading)
        {
            // Gradually increase the alpha value
            currentAlpha += Time.deltaTime / fadeDuration;
            uiPanel.alpha = Mathf.Clamp01(currentAlpha);  // Ensure alpha stays between 0 and 1

            // Once the fade-in is complete, load the new scene
            if (currentAlpha >= 1f)
            {
                isFading = false;  // Stop fading
                LoadNextScene();   // Call the method to load the scene
            }
        }
    }

    // Method to load the next scene
    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    public void StartFade(string scene)
    {
        isFading = true;
        sceneToLoad = scene;
    }
    public void StartFade()
    {
        isFading = true;
    }
}