using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;

    bool isThrowing = false;

    //현재 변신해 있는 캐릭터
    public Creature _currentCreature;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SetCreature(_currentCreature);
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _currentCreature.MoveFoward();
        }
        if (Input.GetKey(KeyCode.S))
        {
            _currentCreature.MoveBackward();
        }
        if (Input.GetKey(KeyCode.A))
        {
            _currentCreature.RotateLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            _currentCreature.RotateRight();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentCreature.UseAbillity();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            float anglex = Camera.main.transform.localEulerAngles.x;
            float angley = Camera.main.transform.localEulerAngles.y;


            float x1 = Mathf.Cos(anglex * Mathf.Deg2Rad);
            float y1 = Mathf.Sin(anglex * Mathf.Deg2Rad);

            float x2 = Mathf.Cos(angley * Mathf.Deg2Rad);
            float y2 = Mathf.Sin(angley * Mathf.Deg2Rad);

            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, new Vector3(y2 * x1, -y1, x1 * x2),30);
            Debug.DrawRay(Camera.main.transform.position, new Vector3(y2 * x1, -y1, x1 * x2) * 30, Color.red, 3);
            Vector3 hitPos = Vector3.zero;
            foreach (RaycastHit hit in hits )
            {
                if (hit.collider.CompareTag("Player"))
                    continue;
                hitPos = hit.point;
                break;
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _currentCreature.Jump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //todo : 모자 던지기
            ThrowHat().Forget();
        }
    }
    async UniTaskVoid ThrowHat()
    {
        if (isThrowing)
            return;
        isThrowing = true;
        float duration = 1f;
        float elapsedtime = 0f;

        float anglex = Camera.main.transform.localEulerAngles.x;
        float angley = Camera.main.transform.localEulerAngles.y;

        float x1 = Mathf.Cos(anglex * Mathf.Deg2Rad);
        float y1 = Mathf.Sin(anglex * Mathf.Deg2Rad);

        float x2 = Mathf.Cos(angley * Mathf.Deg2Rad);
        float y2 = Mathf.Sin(angley * Mathf.Deg2Rad);

        Vector3 dir = new Vector3(y2 * x1, -y1, x1 * x2);
        transform.SetParent(null);
        while (elapsedtime < duration)
        {
            if (!isThrowing)
                return;
            transform.position += dir * DataManager.Setting.HatThrowingPower * Time.deltaTime;
            elapsedtime += Time.deltaTime;
            await UniTask.Yield();
        }

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        elapsedtime = 0f;
        while (elapsedtime < duration)
        {
            if (!isThrowing)
                return;
            transform.position = Vector3.Lerp(transform.position, _currentCreature.HatPos.transform.position, elapsedtime / duration);
            elapsedtime += Time.deltaTime;
            await UniTask.Yield();
        }
        transform.position = _currentCreature.HatPos.transform.position;
        transform.SetParent(_currentCreature.HatPos.transform);
        isThrowing = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        Creature creature = other.GetComponent<Creature>();

        if (creature == null)
            return;
        if (creature == _currentCreature)
            return;
        SetCreature(creature);
    }
    void SetCreature(Creature _creature)
    {
        _currentCreature = _creature;
        transform.SetParent(_currentCreature.HatPos.transform);
        transform.position = _currentCreature.HatPos.transform.position;
        transform.rotation = Quaternion.identity;
        Camera.main.GetComponent<PlayerCamera>().target = _currentCreature.transform;
        isThrowing = false;
    }
}
