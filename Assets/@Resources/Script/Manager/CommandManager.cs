using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    #region 싱글톤 코드
    private static CommandManager instance = null;
    public static CommandManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            Application.targetFrameRate = 100;
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            Manager.Instance.ResourceManager.LoadAllAsync<Sprite>("PreLoad(Sprite)", () =>
            {
                Manager.Instance.ResourceManager.LoadAllAsync<TextAsset>("PreLoad(Data)", () =>
                {
                    Manager.Instance.DataManager.Init();
                    Manager.Instance.ResourceManager.LoadAllAsync<GameObject>("PreLoad(Prefab)", () =>
                    {
                        EventManager.AssetAllLoading?.Invoke();
                    });
                });
            });

        }
        else
        {
            Destroy(instance);
        }
    }
    #endregion
    
    //ICommand 인터페이스를 상속받는 클래스를 파라미터로 넣어주고, ICommand의 Execute함수를 실행시켜준다.
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
    //ResourceManager에서 오브젝트를 가져오는 메소드
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
    //ObjectManager에서 스폰해주는 코드 
    string key;
    public T Execute<T>() where T : UnityEngine.Object
    {
        //todo : 오브젝트 매니저 만들기, 생성해주는 코드 
        if (typeof(T) == typeof(AudioSource))
            return Manager.Instance.ObjectManager.Audios.Spawn(key) as T;
        else if (typeof(T) == typeof(Tongue))
            return Manager.Instance.ObjectManager.Tongues.Spawn(key) as T;
        else if (typeof(T) == typeof(Bullet))
            return Manager.Instance.ObjectManager.Bullets.Spawn(key) as T;
        else if (typeof(T) == typeof(PlayerController))
            return Manager.Instance.ObjectManager.Player.Spawn(key) as T;
        else if (typeof(T).BaseType == typeof(UI_Base))
            return Manager.Instance.ObjectManager.UIs.Spawn<UI_Base>(key) as T;
        else if (typeof(T) == typeof(Slime))
            return Manager.Instance.ObjectManager.Slimes.Spawn(key) as T;
        else if (typeof(T) == typeof(Gun))
            return Manager.Instance.ObjectManager.Guns.Spawn(key) as T;
        else if (typeof(T) == typeof(Frog))
            return Manager.Instance.ObjectManager.Frogs.Spawn(key) as T;
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
            Manager.Instance.ObjectManager.Audios.DeSpawn(element as AudioSource);
        else if (type == typeof(Tongue))
            Manager.Instance.ObjectManager.Tongues.DeSpawn(element as Tongue);
        else if (type == typeof(Bullet))
            Manager.Instance.ObjectManager.Bullets.DeSpawn(element as Bullet);
        return default(J);
    }
    public DeSpawn(T element)
    {
        this.element = element;
    }
}
public class PlaySound : ICommand
{
    //키값
    string key;
    //배경음인지 아닌지
    bool isBG;
    //소리를 틀어줄 위치
    Vector3 pos;
    //소리를 틀어준다.
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