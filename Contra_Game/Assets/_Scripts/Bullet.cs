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
        // Проверяем: "А не попал ли я во врага?"
        Enemy enemy = hitInfo.GetComponent<Enemy>();

        // Если скрипт Enemy найден на том, во что мы попали...
        if (enemy != null)
        {
            enemy.TakeDamage(1); // ...наносим 1 урон
        }

        // Уничтожаем пулю в любом случае (даже если попали в стену)
        Destroy(gameObject);
    }
}