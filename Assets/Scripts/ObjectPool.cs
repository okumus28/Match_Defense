using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    private readonly Queue<T> pool;
    private readonly T prefab;
    private readonly Transform parentTransform;

    public ObjectPool(T prefab, int initialCapacity = 10, Transform parentTransform = null)
    {
        this.prefab = prefab;
        this.parentTransform = parentTransform;

        pool = new();
        for (int i = 0; i < initialCapacity; i++)
        {
            CreateNewObject();
        }
    }

    public T GetObject()
    {
        T obj;

        if (pool.Count <= 0)
        {
            CreateNewObject();
            obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
        }

        return obj;
    }
    
    public T GetObject(Vector3 newLocalPosition)
    {
        T obj;


        if (pool.Count <= 0)
        {
            CreateNewObject();

            obj = pool.Dequeue();
            obj.transform.localPosition = newLocalPosition;
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = pool.Dequeue();
            obj.transform.localPosition = newLocalPosition;
            obj.gameObject.SetActive(true);
        }

        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    private T CreateNewObject()
    {
        T obj = Object.Instantiate(prefab, parentTransform);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public void ReturnObject(T obj , float time)
    {
        StartCoroutine(ReturnObjectTimeCalc(obj, time));
    }

    private IEnumerator ReturnObjectTimeCalc(T obj , float time)
    {
        yield return new WaitForSecondsRealtime(time);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        //ReturnObject(obj);
    }
}