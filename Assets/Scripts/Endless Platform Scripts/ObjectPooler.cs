using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; // Singleton

    private int platformIndex = 0;
    private int obstacleIndex = 0;
    private int collectableIndex = 0;

    public GameObject[] platformPrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject[] collectablePrefabs;
    public int poolSize = 10;

    private Queue<GameObject> pool;
    private Dictionary<GameObject, Queue<GameObject>> obstaclePools;
    private Dictionary<GameObject, Queue<GameObject>> collectablePools;
    private Dictionary<GameObject, Queue<GameObject>> platformPools;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Bu sahnede zaten bir instance varsa, yok et

        pool = new Queue<GameObject>();
        obstaclePools = new Dictionary<GameObject, Queue<GameObject>>();
        collectablePools = new Dictionary<GameObject, Queue<GameObject>>();
        platformPools = new Dictionary<GameObject, Queue<GameObject>>();

        foreach (var prefab in platformPrefabs)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            platformPools[prefab] = queue;
        }

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

        if (platformPrefabs.Length == 0)
            return null;

        GameObject selectedPrefab = platformPrefabs[platformIndex];
        platformIndex = (platformIndex + 1) % platformPrefabs.Length;

        if (platformPools.ContainsKey(selectedPrefab) && platformPools[selectedPrefab].Count > 0)
        {
            GameObject obj = platformPools[selectedPrefab].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        // Eğer havuz boşsa yeni oluşturmak opsiyonel:
        return Instantiate(selectedPrefab);
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

        foreach (var kvp in platformPools)
        {
            // Prefab'larla eşleşen objeyi doğru havuza geri gönder
            if (obj.name.Contains(kvp.Key.name))
            {
                platformPools[kvp.Key].Enqueue(obj);
                return;
            }
        }

        pool.Enqueue(obj); // Tekrar kuyruğa ekle
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
