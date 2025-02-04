using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Creature
{
    float _speed;
    float _shotSpeed;
    float _shotCooldown;
    float _jumpPower;
    bool _canShoot;
    bool CanShoot 
    {  
        get { return _canShoot; }
        set
        {
            _canShoot = value;
            if (!value)
                WaitTilCanUseAbillity().Forget();
        }
    }
    Rigidbody _rb;
    Transform shooter;

    void Start()
    {
        HatPos = transform.Find("hatpos").gameObject;

        _speed = DataManager.GameData.GunData.Speed;
        _shotSpeed = DataManager.GameData.GunData.ShotSpeed;
        _shotCooldown = DataManager.GameData.GunData.ShotCoolDown;
        _jumpPower = DataManager.GameData.GunData.JumpPower;
        _canShoot = true;

        _rb = GetComponent<Rigidbody>();
        shooter = transform.Find("shooter");
        CreatureType = Define.Creature.Gun;
    }

    public override void MoveFoward()
    {
        transform.Translate(Vector3.forward *  _speed * Time.deltaTime);
    }
    public override void MoveBackward()
    {
        transform.Translate(Vector3.back * _speed * Time.deltaTime);
    }
    public override void RotateLeft()
    {
        transform.Rotate(0, -90 * Time.deltaTime, 0);
    }
    public override void RotateRight()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }
    public override void Jump()
    {
        if (_rb.velocity.y != 0)
            return;
        Debug.Log("점프");
        _rb.velocity = new Vector3(0, _jumpPower, 0);
    }
    public override bool UseAbillity()
    {
        if (!_canShoot)
            return false;
        Debug.Log("발사 히히");
        Bullet _bullet = CommandManager.Instance.ExecuteCommand<Bullet>(new Spawn("bullet.prefab"));
        _bullet.transform.position = shooter.position;
        _bullet.Fire(transform.forward, _shotSpeed);
        CanShoot = false;
        return true;
    }
    async UniTaskVoid WaitTilCanUseAbillity()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_shotCooldown));
        CanShoot = true;
    }
}

