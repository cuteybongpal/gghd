using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectManager 
{
    HashSet<AudioSource> _audios = new HashSet<AudioSource>();
    List<AudioSource> _audioPool = new List<AudioSource>();


    public T Spawn<T>(string key) where T : UnityEngine.Object
    {
        Type type = typeof(T);

        if (type == typeof(AudioSource))
        {
            //풀링 리스트에 원소가 1개 이상이고, 첫번째 원소가 null인 경우 클리어 해줌(씬 이동으로 파괴되었을 가능성이 있기 때문임)
            if (_audioPool.Count > 0)
                if (_audioPool[0] == null)
                    _audioPool.Clear();
            //풀링 리스트에 요소가 있을 때 
            if (_audioPool.Count > 0)
            {
                AudioSource _source = _audioPool[0];
                _audioPool.Remove(_source);
                //풀링 리스트에서 요소를 꺼내고 지워준다.
                _source.transform.SetParent(null);
                _source.gameObject.SetActive(true);
                return _source as T;
            }
            else
            {
                GameObject _source = Manager.Instance.ResourceManager.Instantiate(key);
                AudioSource _audio = _source.GetComponent<AudioSource>();
                return _audio as T;
            }
        }
        if (type == typeof(Tongue))
        {
            GameObject go = Manager.Instance.ResourceManager.Instantiate(key);
            T t = go.GetComponent<T>();
            return t; 
        }
        return default(T);
    }

    public void DeSapwn<T>(T element) where T : UnityEngine.Object
    {
        Type type = typeof (T);

        if (type == typeof(AudioSource))
        {
            //해쉬셋에서 지워주고 풀링 리스트에 추가해줌
            _audioPool.Add(element as AudioSource);
            _audios.Remove(element as AudioSource);

            AudioSource audio = element as AudioSource;
            //위치를 0,0,0으로 바꾼 후 setActive를 꺼준다.
            audio.transform.position = Vector3.zero;
            audio.gameObject.SetActive(false);
        }
    }
}
