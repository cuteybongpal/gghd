using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public PlayerController _player;
    public GameObject HatPos;
    public Define.Creature CreatureType;
    public virtual void MoveFoward()
    {

    }
    public virtual void MoveBackward()
    {

    }
    public virtual void RotateLeft()
    {

    }
    public virtual void RotateRight()
    {

    }
    public virtual void Jump()
    {

    }
    //사용 불가가 될 시 true반환
    public virtual bool UseAbillity()
    {
        return true;
    }
}
