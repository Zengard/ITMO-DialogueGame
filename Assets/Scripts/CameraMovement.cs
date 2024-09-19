using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target; // player 
    private float detectCollider;
    public Vector3 offsetorigin; //���������� ������ ��� ������� (���� ����� X=0, Y=3.6, Z=-8.91)
    public Vector3 offsetnew; //����� ���������, ������� ����� ��� ����������� �����
    public float damping; //��������� ������
    public float rotationDamping; //��������� ��������
    public bool useOffsetNew = false; //����������,� ������� ������ ������� ��� ��������� � ��������
    private Vector3 velocity = Vector3.zero; //��� ����� ���, ��� ���� � ��������� �� �����
    public float angle; //����,��� ������� ������ ����� ��������� ������
    public float originAngleX; //������������ ��������� ���� ������
    public float zDamping; //��������� ��������� Z ��� ������. ����� ��� ��������������, ����� ������� �� 7

    public bool freezeCameraX;
    public bool freezeCameraY;
    public bool freezeCameraZ;

    Vector3 movePosition;

    void FixedUpdate()
    {
        if (useOffsetNew == true) //���� ����� ������� �� ����� �������, �� ������ �� �� ��, ��� ��������� � ��������� ��������
        {

            FreezeAxises(offsetnew);

            transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping, zDamping); //����� ���� ZDamping, ������� �� ������� ������, �� ��������
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle, 0, 0), rotationDamping); //��������� ������ ������� ������
        }
        else
        {
            FreezeAxises(offsetorigin);
            transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping, zDamping);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(originAngleX, 0, 0), rotationDamping);
        }
    }

    void FreezeAxises(Vector3 offset)
    {
        float moveX = target.position.x + offset.x;
        float moveY = target.position.y + offset.y;
        float moveZ = target.position.z + offset.z;
        if (freezeCameraX == true)
        {
            moveX = movePosition.x;
        }

        if (freezeCameraY == true)
        {
            moveY = movePosition.y;
        }

        if (freezeCameraZ == true)
        {
            moveZ = movePosition.z;
        }

        movePosition = new Vector3(moveX, moveY, moveZ);
    }

}
