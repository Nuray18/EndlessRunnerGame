using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverPanel;

    // bunlar gameOver panelinde gozukenlerdir.
    public TMP_Text totalScoreText;
    public TMP_Text totalDistanceText;
    // bu main olanlar akranda gozukenlerdir.
    public TMP_Text mainDistanceText;
    public TMP_Text mainScoreText;

    public Button pauseButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //private GameOverPanel gameOverPanel;

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(1);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        pauseButton.gameObject.SetActive(false); // burda interactivini engelliyoruz
        mainDistanceText.enabled = false; // SetActive() funcu ile yapmagi denedim olmuyormus.
        mainScoreText.enabled = false;

        // get the score and distance
        int score = ScoreManager.instance.GetScore();
        float distance = DistanceTracker.instance.GetDistance();

        // display them on the screen
        totalScoreText.text = "Score: " + score; // so the text that we got from inspector we initialize it with the score value. so it's just text
        totalDistanceText.text = "Distance: " + distance.ToString("F1") + " m";

        PlayerMovement.Instance.Die();
        Time.timeScale = 0f;
    }
}
