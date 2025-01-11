using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    //����� ����� �ҽ�
    AudioSource _audioBG;
    //���� ����Ʈ ������ҽ� ����Ʈ
    List<AudioSource> _audioSoundEffect = new List<AudioSource>();
    //������� Ʋ���ش�.
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
    //���� ����Ʈ�� �÷��� ����
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
    //����� Ŭ�� ��ŭ ���� ����� �ҽ� �ȿ� Ŭ���� ����ش�.
    async UniTaskVoid TurnOffSound(AudioSource source, AudioClip clip)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(clip.length));
        source.Stop();
        source.clip = null;
    }
    //��� �Ҹ��� ��� ���ߴ� �ڵ�
    public void StopAllMusic()
    {
        //������ �����.
        _audioBG.Pause();
        foreach(AudioSource source in _audioSoundEffect)
        {
            source.Pause();
        }
    }
    //��� �Ҹ��� ���� �������� �ٽ� Ʋ���ִ� �ڵ�
    public void PlayAllMusic()
    {
        //������ �ٽ� ���� �������� Ʋ���ش�.
        _audioBG.UnPause();
        foreach (AudioSource source in _audioSoundEffect)
        {
            source.UnPause();
        }
    }
    //������ �����ϴ� �Լ�
    public void SetVolume(float volume)
    {
        _audioBG.volume = volume;
        foreach(AudioSource source in _audioSoundEffect)
        {
            source.volume = volume;
        }
    }

}
