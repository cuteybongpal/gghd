using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISubject
{
    Rigidbody _rb;

    bool _isThrowing = false;
    bool _isCanRewind = true;
    bool IsCanRewind
    {
        get { return _isThrowing; }
        set 
        {
            _isThrowing = value;
            if (!value)
                WaitTilCanUseAbillity().Forget();
        }
    }
    float _rewindCooldown;
    //현재 변신해 있는 캐릭터
    public Creature CurrentCreature;

    HashSet<IObserver> _observers = new HashSet<IObserver>();


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rewindCooldown = DataManager.Setting.RewindCooldown;
        SetCreature(CurrentCreature);
        Camera.main.GetComponent<PlayerCamera>().target = CurrentCreature.transform;
        NotifyObserver(Define.NotifyEvent.ChangeBody, CurrentCreature.CreatureType);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            CurrentCreature.MoveFoward();
        }
        if (Input.GetKey(KeyCode.S))
        {
            CurrentCreature.MoveBackward();
        }
        if (Input.GetKey(KeyCode.A))
        {
            CurrentCreature.RotateLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            CurrentCreature.RotateRight();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentCreature.UseAbillity())
            {
                switch (CurrentCreature.CreatureType)
                {
                    case Define.Creature.Slime:
                        NotifyObserver(Define.NotifyEvent.UseAbillity, Define.CreatureSkill.ThrowingObject);
                        break;
                    case Define.Creature.Frog:
                        NotifyObserver(Define.NotifyEvent.UseAbillity, Define.CreatureSkill.ShootTongue);
                        break;
                    case Define.Creature.Gun:
                        NotifyObserver(Define.NotifyEvent.UseAbillity, Define.CreatureSkill.ShootBullet);
                        break;
                }
                
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            CurrentCreature.Jump();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ThrowHat().Forget();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!_isCanRewind)
                return;
            float anglex = Camera.main.transform.localEulerAngles.x;
            float angley = Camera.main.transform.localEulerAngles.y;

            float x1 = Mathf.Cos(anglex * Mathf.Deg2Rad);
            float y1 = Mathf.Sin(anglex * Mathf.Deg2Rad);

            float x2 = Mathf.Cos(angley * Mathf.Deg2Rad);
            float y2 = Mathf.Sin(angley * Mathf.Deg2Rad);

            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, new Vector3(y2 * x1, -y1, x1 * x2), 30);
            RewindableObject rewindObject = null;
            foreach (RaycastHit hit in hits)
            {
                RewindableObject go =  hit.collider.GetComponent<RewindableObject>();
                if (go != null)
                {
                    rewindObject = go;
                    break;
                }
            }
            if (rewindObject != null)
            {
                rewindObject.StartRewind();
                NotifyObserver(Define.NotifyEvent.UseAbillity, Define.CreatureSkill.RewindObject);
            }
        }
    }
    async UniTaskVoid ThrowHat()
    {
        if (_isThrowing)
            return;
        _isThrowing = true;
        float duration = 1f;
        float elapsedtime = 0f;

        float anglex = Camera.main.transform.localEulerAngles.x;
        float angley = Camera.main.transform.localEulerAngles.y;

        float x1 = Mathf.Cos(anglex * Mathf.Deg2Rad);
        float y1 = Mathf.Sin(anglex * Mathf.Deg2Rad);

        float x2 = Mathf.Cos(angley * Mathf.Deg2Rad);
        float y2 = Mathf.Sin(angley * Mathf.Deg2Rad);

        Vector3 dir = new Vector3(y2 * x1, -y1, x1 * x2);
        transform.SetParent(null);
        while (elapsedtime < duration)
        {
            if (!_isThrowing)
                return;
            transform.position += dir * DataManager.Setting.HatThrowingPower * Time.deltaTime;
            elapsedtime += Time.deltaTime;
            await UniTask.Yield();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        elapsedtime = 0f;
        while (elapsedtime < duration)
        {
            if (!_isThrowing)
                return;
            transform.position = Vector3.Lerp(transform.position, CurrentCreature.HatPos.transform.position, elapsedtime / duration);
            elapsedtime += Time.deltaTime;
            await UniTask.Yield();
        }
        transform.position = CurrentCreature.HatPos.transform.position;
        transform.SetParent(CurrentCreature.HatPos.transform);
        _isThrowing = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Creature creature = other.GetComponent<Creature>();

        if (creature == null)
            return;
        if (creature == CurrentCreature)
            return;
        SetCreature(creature);
    }
    void SetCreature(Creature _creature)
    {
        if (!_isThrowing)
            return;
        CurrentCreature = _creature;
        transform.SetParent(CurrentCreature.HatPos.transform);
        transform.position = CurrentCreature.HatPos.transform.position;
        transform.rotation = Quaternion.identity;
        Camera.main.GetComponent<PlayerCamera>().target = CurrentCreature.transform;
        _isThrowing = false;
        NotifyObserver(Define.NotifyEvent.ChangeBody, _creature.CreatureType);
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
        foreach(var observer in _observers)
        {
            observer.Notify(notifyEvent, value);
        }
    }
    async UniTaskVoid WaitTilCanUseAbillity()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_rewindCooldown));
        IsCanRewind = true;
    }
}
