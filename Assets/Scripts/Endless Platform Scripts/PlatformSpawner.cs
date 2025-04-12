using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    // script
    public ObjectPooler objectPooler;

    public int initialPlatformCount = 10;
    public float platformLength;

    // pattern for center platform
    public GameObject[] obstaclePatterns;
    public GameObject[] collectablePatterns;

    private float spawnZ = 0f;
    private float spawnX = 0f;

    private Transform playerTransform;

    void Start()
    {
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler is not assigned in PlatformSpawner!");
        }

        if (obstaclePatterns.Length > 0)
        {
            // Platform uzunluğunu prefab scale’ine göre ayarla (varsayılan Unity plane = 10 birim)
            platformLength = objectPooler.GetPooledObject().transform.localScale.z * 10f;
        }


        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (Player.Instance == null)
            return;

        // player'e gore platform spawn et
        if (playerTransform.position.z + (platformLength * 5) > spawnZ)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        GameObject platform = objectPooler.GetPooledObject();
        if (platform == null) return;

        foreach (Transform child in platform.transform)
        {
            if (child.name == "ObstacleContainer" || child.name == "CollectableContainer")
            {
                Destroy(child.gameObject);
            }
        }

        if(platform.name.Contains("RightPlatform"))
        {
            spawnX = 54.5f;
        }
        else if(platform.name.Contains("LeftPlatform"))
        {
            spawnX = -54.5f;
        }

        platform.transform.position = new Vector3(spawnX, 0, spawnZ);
        platform.transform.rotation = Quaternion.identity;
        platform.SetActive(true);

        // Obstacle pattern ekle
        if (obstaclePatterns.Length > 0)
        {
            int rand = Random.Range(0, obstaclePatterns.Length);
            GameObject prefab = obstaclePatterns[rand];

            GameObject container = new GameObject("ObstacleContainer");
            container.transform.SetParent(platform.transform);
            container.transform.localPosition = Vector3.zero;

            GameObject pattern = objectPooler.GetPooledObstacle(prefab);
            pattern.transform.SetParent(container.transform);
            pattern.transform.localPosition = Vector3.zero;
            pattern.SetActive(true);
        }

        // Collectable pattern ekle
        if (collectablePatterns.Length > 0)
        {
            int rand = Random.Range(0, collectablePatterns.Length);
            GameObject prefab = collectablePatterns[rand];

            GameObject container = new GameObject("CollectableContainer");
            container.transform.SetParent(platform.transform);
            container.transform.localPosition = Vector3.zero;

            GameObject pattern = objectPooler.GetPooledCollectable(prefab);
            pattern.transform.SetParent(container.transform);
            pattern.transform.localPosition = Vector3.zero;
            pattern.SetActive(true);
        }

        spawnZ += platformLength;
    }

}
