using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.tvOS;

public class RewindableObject : MonoBehaviour
{
    //�ð����� �̺� �� ���̱⿡ ����� 0.01�� �� 100�������� �̺� �� �������� ����
    const float deltatime = 0.01f;
    public enum State
    {
        Recording,
        Rewind
    }
    public State _currentState = State.Recording;
    public State CurrentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            switch(_currentState)
            {
                case State.Recording:
                    Recording().Forget();
                    break;
                case State.Rewind:
                    Rewind().Forget();
                    break;
                default:
                    break;
            }
        }
    }
    Rigidbody _rb;

    public List<Vector3> Positions = new List<Vector3>();
    public List<Quaternion> Rotations = new List<Quaternion>();

    public List<Vector3> PositionDerivative = new List<Vector3>();
    public List<Vector3> RotationDerivative = new List<Vector3>();
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        CurrentState = State.Recording;
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (CurrentState != State.Rewind) 
                CurrentState = State.Rewind;
        }
    }
    void Add(Vector3 position, Quaternion rotation)
    {
        Positions.Add(position);
        Rotations.Add(rotation);

        if (Positions.Count > 1 && Rotations.Count > 1 && Positions.Count == Rotations.Count)
        {
            //�ε����� �������� ����
            PositionDerivative.Add((Positions[^1] - Positions[^2]) / deltatime);
            RotationDerivative.Add((Rotations[^1] * Quaternion.Inverse(Rotations[^2])).eulerAngles / deltatime);
        }
        //����Ʈ�� ������ ������ 600���� ���� �ʰ� ��
        if (Positions.Count > 600 &&Rotations.Count > 600 && Positions.Count == Rotations.Count)
        {
            Positions.RemoveAt(0);
            Rotations.RemoveAt(0);

            PositionDerivative.RemoveAt(0);
            RotationDerivative.RemoveAt(0);
        }
    }
    void Remove(int index)
    {
        PositionDerivative.RemoveAt(index);
        RotationDerivative.RemoveAt(index);
        Positions.RemoveAt(index);
        Rotations.RemoveAt(index);
    }
    //��ġ ���Լ��� �����Ѵ�
    Vector3 IntegralPosition(int a, int b)
    {
        Vector3 result1 = Vector3.zero;
        Vector3 result2 = Vector3.zero;
        if (a >= PositionDerivative.Count || a < 0 || b < 0 || b>= PositionDerivative.Count)
        {
            Debug.Log("������ ���� ����");
            return result1;
        }
        for (int i = 0; i < a; i++)
        {
            result1 += PositionDerivative[i] * deltatime;
        }
        for (int i = 0; i < b; i++)
        {
            result2 += PositionDerivative[i] * deltatime;
        }
        return result2 - result1;
    }

    Vector3 IntegralRotation(int a, int b)
    {
        Vector3 result1 = Vector3.zero;
        Vector3 result2 = Vector3.zero;

        if (a >= PositionDerivative.Count || a < 0 || b < 0 || b >= PositionDerivative.Count)
        {
            Debug.Log("������ ���� ����");
            return result1;
        }
        for (int i = 0; i < a; i++)
        {
            result1 += RotationDerivative[i] * deltatime;
        }
        for (int i = 0; i < b; i++)
        {
            result2 += RotationDerivative[i] * deltatime;
        }
        return result2 - result1;
    }
    async UniTaskVoid Recording()
    {
        while (CurrentState == State.Recording)
        {
            Add(transform.position, transform.rotation);
            await UniTask.Delay(TimeSpan.FromSeconds(deltatime));
        }
    }
    async UniTaskVoid Rewind()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.freezeRotation = true;
        _rb.useGravity = false;
        _currentState = State.Rewind;
        int index = PositionDerivative.Count - 1;
        while (CurrentState == State.Rewind && index > 1)
        {
            transform.position += IntegralPosition(index, index - 1);
            transform.Rotate(IntegralRotation(index, index - 1));
            Remove(index);
            index--;
            await UniTask.Delay(TimeSpan.FromSeconds(deltatime));
        }
        PositionDerivative.Clear();
        RotationDerivative.Clear();
        Positions.Clear();
        Rotations.Clear();
        _rb.useGravity = true;
        _rb.freezeRotation = false;
        CurrentState = State.Recording;
    }
}
