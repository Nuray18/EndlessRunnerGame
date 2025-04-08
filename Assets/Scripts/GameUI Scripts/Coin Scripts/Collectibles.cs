using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public int scoreValue = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddScore(scoreValue); 
            Destroy(gameObject);
        }
    }
}

