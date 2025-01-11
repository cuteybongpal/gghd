using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Creature
{
    float _speed;
    float _jumpPower;

    Rigidbody _rb;
    private void Start()
    {
        HatPos = transform.Find("hatpos").gameObject;
        _speed = DataManager.GameData.FrogData.Speed;
        _jumpPower = DataManager.GameData.FrogData.MaxJumpPower;
        Debug.Log(_jumpPower);
        _rb = GetComponent<Rigidbody>();
    }
    public override void Jump()
    {
        if (_rb.velocity.y != 0)
            return;
        Debug.Log("มกวม");
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
        float anglex = Camera.main.transform.localEulerAngles.x;
        float angley = Camera.main.transform.localEulerAngles.y;


        float x1 = Mathf.Cos(anglex * Mathf.Deg2Rad);
        float y1 = Mathf.Sin(anglex * Mathf.Deg2Rad);

        float x2 = Mathf.Cos(angley * Mathf.Deg2Rad);
        float y2 = Mathf.Sin(angley * Mathf.Deg2Rad);

        
        Debug.DrawRay(Camera.main.transform.position, new Vector3(y2 * x1, -y1, x1 * x2) * 30, Color.red, 3);
        _tongue.shoot(new Vector3(y2 * x1, -y1, x1 * x2));
    }
}
