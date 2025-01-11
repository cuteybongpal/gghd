using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager  
{
    //싱글톤 객체 매니저 내부에서만 사용함
    static Manager instance = new Manager();
    public static Manager Instance { get { return instance; } }
    //리소스를 로딩하는 클래스
    public ResourceManager ResourceManager = new ResourceManager();
    //데이터의 저장 불러오기 하는 클래스
    public DataManager DataManager = new DataManager();
    //소리를 플레이해주는 클래스
    public SoundManager SoundManager = new SoundManager();
    //스폰과 디스폰을 해주는 클래스
    public ObjectManager ObjectManager = new ObjectManager();
}
