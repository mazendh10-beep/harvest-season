using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        // For now, load your first gameplay scene
        SceneManager.LoadScene("Harvest_Season_0.2");
    }

    public void QuitGame()
    {
        Application.Quit();

        // This line is for testing in the Editor
        Debug.Log("Quit Game");
    }
    public void GoToInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
