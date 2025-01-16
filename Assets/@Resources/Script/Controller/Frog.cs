using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Creature, IObserver
{
    float _speed;
    float _jumpPower;
    float _tongueRange;
    bool _isAttach;
    Rigidbody _rb;

    private void Start()
    {
        HatPos = transform.Find("hatpos").gameObject;
        _speed = DataManager.GameData.FrogData.Speed;
        _jumpPower = DataManager.GameData.FrogData.MaxJumpPower;
        
        _rb = GetComponent<Rigidbody>();
    }
    public override void Jump()
    {
        if (_rb.velocity.y != 0)
            return;
        _rb.velocity = new Vector3(0, _jumpPower, 0);
    }
    public override void MoveFoward()
    {
        transform.Translate(new Vector3(0, 0, 1) * _speed * Time.deltaTime);
    }
    public override void MoveBackward()
    {
        transform.Translate(new Vector3(0, 0, -1) * _speed * Time.deltaTime);
    }
    public override void RotateLeft()
    {
        transform.Rotate(new Vector3(0, -90 * Time.deltaTime, 0));
    }
    public override void RotateRight()
    {
        transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
    }
    public override void UseAbillity()
    {
        Tongue _tongue = CommandManager.Instance.ExecuteCommand<Tongue>(new Spawn("tongue.prefab"));
        _tongue.transform.position = transform.position;
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 10);
        Vector3 collisionPos = Vector3.zero;
        foreach (RaycastHit hit in hits )
        {
            if (hit.collider.CompareTag("Wall"))
            {
                collisionPos = hit.transform.position;
                
                break;
            }
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10 , Color.green, 3);
        Debug.DrawRay(transform.position, (collisionPos - transform.position).normalized * 30, Color.red, 3);
        if (collisionPos != Vector3.zero)
            _tongue.shoot((collisionPos - transform.position).normalized);
    }

    public void Notify(Define.NotifyEvent notifyEvent, object value)
    {
        
    }
}
