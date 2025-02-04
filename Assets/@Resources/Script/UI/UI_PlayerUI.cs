using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerUI : UI_Base, IObserver
{
    //개구리 스킬 이미지
    Image _imageFrogSkill;
    //슬라임 스킬 이미지
    Image _imageSlimeSkill;
    //총 스킬 이미지
    Image _imageGunSkill;
    //되돌리기 스킬 이미지
    Image _imageRewind;
    //현재 캐릭터 사진
    Image _ImageCurrentCreature;
    void Start()
    {
        _imageFrogSkill = FindChild<Image>("Image_FrogSkill");
        _imageSlimeSkill = FindChild<Image>("Image_SlimeSkill");
        _imageGunSkill = FindChild<Image>("Image_GunSkill");

        _imageRewind = FindChild<Image>("Image_Rewind");
        _ImageCurrentCreature = FindChild<Image>("Image_Creature");
    }
    public void Notify(Define.NotifyEvent notifyEvent, object value)
    {
        if (notifyEvent == Define.NotifyEvent.ChangeBody)
        {
            Define.Creature creature = (Define.Creature)value;

            switch(creature)
            {
                case Define.Creature.Slime:
                    _imageSlimeSkill.gameObject.SetActive(true);
                    _imageGunSkill.gameObject.SetActive(false);
                    _imageFrogSkill.gameObject.SetActive(false);

                    _ImageCurrentCreature.sprite = CommandManager.Instance.ExecuteCommand<Sprite>(new Load("slime.sprite"));
                    break;
                case Define.Creature.Gun:
                    _imageSlimeSkill.gameObject.SetActive(false);
                    _imageGunSkill.gameObject.SetActive(true);
                    _imageFrogSkill.gameObject.SetActive(false);

                    _ImageCurrentCreature.sprite = CommandManager.Instance.ExecuteCommand<Sprite>(new Load("gun.sprite"));
                    break;
                case Define.Creature.Frog:
                    _imageSlimeSkill.gameObject.SetActive(false);
                    _imageGunSkill.gameObject.SetActive(false);
                    _imageFrogSkill.gameObject.SetActive(true);

                    _ImageCurrentCreature.sprite = CommandManager.Instance.ExecuteCommand<Sprite>(new Load("frog.sprite"));
                    break;
            }
        }
        else if (notifyEvent == Define.NotifyEvent.UseAbillity)
        {
            Define.CreatureSkill skill = (Define.CreatureSkill)value;

            switch (skill)
            {
                case Define.CreatureSkill.ShootBullet:
                    ShowCooldown(_imageGunSkill, DataManager.GameData.GunData.ShotCoolDown).Forget();
                    break;
                case Define.CreatureSkill.ThrowingObject:
                    ShowCooldown(_imageSlimeSkill, DataManager.GameData.SlimeData.ThrowingCooldown).Forget();
                    break;
                case Define.CreatureSkill.ShootTongue:
                    ShowCooldown(_imageFrogSkill, DataManager.GameData.FrogData.AbillityCooldown).Forget();
                    break;
                case Define.CreatureSkill.RewindObject:
                    ShowCooldown(_imageRewind, DataManager.Setting.RewindCooldown).Forget();
                    break;
            }
        }
    }
    //쿨타임 보여주기
    async UniTaskVoid ShowCooldown(Image image, float Cooldown)
    {
        float elapsedTime = 0;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            image.fillAmount = elapsedTime / Cooldown;
            await UniTask.Yield();
        }
    }
}
