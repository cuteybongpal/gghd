using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    #region �̱��� �ڵ�
    private static CommandManager instance = null;
    public static CommandManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            Application.targetFrameRate = 100;
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            Manager.Instance.ResourceManager.LoadAllAsync<TextAsset>("PreLoad(Data)", () =>
            {
                Manager.Instance.DataManager.Init();
                Manager.Instance.ResourceManager.LoadAllAsync<GameObject>("PreLoad(Prefab)", () =>
                {
                    EventManager.AssetAllLoading?.Invoke();
                });
            });

        }
        else
        {
            Destroy(instance);
        }
    }
    #endregion
    
    //ICommand �������̽��� ��ӹ޴� Ŭ������ �Ķ���ͷ� �־��ְ�, ICommand�� Execute�Լ��� ��������ش�.
    public T ExecuteCommand<T>(ICommand command) where T : UnityEngine.Object
    {
        
        if (command == null)
        {
            Debug.LogError("Invalid command");
            return default;
        }
        return command.Execute<T>();
    }
}

public class Load : ICommand
{
    //ResourceManager���� ������Ʈ�� �������� �޼ҵ�
    string key;
    public T Execute<T>() where T : UnityEngine.Object
    {
        T loadedobject = Manager.Instance.ResourceManager.Load<T>(key);
        return loadedobject as T;
    }
    public Load(string _key)
    {
        key = _key;
    }
}
public class Spawn : ICommand
{
    //ObjectManager���� �������ִ� �ڵ� 
    string key;
    public T Execute<T>() where T : UnityEngine.Object
    {
        //todo : ������Ʈ �Ŵ��� �����, �������ִ� �ڵ� 
        if (typeof(T) == typeof(AudioSource))
            return Manager.Instance.ObjectManager.Audios.Spawn(key) as T;
        else if (typeof(T) == typeof(Tongue))
            return Manager.Instance.ObjectManager.Tongues.Spawn(key) as T;
        return default(T);
        
    }
    public Spawn(string _key)
    {
        key = _key;
    }
}
public class DeSpawn<T> : ICommand where T : UnityEngine.Object
{
    T element;
    public J Execute<J>() where J : UnityEngine.Object
    {
        Type type = typeof(T);
        if (type == typeof(AudioSource))
        {
            Manager.Instance.ObjectManager.Audios.DeSpawn(element as AudioSource);
        }
        else if (type == typeof(Tongue))
        {
            Manager.Instance.ObjectManager.Tongues.DeSpawn(element as Tongue);
        }
        return default(J);
    }
    public DeSpawn(T element)
    {
        this.element = element;
    }
}
public class PlaySound : ICommand
{
    //Ű��
    string key;
    //��������� �ƴ���
    bool isBG;
    //�Ҹ��� Ʋ���� ��ġ
    Vector3 pos;
    //�Ҹ��� Ʋ���ش�.
    public T Execute<T>() where T : UnityEngine.Object
    {
        if (isBG)
        {
            Manager.Instance.SoundManager.PlayBG(key);
        }
        else
        {
            Manager.Instance.SoundManager.PlaySoundEffect(key, pos);
        }
        return default(T);
    }
    public PlaySound(string _key, bool _isBG, Vector3 pos)
    {
        key = _key;
        isBG = _isBG;
        this.pos = pos;
    }
}