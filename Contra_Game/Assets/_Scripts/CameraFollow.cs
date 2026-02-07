using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float offsetZ = -10f;
    public float offsetX = 3f;

    [Header("Границы")]
    public float rightLimit = 145f; // Где камера стопорится у босса

    private float leftLimit; // "Память" камеры

    void Start()
    {
        leftLimit = transform.position.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float desiredX = target.position.x + offsetX;

        // Едем только вправо и не дальше финиша
        if (desiredX > leftLimit && desiredX < rightLimit)
        {
            leftLimit = desiredX;
            transform.position = new Vector3(leftLimit, transform.position.y, offsetZ);
        }
    }

    // Добавь этот метод в CameraFollow.cs
    public void ResetCamera()
    {
        if (target == null) return;

        // 1. Вычисляем, где должна стоять камера рядом с игроком
        float newX = target.position.x + offsetX;

        // 2. Моментально переносим камеру туда
        transform.position = new Vector3(newX, transform.position.y, offsetZ);

        // 3. Самое важное: сбрасываем "память" левой границы
        leftLimit = newX;
    }


}