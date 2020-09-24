using UnityEngine;
using System;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [SerializeField]
    private ObjectPoolItem[] _poolItems = new ObjectPoolItem[1];

    // The name of the pool object will be the key of these dictionaries
    private readonly Dictionary<string, Queue<GameObject>> _pools =
        new Dictionary<string, Queue<GameObject>>();
    private readonly Dictionary<string, GameObject> _objectsToPool =
        new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;

        foreach (var item in _poolItems) {
            var queue = new Queue<GameObject>();
            var originalObj = item.objectToPool;
            var objName = originalObj.name;

            _pools.Add(objName, queue);

            for (var i = 0; i < item.initialNum; ++i) {
                var obj = Instantiate(originalObj, transform);
                obj.name = objName;
                ReturnObject(obj);
                obj.SetActive(false);
            }

            _objectsToPool.Add(objName, originalObj);
        }
    }

    public GameObject GetObject(string objName)
    {
        var pool = _pools[objName];
        var originalObj = _objectsToPool[objName];
        var obj = pool.Count == 0 ?
            Instantiate(originalObj) : pool.Dequeue();
        obj.name = originalObj.name;
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        _pools[obj.name].Enqueue(obj);
    }
}

[Serializable]
public class ObjectPoolItem
{
    [ShowOnly]
    public string name;
    public GameObject objectToPool;
    public int initialNum;
}
