﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [SerializeField]
    private ObjectPoolItem[] _poolItems = new ObjectPoolItem[1];

    private readonly Dictionary<string, Queue<GameObject>> _pools =
        new Dictionary<string, Queue<GameObject>>();
    private readonly Dictionary<string, GameObject> _objectsToPool =
        new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;

        foreach (var item in _poolItems) {
            var queue = new Queue<GameObject>();
            _pools.Add(item.name, queue);

            for (var i = 0; i < item.initialNum; ++i) {
                var obj = Instantiate(item.objectToPool, transform);
                ReturnObject(item.name, obj);
                obj.SetActive(false);
            }

            _objectsToPool.Add(item.name, item.objectToPool);
        }
    }

    public GameObject GetObject(string objName)
    {
        var pool = _pools[objName];
        var obj = pool.Count == 0 ?
            Instantiate(_objectsToPool[objName]) : pool.Dequeue();
        return obj;
    }

    public void ReturnObject(string objName, GameObject obj)
    {
        _pools[objName].Enqueue(obj);
    }
}

[Serializable]
public class ObjectPoolItem
{
    public string name;
    public GameObject objectToPool;
    public int initialNum;
}
