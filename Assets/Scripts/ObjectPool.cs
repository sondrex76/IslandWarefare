using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        
        public string key;
        public GameObject prefab;
        public int capacity;
    }

    [SerializeField]
    public List<Pool> _pools;
    public IDictionary<string, Queue<GameObject>> _poolDictionary;
    // Start is called before the first frame update
    void Awake()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool p in _pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < p.capacity; i++)
            {
                GameObject obj = Instantiate(p.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            _poolDictionary.Add(p.prefab.name, objectPool);  
        }
    }

    public GameObject GetPooledObject(string key) 
    {
        GameObject obj = _poolDictionary[key].Dequeue();
        if (!obj.activeInHierarchy)
        {
            return obj;
        }
        _poolDictionary[key].Enqueue(obj);
        return null;
    }

    public void AddObjcetToPool(string key, GameObject obj)
    {
        obj.SetActive(false);
        _poolDictionary[key].Enqueue(obj);
    }
}
