using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f; // Скорость бега
    public float activationDistance = 15f; // С какого расстояния начинать бежать
    private Transform player;
    private bool isActivated = false; // По умолчанию он спит

    void Start()
    {
        // 1. Находим игрока, чтобы знать до кого мерить расстояние
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // 2. Сразу говорим врагу игнорировать левую стену, чтобы он не застревал, когда начнет бежать
        GameObject wall = GameObject.Find("LeftWall");
        if (wall != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), wall.GetComponent<Collider2D>(), true);
        }
    }

    void Update()
    {
        // Если враг еще "спит"
        if (!isActivated)
        {
            if (player == null) return; // Если игрока нет, спим дальше

            // Проверяем дистанцию
            float distance = Vector2.Distance(transform.position, player.position);

            // Если игрок подошел близко, то просыпаемся
            if (distance < activationDistance)
            {
                isActivated = true;
            }
            else
            {
                return; // Выходим из Update, код движения ниже не выполняется
            }
        }

        // Всегда двигаться влево
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    // Если враг упал в пропасть удаляем его
    void FixedUpdate()
    {
        if (transform.position.y < -9)
        {
            Debug.LogWarning("Враг опасно низко! Высота: " + transform.position.y);
        }

        if (transform.position.y < -50)
        {
            Debug.LogError("ПРИЧИНА СМЕРТИ: Сработал скрипт падения в бездну (Y < -50)");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Враг врезался в: " + collision.gameObject.name);

        if (collision.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(1);
        }
    }

    // Рисуем круг в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
}