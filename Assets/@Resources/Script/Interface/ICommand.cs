using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    public T Execute<T>() where T : UnityEngine.Object;
}
