using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA;
    public Transform posB;
    public float speed = 3f;

    private Vector3 targetPos;

    void Start()
    {
        targetPos = posB.position;
    }

    void Update()
    {
        // Едем к цели
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Если приехали — меняем цель
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            if (targetPos == posB.position) targetPos = posA.position;
            else targetPos = posB.position;
        }
    }

    // Когда Игрок наступает на платформу
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем по тегу, точно ли это Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Делаем игрока "пассажиром" платформы
            collision.transform.SetParent(transform);
        }
    }

    // Когда Игрок спрыгивает
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}