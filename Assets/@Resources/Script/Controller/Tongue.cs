using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour, ISubject
{
    Vector3 _originPos;
    Rigidbody _rb;
    Creature _owner;
    bool _isAttach = false;
    List<IObserver> _observers = new List<IObserver>();
    public void Init(Creature owner)
    {
        _owner = owner;
        RegisterObserver(owner as IObserver);
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public void shoot(Vector3 dir)
    {
        _originPos = transform.position;

        transform.LookAt(transform.position + dir);
        transform.Rotate(90, 0, 0);
        _rb.velocity = dir * DataManager.GameData.FrogData.TongueSpeed;
        CheckDistance().Forget();
    }

    async UniTaskVoid CheckDistance()
    {
        float distance  = 0;
        while (distance <= DataManager.GameData.FrogData.TongueRange)
        {
            distance = Vector3.Distance(transform.position, _originPos);
            if (_isAttach)
                return;
            await UniTask.Yield();
        }
        CommandManager.Instance.ExecuteCommand<Tongue>(new DeSpawn<Tongue>(this));
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;
        if (other.CompareTag("Wall"))
        {
            NotifyObserver(Define.NotifyEvent.Attach, null);
            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }
    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }
    public void NotifyObserver(Define.NotifyEvent notifyEvent, object value)
    {
        foreach (var observer in _observers)
        {
            observer.Notify(notifyEvent, value);
        }
    }
}
