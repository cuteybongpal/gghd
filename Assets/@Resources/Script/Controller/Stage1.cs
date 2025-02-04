using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{
    PlayerController _player;

    void Awake()
    {
        Slime slime = CommandManager.Instance.ExecuteCommand<Slime>(new Spawn("Slime.prefab"));
        UI_PlayerUI playerUI = CommandManager.Instance.ExecuteCommand<UI_PlayerUI>(new Spawn("PlayerUI"));
        _player = CommandManager.Instance.ExecuteCommand<PlayerController>(new Spawn("Player.prefab"));
        _player.RegisterObserver(playerUI);
        _player.CurrentCreature = slime;
    }
}
