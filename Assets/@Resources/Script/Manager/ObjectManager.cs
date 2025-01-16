using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager 
{
    public ManagedObject<AudioSource> Audios = new ManagedObject<AudioSource>();
    public ManagedObject<Tongue> Tongues = new ManagedObject<Tongue>();
}
public class ManagedObject<T> where T : UnityEngine.Component
{
    public HashSet<T> SpawnedObject = new HashSet<T>();
    public Queue<T> PoolingList = new Queue<T>();

    public virtual T Spawn(string key) 
    {
        //씬 변환이 있었는지 확인한다.
        if (PoolingList.Count != 0)
        {
            if (PoolingList.Peek() == null)
            {
                PoolingList.Clear();
                SpawnedObject.Clear();
            }

        }
        if (PoolingList.Count != 0)
        {
            T element = PoolingList.Dequeue();
            element.gameObject.SetActive(true);
            element.transform.position = Vector3.zero;
            SpawnedObject.Add(element);
            return element;
        }
        else
        {
            GameObject go = Manager.Instance.ResourceManager.Instantiate(key);
            T element = go.GetComponent<T>();
            SpawnedObject.Add(element);
            return element;
        }
    }
    public virtual void DeSpawn(T element)
    {
        SpawnedObject.Remove(element);
        PoolingList.Enqueue(element);
        element.transform.SetParent(null);
        element.transform.position = Vector3.zero;
        element.gameObject.SetActive(false);
    }
}