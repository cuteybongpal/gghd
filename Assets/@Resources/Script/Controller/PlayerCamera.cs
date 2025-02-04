using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float Yaxis;
    public float Xaxis;
    public float offset1;
    public Transform target;//Player

    private float rotSensitive = 3f;//ī�޶� ȸ�� ����
    private float dis = 10f;//ī�޶�� �÷��̾������ �Ÿ�
    private float RotationMin = -80f;//ī�޶� ȸ������ �ּ�
    private float RotationMax = 80f;//ī�޶� ȸ������ �ִ�
    private float smoothTime = 0.12f;//ī�޶� ȸ���ϴµ� �ɸ��� �ð�
    //�� 5���� value�� �������� ���ⲯ �˾Ƽ� ����������
    private Vector3 targetRotation;
    private Vector3 currentVel;

    void Update()//Player�� �����̰� �� �� ī�޶� ���󰡾� �ϹǷ� LateUpdate
    {
        
        Yaxis = Yaxis + Input.GetAxis("Mouse X") * rotSensitive;//���콺 �¿�������� �Է¹޾Ƽ� ī�޶��� Y���� ȸ����Ų��
        Xaxis = Xaxis - Input.GetAxis("Mouse Y") * rotSensitive;//���콺 ���Ͽ������� �Է¹޾Ƽ� ī�޶��� X���� ȸ����Ų��
        //Xaxis�� ���콺�� �Ʒ��� ������(�������� �Է� �޾�����) ���� �������� ī�޶� �Ʒ��� ȸ���Ѵ� 

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);
        //X��ȸ���� �Ѱ�ġ�� �����ʰ� �������ش�.

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis), ref currentVel, smoothTime);
        this.transform.eulerAngles = targetRotation;
        //SmoothDamp�� ���� �ε巯�� ī�޶� ȸ��

        transform.position = target.position - transform.forward * dis + Vector3.up * 2;


        RaycastHit[] hits = Physics.RaycastAll(target.position + Vector3.up * 2, (transform.position - (target.position + Vector3.up * 2)).normalized, dis);
        Vector3 collisionPos = Vector3.zero;

        foreach (RaycastHit hit in hits)
        {
            if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Hat"))
            {
                collisionPos = hit.point;
                break;
            }
        }
        if (collisionPos != Vector3.zero)
        {
            transform.position = collisionPos - (transform.position - (target.position + Vector3.up * 2)).normalized * 0.4f;
        }
        //ī�޶��� ��ġ�� �÷��̾�� ������ ����ŭ �������ְ� ��� ����ȴ�.
    }
}
