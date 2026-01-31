using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3; // Сколько попаданий выдержит (3 жизни)

    public void TakeDamage(int damage)
    {
        health -= damage; // Отнимаем жизнь

        // Если жизни кончились
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Тут потом можно добавить анимацию взрыва
        Destroy(gameObject); // Просто удаляем объект
    }
}