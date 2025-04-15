using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; // Singleton

    private int platformIndex = 0;
    private int obstacleIndex = 0;
    private int collectableIndex = 0;

    public GameObject platformPrefab;
    public GameObject leftPlatformPrefab;
    public GameObject rightPlatformPrefab;

    public GameObject[] obstaclePrefabs;
    public GameObject[] collectablePrefabs;
    public int poolSize = 10;

    private Queue<GameObject> pool;
    private Queue<GameObject> leftPool;
    private Queue<GameObject> rightPool;

    private Dictionary<GameObject, Queue<GameObject>> obstaclePools;
    private Dictionary<GameObject, Queue<GameObject>> collectablePools;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Bu sahnede zaten bir instance varsa, yok et
        
        // Create ortadaki platform
        pool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        // Create left platform
        leftPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(leftPlatformPrefab);
            obj.SetActive(false);
            leftPool.Enqueue(obj);
        }

        // Create right platform
        rightPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(rightPlatformPrefab);
            obj.SetActive(false);
            rightPool.Enqueue(obj);
        }


        obstaclePools = new Dictionary<GameObject, Queue<GameObject>>();
        collectablePools = new Dictionary<GameObject, Queue<GameObject>>();
        
        // Obstacle havuzlarını hazırla
        foreach (var prefab in obstaclePrefabs)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            obstaclePools[prefab] = queue;
        }

        // Collectable havuzlarını hazırla
        foreach (var prefab in collectablePrefabs)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            collectablePools[prefab] = queue;
        }
    }

    public GameObject GetPooledObject()
    {
        if (pool == null)
        {
            Debug.LogError("Object Pool is not initialized!");
            return null;  // Pool boş olduğu için null döndürüyoruz
        }
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // Eğer havuz boşsa yeni oluşturmak opsiyonel:
        return Instantiate(platformPrefab);
    }

    public GameObject GetPooledLeftObject()
    {
        if (leftPool.Count > 0)
        {
            GameObject obj = leftPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(leftPlatformPrefab);
    }

    public GameObject GetPooledRightObject()
    {
        if (rightPool.Count > 0)
        {
            GameObject obj = rightPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(rightPlatformPrefab);
    }


    public GameObject GetPooledObstacle(GameObject prefab)
    {
        if (!obstaclePools.ContainsKey(prefab))
        {
            Debug.LogError("Obstacle pool not initialized for this prefab!");
            return null;
        }

        if (obstaclePools[prefab].Count > 0)
        {
            GameObject obj = obstaclePools[prefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // Eğer havuz boşsa, opsiyonel olarak yeni oluştur
        return Instantiate(prefab);
    }

    public GameObject GetPooledCollectable(GameObject prefab)
    {
        if (!collectablePools.ContainsKey(prefab))
        {
            Debug.LogError("Collectable pool not initialized for this prefab!");
            return null;
        }

        if (collectablePools[prefab].Count > 0)
        {
            GameObject obj = collectablePools[prefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // Eğer havuz boşsa, opsiyonel olarak yeni oluştur
        return Instantiate(prefab);
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj); // Tekrar kuyruğa ekle
    }

    public void ReturnLeftPool(GameObject obj)
    {
        obj.SetActive(false);
        leftPool.Enqueue(obj);
    }

    public void ReturnRightPool(GameObject obj)
    {
        obj.SetActive(false);
        rightPool.Enqueue(obj);
    }


    public void ReturnObstacle(GameObject obj, GameObject prefab)
    {
        obj.SetActive(false);
        if (!obstaclePools.ContainsKey(prefab))
            obstaclePools[prefab] = new Queue<GameObject>();

        obstaclePools[prefab].Enqueue(obj);
    }

    public void ReturnCollectable(GameObject obj, GameObject prefab)
    {
        obj.SetActive(false);
        if (!collectablePools.ContainsKey(prefab))
            collectablePools[prefab] = new Queue<GameObject>();

        collectablePools[prefab].Enqueue(obj);
    }
}
