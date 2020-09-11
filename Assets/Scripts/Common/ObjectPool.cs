using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public GameObject targetObject;

    private readonly Queue<GameObject> _pool = new Queue<GameObject>();

    public GameObject GetObject()
    {
        var obj =
            _pool.Count == 0 ? Instantiate(targetObject) : _pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        _pool.Enqueue(obj);
    }
}
