using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
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
                EventManager.AssetAllLoading?.Invoke();
            });

        }
        else
        {
            Destroy(instance);
        }
    }

    //반환타입이 없는 명령실행 메소드
    
    //반환타입이 있는 명령실행 메소드
    public T ExecuteCommand<T>(ICommand command, string data) where T : UnityEngine.Object
    {
        
        if (command == null)
        {
            Debug.LogError("Invalid command");
            return default;
        }
        return command.Execute<T>(data);
    }
}

public class Load : ICommand
{
    //ResourceManager에서 오브젝트를 가져오는 메소드
    public T Execute<T>(string key) where T : UnityEngine.Object
    {
        T loadedobject = Manager.Instance.ResourceManager.Load<T>(key);
        return loadedobject as T;
    }
}
public class Spawn : ICommand
{
    //ObjectManager에서 스폰해주는 코드 
    public T Execute<T>(string key) where T : UnityEngine.Object
    {
        //todo : 오브젝트 매니저 만들기, 생성해주는 코드 만들기
        return default(T);
    }
}
public class GetGameData : ICommand
{
    //데이터 매니저에 있는 값들을 가져옴
    public T Execute<T>(string data) where T : UnityEngine.Object
    {
        return 10 as T;
    }
}
