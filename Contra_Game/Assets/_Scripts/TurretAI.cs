using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public Transform gun;         // Ссылка на дуло (которое будет крутиться)
    public Transform firePoint;   // Откуда вылетает
    public GameObject bulletPrefab; // Злая пуля

    public float range = 10f;     // Дальность стрельбы
    public float fireRate = 2f;   // Пауза между выстрелами

    private float nextFireTime = 0f;
    private Transform player;     // За кем следим

    void Start()
    {
        // Находим игрока по тегу автоматически
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // 1. Проверяем дистанцию
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < range)
        {
            // 2. Целимся (Математика поворота)
            Vector2 direction = player.position - gun.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gun.rotation = Quaternion.Euler(0, 0, angle);

            // 3. Стреляем по таймеру
            if (Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    // Рисуем круг радиуса в редакторе, чтобы видеть зону обстрела
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}