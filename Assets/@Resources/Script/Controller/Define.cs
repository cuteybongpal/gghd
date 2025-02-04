using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum CommandType
    {
        Load,
        Spawn,
        PlayMusic,
    }
    public enum NotifyEvent
    {
        ChangeBody,
        UseAbillity,
        Appear,
        Disappear,
        Attach
    }
    public enum Creature
    {
        Slime,
        Frog,
        Gun
    }
    public enum CreatureSkill
    {
        ThrowingObject,
        ShootTongue,
        ShootBullet,
        RewindObject
    }
}
