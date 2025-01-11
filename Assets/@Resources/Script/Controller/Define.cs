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
        Damaged,
        Appear,
        Disappear
    }
    public static float DELTATIME = 0.01f;
}
