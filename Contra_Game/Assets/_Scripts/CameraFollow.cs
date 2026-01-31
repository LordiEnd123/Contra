using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // За кем следим (Игрок)
    public float smoothSpeed = 5f; // Насколько плавно (чем больше, тем резче)
    public Vector3 offset = new Vector3(0, 2, -10); // Сдвиг (чтобы камера видела чуть больше спереди/сверху)

    void LateUpdate()
    {
        if (target == null) return;

        // Куда камера хочет попасть
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, offset.z);

        // Плавно перемещаем камеру из текущей точки в нужную
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}