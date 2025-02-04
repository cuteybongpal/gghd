using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Base : MonoBehaviour
{
    private Dictionary<string, Component> components = new Dictionary<string, Component>();

    protected virtual T FindChild<T>(string name) where T : Component
    {
        //components컴포넌트 안에 name이라는 키값이 있는지 확인
        if (components.ContainsKey(name))
        {
            return components[name].GetComponent<T>();
        }
        T[] _components = GetComponentsInChildren<T>();
        foreach (T component in _components)
        {
            if (component.name == name)
                return component;
        }
        return null;
    }
}
