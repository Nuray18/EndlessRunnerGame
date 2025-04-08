using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public ObjectPooler objectPooler;
    public int initialPlatformCount = 10;
    public float platformLength = 10f;

    private float spawnZ = 0f;
    private Transform playerTransform;

    void Start()
    {
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler is not assigned in PlatformSpawner!");
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (PlayerMovement.Instance == null)
            return;

        if (playerTransform.position.z + (platformLength * 5) > spawnZ)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        GameObject platform = objectPooler.GetPooledObject();
        platform.transform.position = new Vector3(0, 0, spawnZ);
        spawnZ += platformLength;
    }
}
