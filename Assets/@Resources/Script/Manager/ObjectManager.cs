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
            //Ǯ�� ����Ʈ�� ���Ұ� 1�� �̻��̰�, ù��° ���Ұ� null�� ��� Ŭ���� ����(�� �̵����� �ı��Ǿ��� ���ɼ��� �ֱ� ������)
            if (_audioPool.Count > 0)
                if (_audioPool[0] == null)
                    _audioPool.Clear();
            //Ǯ�� ����Ʈ�� ��Ұ� ���� �� 
            if (_audioPool.Count > 0)
            {
                AudioSource _source = _audioPool[0];
                _audioPool.Remove(_source);
                //Ǯ�� ����Ʈ���� ��Ҹ� ������ �����ش�.
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
            //�ؽ��¿��� �����ְ� Ǯ�� ����Ʈ�� �߰�����
            _audioPool.Add(element as AudioSource);
            _audios.Remove(element as AudioSource);

            AudioSource audio = element as AudioSource;
            //��ġ�� 0,0,0���� �ٲ� �� setActive�� ���ش�.
            audio.transform.position = Vector3.zero;
            audio.gameObject.SetActive(false);
        }
    }
}
