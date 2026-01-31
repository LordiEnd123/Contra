using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; // Скорость бега

    void Update()
    {
        // Всегда двигаться влево (Vector2.left = x: -1, y: 0)
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    // Если враг упал в пропасть (ниже Y = -10), удаляем его, чтобы не грузить память
    void FixedUpdate()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    // Когда враг сталкивается с чем-то физическим (телом игрока)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, это Игрок?
        if (collision.gameObject.tag == "Player")
        {
            // Пытаемся найти у него скрипт здоровья
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Наносим 1 урон
            }

            // В Contra враги часто не умирают от удара об игрока, но давай сделаем,
            // чтобы враг отталкивал или уничтожался? 
            // Для простоты пока ничего не делаем, враг просто толкает игрока.
        }
    }
}