using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody _rb;
    bool _isShoot = false;
    public void Fire(Vector3 dir, float speed)
    {
        _rb = GetComponent<Rigidbody>();

        transform.LookAt(transform.position + dir);
        transform.Rotate(90, 0, 0);
        _rb.velocity = dir * speed;
        _isShoot = true;
        CheckDistance(transform.position).Forget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isShoot)
            return;
        if (other.CompareTag("Player"))
            return;
        if (other.CompareTag("Hat"))
            return;
        if (other.gameObject.layer == 6) 
            return;
        if (other.CompareTag("BreakableObject"))
        {
            Destroy(other.gameObject);
        }
        CommandManager.Instance.ExecuteCommand<Bullet>(new DeSpawn<Bullet>(this));
        _isShoot = false;
    }
    async UniTaskVoid CheckDistance(Vector3 originPos)
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, originPos) >= DataManager.GameData.GunData.BulletRange)
                break;
            await UniTask.Yield();
        }
        if (_isShoot)
        {
            CommandManager.Instance.ExecuteCommand<Bullet>(new DeSpawn<Bullet>(this));
            _isShoot= false;
        }
    }
}
