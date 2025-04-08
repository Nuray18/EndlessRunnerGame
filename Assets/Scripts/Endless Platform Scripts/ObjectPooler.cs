using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance; // Singleton

    public GameObject platformPrefab;
    public int poolSize = 10;

    private Queue<GameObject> pool;

    void Awake()
    {
        Instance = this;  // Singleton'ı doğru başlatıyoruz
        pool = new Queue<GameObject>();

        // Platform prefab'larını havuza ekliyoruz
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(platformPrefab);
            obj.SetActive(false);  // Başta pasif yapıyoruz
            pool.Enqueue(obj);     // Kuyruğa ekliyoruz
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
        GameObject newObj = Instantiate(platformPrefab);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj); // Tekrar kuyruğa ekle
    }
}
