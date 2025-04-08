using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtons : MonoBehaviour
{
    public GameObject pausePanel;

    public void RestartGameFromBeginning()
    {
        GameManager.instance.RestartGame();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true); // Pause panelini aktif et
        Time.timeScale = 0f; // Oyun zamanını duraklat
    }

    public void BackToGame()
    {
        pausePanel.SetActive(false); // Pause panelini gizle
        Time.timeScale = 1f; // Oyun zamanını eski haline getir
    }

    public void LeaveToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
