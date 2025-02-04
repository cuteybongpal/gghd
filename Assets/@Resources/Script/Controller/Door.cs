using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    // ���� ���� �� �̵��� ��ġ�� ������
    public Vector3 _openPositionOffset;

    // ���� ���� ��ġ
    private Vector3 _originalPosition;

    // ���� �ݴ� �ִϸ��̼ǿ� �ɸ��� �ð�
    private float _animationDuration = 3f;

    // ���� ���� �ִ��� ���θ� ��Ÿ���� �÷���
    private bool _isOpen;

    // �ε巯�� �ִϸ��̼��� ���� ��� �ð�
    private float _elapsedTime;

    private void Start()
    {
        // ���� �ʱ� ��ġ ����
        _originalPosition = transform.position;
    }

    // ���� �ݴ� ��ȣ�ۿ��� ȣ��� �� ����
    public void DeInteract()
    {
        _isOpen = false;
        Close();
    }

    // ���� ���� ��ȣ�ۿ��� ȣ��� �� ����
    public void Interact()
    {
        _isOpen = true;
        Open();
    }

    // ���� ���� �ִϸ��̼� ó��
    private async void Open()
    {
        // �ִϸ��̼��� �Ϸ�Ǹ� ��� �ð� �ʱ�ȭ
        if (_elapsedTime <= 0)
        {
            _elapsedTime = 0;
        }
        // �ִϸ��̼��� �����ų� ���� ������ ������ ����
        while (_elapsedTime <= _animationDuration && _isOpen)
        {
            // ���� ��ġ�� ��ǥ ��ġ ���̸� �����Ͽ� ���ο� ��ġ ���
            Vector3 newPosition = Vector3.Lerp(_originalPosition, _originalPosition + _openPositionOffset, _elapsedTime / _animationDuration);
            transform.position = newPosition;

            _elapsedTime += Time.deltaTime;
            
            await UniTask.Yield();
        }
        
    }

    // ���� �ݴ� �ִϸ��̼� ó��
    private async void Close()
    {
        // �ִϸ��̼��� �Ϸ�Ǹ� ��� �ð� �ʱ�ȭ
        if (_elapsedTime >= _animationDuration)
        {
            _elapsedTime = _animationDuration;
        }
        // �ִϸ��̼��� �����ų� ���� ������ ������ ����
        while (_elapsedTime > 0 && !_isOpen)
        {
            // ��ǥ ��ġ�� ���� ��ġ ���̸� �����Ͽ� ���ο� ��ġ ���
            Vector3 newPosition = Vector3.Lerp(_originalPosition, _originalPosition + _openPositionOffset, _elapsedTime / _animationDuration);
            transform.position = newPosition;

            _elapsedTime -= Time.deltaTime;
            await UniTask.Yield();
        }
        
    }
}
