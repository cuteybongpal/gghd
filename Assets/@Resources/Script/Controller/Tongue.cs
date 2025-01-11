using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    SpringJoint _joint;
    Vector3 _originPos;
    Rigidbody _rb;
    
    
    public void shoot(Vector3 dir)
    {
        _originPos = transform.position;
        _rb = GetComponent<Rigidbody>();

        float angley = Mathf.Asin(dir.y);
        float anglex = Mathf.Asin(dir.z);

        transform.rotation = Quaternion.Euler(angley * Mathf.Rad2Deg, anglex * Mathf.Rad2Deg, 0);
        _rb.velocity = dir * DataManager.GameData.FrogData.TongueSpeed;

    }
    void Attach()
    {

    }
}
