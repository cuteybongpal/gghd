using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    // 문이 열릴 때 이동할 위치의 오프셋
    public Vector3 _openPositionOffset;

    // 문의 원래 위치
    private Vector3 _originalPosition;

    // 열고 닫는 애니메이션에 걸리는 시간
    private float _animationDuration = 3f;

    // 문이 열려 있는지 여부를 나타내는 플래그
    private bool _isOpen;

    // 부드러운 애니메이션을 위한 경과 시간
    private float _elapsedTime;

    private void Start()
    {
        // 문의 초기 위치 저장
        _originalPosition = transform.position;
    }

    // 문을 닫는 상호작용이 호출될 때 실행
    public void DeInteract()
    {
        _isOpen = false;
        Close();
    }

    // 문을 여는 상호작용이 호출될 때 실행
    public void Interact()
    {
        _isOpen = true;
        Open();
    }

    // 문을 여는 애니메이션 처리
    private async void Open()
    {
        // 애니메이션이 완료되면 경과 시간 초기화
        if (_elapsedTime <= 0)
        {
            _elapsedTime = 0;
        }
        // 애니메이션이 끝나거나 문이 닫히기 전까지 실행
        while (_elapsedTime <= _animationDuration && _isOpen)
        {
            // 원래 위치와 목표 위치 사이를 보간하여 새로운 위치 계산
            Vector3 newPosition = Vector3.Lerp(_originalPosition, _originalPosition + _openPositionOffset, _elapsedTime / _animationDuration);
            transform.position = newPosition;

            _elapsedTime += Time.deltaTime;
            
            await UniTask.Yield();
        }
        
    }

    // 문을 닫는 애니메이션 처리
    private async void Close()
    {
        // 애니메이션이 완료되면 경과 시간 초기화
        if (_elapsedTime >= _animationDuration)
        {
            _elapsedTime = _animationDuration;
        }
        // 애니메이션이 끝나거나 문이 열리기 전까지 실행
        while (_elapsedTime > 0 && !_isOpen)
        {
            // 목표 위치와 원래 위치 사이를 보간하여 새로운 위치 계산
            Vector3 newPosition = Vector3.Lerp(_originalPosition, _originalPosition + _openPositionOffset, _elapsedTime / _animationDuration);
            transform.position = newPosition;

            _elapsedTime -= Time.deltaTime;
            await UniTask.Yield();
        }
        
    }
}
