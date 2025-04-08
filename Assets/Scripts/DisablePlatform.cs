using UnityEngine;

public class DisablePlatform : MonoBehaviour
{
    private Transform player;
    public float disableDistance = 30f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player.position.z - transform.position.z > disableDistance)
        {
            Debug.Log("Platform disabled and returned to pool: " + gameObject.name);
            ObjectPooler.Instance.ReturnToPool(gameObject); // Kuyruğa geri gönder
        }
    }
}
