using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 10;
    public Rigidbody2D rb;

    // --- ВОТ ЭТОЙ СТРОЧКИ У ТЕБЯ НЕ ХВАТАЛО ---
    public bool isLaser = false;
    // ------------------------------------------

    void Start()
    {
        // Задаем скорость полета
        rb.linearVelocity = transform.right * speed;

        // Удаляем пулю через 3 секунды, чтобы не улетела бесконечно далеко
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. Игнорируем самого игрока, другие пули и бонусы
        if (hitInfo.CompareTag("Player")) return;
        if (hitInfo.GetComponent<Bullet>() != null) return;
        if (hitInfo.GetComponent<PowerUp>() != null) return;

        // 2. Логика попадания во ВРАГА
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            // ЕСЛИ ЭТО ОБЫЧНАЯ ПУЛЯ (не лазер) - она исчезает
            if (isLaser == false)
            {
                Destroy(gameObject);
            }
            // А если лазер (isLaser == true) - код просто идет дальше, пуля не удаляется!
            return;
        }

        // 3. Логика для БОССА (если скрипт Boss существует)
        // Если будет гореть красным - просто удали этот блок пока
        BossController boss = hitInfo.GetComponent<BossController>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            if (isLaser == false) Destroy(gameObject);
            return;
        }

        // 4. Попали в СТЕНУ или ЗЕМЛЮ
        if (hitInfo.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        // Попали во что-то непонятное (ящик, платформа)
        else if (isLaser == false)
        {
            Destroy(gameObject);
        }
    }
}