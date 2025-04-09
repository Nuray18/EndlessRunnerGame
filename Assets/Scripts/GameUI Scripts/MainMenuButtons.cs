using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(1); // load the level scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
