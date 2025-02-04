using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Creature, IObserver
{
    float _speed;
    float _jumpPower;
    bool _isAttach;
    bool _isDrawLine;
    bool _isThrowing;
    bool _canUseAbillity = true;
    bool CanUseAbillity
    {
        get { return _canUseAbillity; }
        set 
        {
            _canUseAbillity = value;
            if (!value)
                WaitTilCanUseAbillity().Forget();
        }
    }
    Rigidbody _rb;
    ConfigurableJoint _joint;
    Tongue _tongue;
    LineDrawer _line;

    private void Start()
    {
        HatPos = transform.Find("hatpos").gameObject;
        _speed = DataManager.GameData.FrogData.Speed;
        _jumpPower = DataManager.GameData.FrogData.MaxJumpPower;
        
        _rb = GetComponent<Rigidbody>();
        _line = GetComponentInChildren<LineDrawer>();
        _line.SetPosCount(2);
        CreatureType = Define.Creature.Frog;
    }
    public override void Jump()
    {
        if (_rb.velocity.y != 0)
            return;
        _rb.velocity = new Vector3(0, _jumpPower, 0);
    }
    public override void MoveFoward()
    {
        if (!_isAttach)
            transform.Translate(new Vector3(0, 0, 1) * _speed * Time.deltaTime);
        else
            _rb.AddForce(transform.forward * _speed, ForceMode.Force);
    }
    public override void MoveBackward()
    {
        if (!_isAttach)
            transform.Translate(new Vector3(0, 0, -1) * _speed * Time.deltaTime);
        else
            _rb.AddForce(-transform.forward * _speed, ForceMode.Force);
    }
    public override void RotateLeft()
    {
        transform.Rotate(new Vector3(0, -90 * Time.deltaTime, 0));
    }
    public override void RotateRight()
    {
        transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
    }
    public override bool UseAbillity()
    {
        if (!CanUseAbillity)
            return false;
        if (_isThrowing)
            return false;
        if (!_isAttach)
        {
            
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, DataManager.GameData.FrogData.TongueRange);
            Vector3 collisionPos = Vector3.zero;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    collisionPos = hit.point;
                    break;
                }
            }
            if (collisionPos != Vector3.zero)
            {
                _tongue = CommandManager.Instance.ExecuteCommand<Tongue>(new Spawn("tongue.prefab"));
                _tongue.transform.position = transform.position;
                _tongue.Init(this);
                _tongue.shoot((collisionPos - transform.position).normalized);
                _isThrowing = true;
                _isDrawLine = true;
                
                DrawTongueLine().Forget();
            }
            return false;
        }
        else
        {
            _joint.connectedBody = null;
            CommandManager.Instance.ExecuteCommand<Tongue>(new DeSpawn<Tongue>(_tongue));
            _tongue = null;
            _isAttach = false;
            CanUseAbillity = false;
            _isDrawLine = false;
            Destroy(_joint);
            return true;
        }
    }

    public void Notify(Define.NotifyEvent notifyEvent, object value)
    {
        if (notifyEvent == Define.NotifyEvent.Attach)
        {
            _joint = AddJoint(Vector3.Distance(transform.position, _tongue.transform.position));
            _isAttach = true;
            _isThrowing = false;
        }
    }
    async UniTaskVoid DrawTongueLine()
    {
        while (true)
        {
            if (!_isDrawLine)
                break;
            _line.SetPosCount(2);
            _line.SetPosition(0);
            _line.SetPosition(1, _tongue.transform.position);
            
            if (!_isDrawLine)
                break;
            await UniTask.Yield();
            _line.ResetLine();
        }
        _line.ResetLine();
    }
    ConfigurableJoint AddJoint(float distance)
    {
        ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = _tongue.GetComponent<Rigidbody>();
        joint.zMotion = ConfigurableJointMotion.Limited;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;

        joint.linearLimit = new SoftJointLimit { limit = distance };

        JointDrive drive = new JointDrive();
        drive.positionSpring = 100f; // 스프링 강도
        drive.positionDamper = 10f;  // 댐퍼 강도
        drive.maximumForce = Mathf.Infinity; // 최대 힘

        // x, y, z축에 대해 스프링 적용
        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        return joint;
    }
    async UniTaskVoid WaitTilCanUseAbillity()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(DataManager.GameData.FrogData.AbillityCooldown));
        CanUseAbillity = true;
    }
}
