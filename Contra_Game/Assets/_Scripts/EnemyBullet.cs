using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    void Start()
    {
        // Уничтожить через 3 секунды, чтобы не летала вечно
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Летит туда, куда смотрит
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Если попали в Игрока
        if (hitInfo.tag == "Player")
        {
            // Ищем скрипт здоровья игрока
            PlayerHealth player = hitInfo.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject); // Пуля исчезает
        }

        // Если попали в землю, то исчезаем
        if (hitInfo.name == "Ground" || hitInfo.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}