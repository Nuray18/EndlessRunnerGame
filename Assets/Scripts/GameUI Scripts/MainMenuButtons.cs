using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1); // load the level scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
