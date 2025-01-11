using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager  
{
    //�̱��� ��ü �Ŵ��� ���ο����� �����
    static Manager instance = new Manager();
    public static Manager Instance { get { return instance; } }
    //���ҽ��� �ε��ϴ� Ŭ����
    public ResourceManager ResourceManager = new ResourceManager();
    //�������� ���� �ҷ����� �ϴ� Ŭ����
    public DataManager DataManager = new DataManager();
    //�Ҹ��� �÷������ִ� Ŭ����
    public SoundManager SoundManager = new SoundManager();
    //������ ������ ���ִ� Ŭ����
    public ObjectManager ObjectManager = new ObjectManager();
}
