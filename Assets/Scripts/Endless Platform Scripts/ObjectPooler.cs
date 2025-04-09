using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; // Singleton

    public GameObject platformPrefab;
    public GameObject[] obstaclePrefabs;
    public GameObject[] collectablePrefabs;
    public int poolSize = 10;

    private Queue<GameObject> pool;
    private Dictionary<GameObject, Queue<GameObject>> obstaclePools;
    private Dictionary<GameObject, Queue<GameObject>> collectablePools;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); // Bu sahnede zaten bir instance varsa, yok et

        pool = new Queue<GameObject>();
        obstaclePools = new Dictionary<GameObject, Queue<GameObject>>();
        collectablePools = new Dictionary<GameObject, Queue<GameObject>>();

        // Platform prefab'larını havuza ekliyoruz
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);  // Başta pasif yapıyoruz
            pool.Enqueue(obj);     // Kuyruğa ekliyoruz
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

        // Eğer havuzda nesne varsa, sıradakini al
        if (pool.Count > 0)
        { 
            GameObject obj = pool.Dequeue(); // Kuyruktan al
            obj.SetActive(true);
            return obj;
        }

        // Eğer havuz boşsa, opsiyonel olarak yeni oluştur
        return Instantiate(platformPrefab);

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
