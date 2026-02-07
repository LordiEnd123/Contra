using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Характеристики")]
    public int health = 3;
    public float moveSpeed = 5f;

    [Header("Настройки Типа Врага")]
    public bool destroyOverTime = false; // ГАЛОЧКА: Ставь TRUE для бегунов, FALSE для турелей/босса
    public float lifeTime = 10f; // Сколько живет бегун

    [Header("Активация")]
    public float activationDistance = 20f;

    private bool isActivated = false;
    private Transform player;

    void Start()
    {
        // Ищем игрока
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Игнорируем стену (для всех)
        GameObject wall = GameObject.Find("LeftWall");
        if (wall != null)
        {
            Collider2D myCollider = GetComponent<Collider2D>();
            Collider2D wallCollider = wall.GetComponent<Collider2D>();
            if (myCollider != null && wallCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, wallCollider, true);
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        // ЛОГИКА АКТИВАЦИИ
        if (!isActivated)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < activationDistance)
            {
                WakeUp();
            }
        }
        else
        {
            // ДВИЖЕНИЕ (Только если есть скорость и это не турель)
            // Если moveSpeed > 0, враг идет. Для турелей можно ставить Speed = 0.
            if (moveSpeed > 0)
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            }
        }
    }

    void WakeUp()
    {
        isActivated = true;

        // ВАЖНОЕ ИЗМЕНЕНИЕ:
        // Запускаем таймер смерти ТОЛЬКО если стоит галочка
        if (destroyOverTime)
        {
            Destroy(gameObject, lifeTime);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isActivated) WakeUp();

        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}