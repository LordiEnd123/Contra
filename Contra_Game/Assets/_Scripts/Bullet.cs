using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;

    void Start() // ����������� ��� �������� ����
    {
        // ����� ����, ���� ������� ���� (������)
        rb.linearVelocity = transform.right * speed;

        // ������� ���� ����� 3 �������, ���� ��� ������ �� ������
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. ИГНОР ИГРОКА: Если пуля задела того, кто её выпустил - летим дальше
        if (hitInfo.CompareTag("Player"))
        {
            return;
        }

        // 2. ИГНОР ДРУГИХ ПУЛЬ: Если пуля задела другую пулю (у которой тоже есть скрипт Bullet)
        if (hitInfo.GetComponent<Bullet>() != null)
        {
            return;
        }

        // 3. ИГНОР БОНУСОВ: Чтобы пуля не взрывалась об коробку с бонусом
        if (hitInfo.GetComponent<PowerUp>() != null)
        {
            return;
        }

        // --- ДАЛЬШЕ СТАРАЯ ЛОГИКА ---

        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(1);
        }

        // Уничтожаем пулю, только если прошли все проверки выше (значит врезались в стену или врага)
        Destroy(gameObject);
    }
}