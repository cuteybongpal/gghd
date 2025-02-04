using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager 
{
    public ManagedObject<AudioSource> Audios = new ManagedObject<AudioSource>();
    public ManagedObject<Tongue> Tongues = new ManagedObject<Tongue>();
    public ManagedObject<Bullet> Bullets = new ManagedObject<Bullet>();
    public ManagePlayer Player = new ManagePlayer();
    public ManagedUI UIs= new ManagedUI();

    public ManagedObject<Slime> Slimes = new ManagedObject<Slime>();
    public ManagedObject<Frog> Frogs = new ManagedObject<Frog>();
    public ManagedObject<Gun> Guns = new ManagedObject<Gun>();
}
public class ManagedObject<T> where T : UnityEngine.Component
{
    HashSet<T> SpawnedObject = new HashSet<T>();
    Queue<T> PoolingList = new Queue<T>();

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
public class ManagePlayer
{
    public PlayerController Player;

    public PlayerController Spawn(string key)
    {
        GameObject go = Manager.Instance.ResourceManager.Instantiate(key);
        Player = go.GetComponent<PlayerController>();
        return Player;
    }

    public void DeSpawn(PlayerController player)
    {
        GameObject.Destroy(player.gameObject);
    }
}
public class ManagedUI
{
    HashSet<UI_Base> UIList = new HashSet<UI_Base>();
    List<UI_Base> UIPooling = new List<UI_Base>();


    public virtual T Spawn<T>(string key) where T : UI_Base
    {

        bool isNotTypeMatched = true;
        //씬 변환이 있었는지 확인한다.
        if (UIPooling.Count != 0)
        {
            if (UIPooling[0] == null)
            {
                UIList.Clear();
                UIPooling.Clear();
            }

        }
        if (UIPooling.Count != 0)
        {
            for (int i = 0; i < UIPooling.Count; i++)
            {
                if (typeof(T) == UIPooling[i].GetType())
                {
                    T ui = UIPooling[i] as T;
                    ui.gameObject.SetActive(true);
                    UIList.Add(ui);
                    UIPooling.Remove(UIPooling[i]);
                    return ui;
                }
                else
                    isNotTypeMatched &= true;
            }
        }
        if (isNotTypeMatched)
        {
            GameObject go = Manager.Instance.ResourceManager.Instantiate(key);
            T element = go.GetComponent<T>();
            UIList.Add(element);
            return element;
        }
        return default(T);
    }
    public virtual void DeSpawn<T>(T element) where T : UI_Base
    {
        UIList.Remove(element);
        UIPooling.Add(element);
        element.transform.SetParent(null);
        element.transform.position = Vector3.zero;
        element.gameObject.SetActive(false);
    }
}