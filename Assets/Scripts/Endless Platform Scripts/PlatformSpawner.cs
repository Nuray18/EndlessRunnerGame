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

    
    private float sideXPosition = 104.5f; // this is for the left and the right platform X positions

    private Transform playerTransform;

    void Start()
    {
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler is not assigned in PlatformSpawner!");
        }

        if (obstaclePatterns.Length > 0)
        {
            platformLength = objectPooler.GetPooledObject().transform.localScale.z * 10f;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnAllPlatforms();
        }
    }

    void Update()
    {
        if (Player.Instance == null)
            return;

        // player'e gore platform spawn et
        if (playerTransform.position.z + (platformLength * 5) > spawnZ)
        {
            SpawnAllPlatforms();
        }
    }

    void SpawnAllPlatforms()
    {
        float currentZ = spawnZ;

        SpawnPlatform(currentZ);
        SpawnLeftPlatform(currentZ);
        SpawnRightPlatform(currentZ);

        spawnZ += platformLength;
    }

    void SpawnPlatform(float z)
    {
        GameObject platform = objectPooler.GetPooledObject(); // object poolere verdigimiz platformlari aliyoruz
        if (platform == null) return;

        var disable = platform.GetComponent<DisablePlatform>(); // platform dan DisablePlatform scriptini aliyoruz gameObjectin ustunde aldigimiz icin problem yok. Arama yapmiyoruz.
        if (disable != null)
            disable.platformType = DisablePlatform.PlatformType.Center;

        foreach (Transform child in platform.transform)
        {
            if (child.name == "ObstacleContainer")
            {
                foreach (Transform obj in child)
                {
                    objectPooler.ReturnObstacle(obj.gameObject, obj.gameObject); // prefab referansı objenin kendisi
                }
                Destroy(child.gameObject);
            }
            else if (child.name == "CollectableContainer")
            {
                foreach (Transform obj in child)
                {
                    objectPooler.ReturnCollectable(obj.gameObject, obj.gameObject); // prefab referansı objenin kendisi
                }
                Destroy(child.gameObject);
            }
        }

        platform.transform.position = new Vector3(spawnX, 0, z);
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
    }


    void SpawnLeftPlatform(float z)
    {
        GameObject platform = objectPooler.GetPooledLeftObject();
        if (platform == null) return;

        var disable = platform.GetComponent<DisablePlatform>();
        if (disable != null)
            disable.platformType = DisablePlatform.PlatformType.Left;

        platform.transform.position = new Vector3(spawnX - sideXPosition, 0, z); // sola kaydır
        platform.transform.rotation = Quaternion.identity;
        platform.SetActive(true);
    }

    // positive
    void SpawnRightPlatform(float z)
    {
        GameObject platform = objectPooler.GetPooledRightObject();
        if (platform == null) return;

        var disable = platform.GetComponent<DisablePlatform>();
        if (disable != null)
            disable.platformType = DisablePlatform.PlatformType.Right;

        platform.transform.position = new Vector3(spawnX + sideXPosition, 0, z); // sağa kaydır
        platform.transform.rotation = Quaternion.identity;
        platform.SetActive(true);

    }


}
