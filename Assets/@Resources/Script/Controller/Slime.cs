using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Creature
{
    float _speed;
    float _jumpPower;


    Rigidbody _rb;
    //���� �� �ִ� ������Ʈ
    CatchableObject _catchObject;
    //���� ��� �ִ� ������Ʈ
    CatchableObject _currentCatChObject;
    private void Start()
    {
        HatPos = transform.Find("hatpos").gameObject;
        _speed = DataManager.GameData.SlimeData.Speed;
        _jumpPower = DataManager.GameData.SlimeData.JumpPower;
        Debug.Log(_jumpPower);
        _rb = GetComponent<Rigidbody>();
    }
    public override void Jump()
    {
        if (_rb.velocity.y != 0)
            return;
        Debug.Log("����");
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
        transform.Rotate(new Vector3(0,-90 * Time.deltaTime, 0));
    }
    public override void RotateRight()
    {
        transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
    }
    public override void UseAbillity()
    {
        if (_currentCatChObject == null)
        {
            _currentCatChObject = _catchObject;
            _currentCatChObject.Catch();
        }
        else
        {
            _currentCatChObject.Throw();
            _currentCatChObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CatchableObject"))
        {
            _catchObject = other.GetComponent<CatchableObject>();
            _catchObject.Player = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CatchableObject"))
        {
            _catchObject = other.GetComponent<CatchableObject>();
            _catchObject.Player = null;
            _catchObject = null;
        }
    }
}
