using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class CatchableObject : MonoBehaviour, ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    Creature _player;
    Rigidbody _rb;
    LineRenderer _lineRenderer;
    bool isThrowing = false;

    public Creature Player
    {
        private get { return _player; }
        set 
        {
            _player = value;
            if (_player != null)
            {
                NotifyObserver(Define.NotifyEvent.Appear, null);
            }
            else
            {
                NotifyObserver(Define.NotifyEvent.Disappear, null);
            }
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
        foreach (IObserver observer in _observers)
        {
            observer.Notify(notifyEvent, value);
        }
    }

    void Start()
    {
        RegisterObserver(transform.Find("UI_Catchable").GetComponent<IObserver>());
        NotifyObserver(Define.NotifyEvent.Disappear, null);

        _rb = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.positionCount = 300;
    }
    
    void Update()
    {

    }
    public bool Catch()
    {
        _rb.useGravity = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
        if (isThrowing)
            return false;
        _rb.velocity = Vector3.zero;
        transform.rotation = Player.transform.rotation;
        float angle = Player.transform.localEulerAngles.y * Mathf.Deg2Rad;
        float xpos = Mathf.Sin(angle) * 1.7f + Player.transform.position.x;
        float zpos = Mathf.Cos(angle) * 1.7f + Player.transform.position.z;
        transform.position = new Vector3(xpos, Player.transform.position.y + 0.6f, zpos);
        transform.SetParent(Player.transform);
        DrawLine().Forget();
        return true;
    }
    public void Throw()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _lineRenderer.positionCount = 0;
        isThrowing = true;
        float angle = Player.transform.localEulerAngles.y * Mathf.Deg2Rad;
        float xpos = Mathf.Sin(angle);
        float zpos = Mathf.Cos(angle);
        Vector3 Throwdir = new Vector3(xpos, 2, zpos);
        CalcPos(Throwdir, DataManager.Setting.ThrowingPower);
    }
    async void CalcPos(Vector3 dir, float Power)
    {
        float time = 0;
        transform.SetParent(null);
        Vector3 velocity = dir * Power; // 초기 속도 계산
        while (true)
        {
            time += 0.02f;

            // 중력에 의한 속도 변화
            velocity.y -= getFallSpeed(time);

            // 위치 업데이트
            _rb.velocity = velocity;

            await UniTask.Delay(TimeSpan.FromSeconds(0.02));

            // 충돌 체크 및 종료 조건
            if (_rb.velocity.y == 0)
                break;
        }
        _rb.velocity = Vector3.zero;
        isThrowing = false;
        _rb.useGravity = true;
    }
    async UniTaskVoid DrawLine()
    {
        _lineRenderer.positionCount = 300;
        while (!isThrowing)
        {
            Vector3 currentPos = transform.position;
            float angle = Player.transform.localEulerAngles.y * Mathf.Deg2Rad;
            float time = 0;
            float xpos = Mathf.Sin(angle);
            float zpos = Mathf.Cos(angle);
            Vector3 dir = new Vector3(xpos, 2, zpos);
            Vector3 velocity = dir * DataManager.Setting.ThrowingPower; // 초기 속도
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, currentPos);

                time += 0.05f;

                // 중력 적용
                velocity.y -= getFallSpeed(time);

                // 위치 업데이트
                currentPos += velocity * 0.05f;
            }
            await UniTask.Yield();
        }
    }
    public float getFallSpeed(float time)
    {
        return (DataManager.Setting.GravityScale * time * time)/2;
    }
}
