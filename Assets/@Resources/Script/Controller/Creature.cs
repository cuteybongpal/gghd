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
    //��� �Ұ��� �� �� true��ȯ
    public virtual bool UseAbillity()
    {
        return true;
    }
}
