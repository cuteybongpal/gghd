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

    //��ȯŸ���� ���� ��ɽ��� �޼ҵ�
    
    //��ȯŸ���� �ִ� ��ɽ��� �޼ҵ�
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
    //ResourceManager���� ������Ʈ�� �������� �޼ҵ�
    public T Execute<T>(string key) where T : UnityEngine.Object
    {
        T loadedobject = Manager.Instance.ResourceManager.Load<T>(key);
        return loadedobject as T;
    }
}
public class Spawn : ICommand
{
    //ObjectManager���� �������ִ� �ڵ� 
    public T Execute<T>(string key) where T : UnityEngine.Object
    {
        //todo : ������Ʈ �Ŵ��� �����, �������ִ� �ڵ� �����
        return default(T);
    }
}
public class GetGameData : ICommand
{
    //������ �Ŵ����� �ִ� ������ ������
    public T Execute<T>(string data) where T : UnityEngine.Object
    {
        return 10 as T;
    }
}
