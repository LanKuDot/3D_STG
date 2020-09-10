using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
    private readonly GameObject _targetObject;
    private readonly Queue<GameObject> _pool = new Queue<GameObject>();

    public ObjectPool(GameObject targetObject)
    {
        _targetObject = targetObject;
    }

    public GameObject GetObject()
    {
        var obj =
            _pool.Count == 0 ? GameObject.Instantiate(_targetObject) : _pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}
