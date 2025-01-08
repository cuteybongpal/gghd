using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public T Execute<T>(string data) where T : UnityEngine.Object;
}
