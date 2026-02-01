using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10; // Урон пули (у Босса 100 жизней, значит нужно 10 попаданий)
    public Rigidbody2D rb;

    void Start()
    {
        // Полет пули
        rb.linearVelocity = transform.right * speed;

        // Уничтожить через 3 секунды, чтобы не засорять память
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. ИГНОР ИГРОКА
        if (hitInfo.CompareTag("Player")) return;

        // 2. ИГНОР ДРУГИХ ПУЛЬ
        if (hitInfo.GetComponent<Bullet>() != null) return;

        // 3. ИГНОР БОНУСОВ
        if (hitInfo.GetComponent<PowerUp>() != null) return;


        // --- ЛОГИКА ПОПАДАНИЙ ---

        // А. Попали в обычного Врага
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Используем переменную damage
        }

        // Б. Попали в БОССА (НОВОЕ!)
        BossController boss = hitInfo.GetComponent<BossController>();
        if (boss != null)
        {
            boss.TakeDamage(damage); // Наносим урон боссу
        }

        // Уничтожаем пулю при попадании во что угодно (враг, босс, стена, земля)
        Destroy(gameObject);
    }
}