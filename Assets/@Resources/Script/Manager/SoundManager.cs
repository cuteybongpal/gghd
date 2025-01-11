using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    //배경음 오디오 소스
    AudioSource _audioBG;
    //사운드 이펙트 오디오소스 리스트
    List<AudioSource> _audioSoundEffect = new List<AudioSource>();
    //배경음을 틀어준다.
    public void PlayBG(string key)
    {
        if (_audioBG == null)
        {
            _audioBG = CommandManager.Instance.gameObject.gameObject.AddComponent<AudioSource>();
            _audioBG.playOnAwake = false;
            _audioBG.loop = true;
            _audioBG.spatialBlend = 0;
        }

        AudioClip clip = CommandManager.Instance.ExecuteCommand<AudioClip>(new Load(key));
        _audioBG.clip = clip;
        _audioBG.Play();

    }
    //사우드 이펙트를 플레이 해줌
    public void PlaySoundEffect(string key, Vector3 pos)
    {
        if ( _audioSoundEffect.Count < 0)
        {
            _audioSoundEffect.Add(CommandManager.Instance.ExecuteCommand<AudioSource>(new Spawn("Sound.prefab")));
        }
        bool isAllFilled = true;
        foreach(AudioSource audioSource in _audioSoundEffect)
        {
            if (audioSource.clip != null)
            {
                isAllFilled &= true;
            }
            else
            {
                isAllFilled &= false;
                break;
            }
        }
    }
    //오디오 클립 만큼 쉬고 오디오 소스 안에 클립을 비워준다.
    async UniTaskVoid TurnOffSound(AudioSource source, AudioClip clip)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(clip.length));
        source.Stop();
        source.clip = null;
    }
    //모든 소리를 잠시 멈추는 코드
    public void StopAllMusic()
    {
        //음악을 멈춘다.
        _audioBG.Pause();
        foreach(AudioSource source in _audioSoundEffect)
        {
            source.Pause();
        }
    }
    //모든 소리를 멈춘 시점에서 다시 틀어주는 코드
    public void PlayAllMusic()
    {
        //음악을 다시 멈춘 시점에서 틀어준다.
        _audioBG.UnPause();
        foreach (AudioSource source in _audioSoundEffect)
        {
            source.UnPause();
        }
    }
    //음량을 조절하는 함수
    public void SetVolume(float volume)
    {
        _audioBG.volume = volume;
        foreach(AudioSource source in _audioSoundEffect)
        {
            source.volume = volume;
        }
    }

}
