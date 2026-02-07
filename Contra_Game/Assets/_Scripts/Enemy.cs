using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3; // Сколько попаданий выдержит

    // --- ДОБАВИЛ ЭТОТ МЕТОД ---
    void Start()
    {
        // 1. Игнорируем левую стену (чтобы пробегать сквозь неё)
        GameObject wall = GameObject.Find("LeftWall"); // Ищем стену по имени
        if (wall != null)
        {
            // Находим коллайдеры на себе и на стене
            Collider2D myCollider = GetComponent<Collider2D>();
            Collider2D wallCollider = wall.GetComponent<Collider2D>();

            // Если оба коллайдера есть — говорим им "не замечать друг друга"
            if (myCollider != null && wallCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, wallCollider, true);
            }
        }

        // 2. Таймер жизни: враг исчезнет сам через 10 секунд
        // Это нужно, чтобы игра не тормозила от кучи убежавших врагов
        Destroy(gameObject, 10f);
    }
    // ---------------------------

    public void TakeDamage(int damage)
    {
        Debug.Log("Враг получил урон! Осталось жизней: " + (health - damage));
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Тут потом можно добавить анимацию взрыва (Instantiate(explosion...))
        Destroy(gameObject);
    }
}