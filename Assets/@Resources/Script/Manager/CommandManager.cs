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
        //todo : ������Ʈ �Ŵ��� �����, �������ִ� �ڵ� �����
        T Object = Manager.Instance.ObjectManager.Spawn<T>(key);
        return Object;
    }
    public Spawn(string _key)
    {
        key = _key;
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