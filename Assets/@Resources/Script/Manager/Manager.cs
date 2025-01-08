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
    public DataManager DataManager = new DataManager();
}
