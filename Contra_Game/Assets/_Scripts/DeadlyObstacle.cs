using UnityEngine;

public class DeadlyObstacle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ищем скрипт здоровья
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                // Наносим 1 единицу урона. 
                // Скрипт игрока сам решит: возродить его или закончить игру.
                player.TakeDamage(1);
            }
        }
    }
}