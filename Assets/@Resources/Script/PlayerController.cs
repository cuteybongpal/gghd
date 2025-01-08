using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int MaxHp;
    public int CurrentHp;
    public float Speed;

    Rigidbody _rb;
    CatchableObject _catchObject;
    CatchableObject _currentCatChObject;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * Speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -45 * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 45 * Time.deltaTime, 0));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_catchObject == null)
                return;
            if (_currentCatChObject == null)
            {
                if (_catchObject.Catch())
                {
                    _currentCatChObject = _catchObject;
                }
            }
            else
            {
                _currentCatChObject.Throw();
                _currentCatChObject = null;
            }
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
            _rb.velocity = Vector3.up * 5;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CatchableObject"))
        {
            _catchObject = other.GetComponent<CatchableObject>();
            _catchObject.Player = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CatchableObject"))
        {
            _catchObject = other.GetComponent<CatchableObject>();
            _catchObject.Player = null;
            _catchObject = null;
            _currentCatChObject = null;
        }
    }
}
