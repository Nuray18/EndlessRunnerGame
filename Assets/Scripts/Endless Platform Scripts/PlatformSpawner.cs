using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    private Transform playerTransform;
    // script
    public ObjectPooler objectPooler;

    // pattern for center platform
    public GameObject[] obstaclePatterns;
    public GameObject[] collectablePatterns;

    public int initialPlatformCount = 10;

    // Z axis
    private float spawnZCenter = 0.0f;
    private float spawnZLeft = 0.0f;
    private float spawnZRight = 0.0f;

    public float centerPlatformLength = 50f;
    public float sidePlatformLength = 50f;
    
    private float spawnX = 0f;
    private float sideXPosition = 104.5f; // this is for the left and the right platform X positions



    void Start()
    {
        if (objectPooler == null)
        {
            Debug.LogError("ObjectPooler is not assigned in PlatformSpawner!");
        }

        if (obstaclePatterns.Length > 0)
        {
            centerPlatformLength = objectPooler.GetPooledObject().transform.localScale.z * 10f;
        }

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform(spawnZCenter);
            spawnZCenter += centerPlatformLength;

            SpawnLeftPlatform(spawnZLeft);
            spawnZLeft += sidePlatformLength;

            SpawnRightPlatform(spawnZRight);
            spawnZRight += sidePlatformLength;
        }
    }

    void Update()
    {
        if (Player.Instance == null)
            return;

        if (playerTransform.position.z + (centerPlatformLength * 5) > spawnZCenter)
        {
            SpawnPlatform(spawnZCenter);
            spawnZCenter += centerPlatformLength;
        }

        if (playerTransform.position.z + (sidePlatformLength * 5) > spawnZLeft)
        {
            SpawnLeftPlatform(spawnZLeft);
            spawnZLeft += sidePlatformLength;
        }

        if (playerTransform.position.z + (sidePlatformLength * 5) > spawnZRight)
        {
            SpawnRightPlatform(spawnZRight);
            spawnZRight += sidePlatformLength;
        }

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
