using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int collectedScore = 0;

    private void Awake()
    {
        // Singleton yapısı: Her yerden ulaşılabilir
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // bunu scoreye degdigimizde cagiracagiz.
    public void AddScore(int amount)
    {
        collectedScore += amount;
    }

    public int GetScore()
    {
        return collectedScore;
    }
}
