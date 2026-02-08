using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public float range = 10f;       // Радиус зрения
    public float fireRate = 1f;     // Пауза между выстрелами
    public GameObject bulletPrefab; // Префаб пули
    public Transform firePoint;     // Откуда вылетает
    public Transform gunPart;       // Крутящаяся часть (Gun)

    private Transform player;
    private float nextFireTime;

    void Start()
    {
        // Ищем игрока по тегу
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // 1. Проверяем дистанцию
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist < range)
        {
            // 2. Поворачиваем пушку на игрока
            Vector2 direction = player.position - gunPart.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            gunPart.rotation = Quaternion.Euler(0, 0, angle);

            // 3. Стреляем
            if (Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    // Рисуем радиус в редакторе, чтобы было удобно настраивать
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}