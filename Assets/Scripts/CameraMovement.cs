using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target; // player 
    private float detectCollider;
    public Vector3 offsetorigin; //Координаты камеры вне колизий (Норм когда X=0, Y=3.6, Z=-8.91)
    public Vector3 offsetnew; //Новые кординаты, которые задаём для конкретного места
    public float damping; //Плавность камеры
    public float rotationDamping; //Плавность поворота
    public bool useOffsetNew = false; //Переменная,в которую ставлю галочку при попадании в коллизию
    private Vector3 velocity = Vector3.zero; //хуй знает что, это было в туториале по лодке
    public float angle; //Угол,под который ставим новую кординату камеры
    public float originAngleX; //оригинальная кордината угла камеры
    public float zDamping; //Плавность кординаты Z для камеры. Нужна для мастабирования, лучше ставить на 7

    public bool freezeCameraX;
    public bool freezeCameraY;
    public bool freezeCameraZ;

    Vector3 movePosition;

    void FixedUpdate()
    {
        if (useOffsetNew == true) //Если стоит галочка на новом оффсете, то меняем их на те, что прописаны в свойствах триггера
        {

            FreezeAxises(offsetnew);

            transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping, zDamping); //Здесь есть ZDamping, который не понятно почему, но работает
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(angle, 0, 0), rotationDamping); //Позволяет менять поворот камеры
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
