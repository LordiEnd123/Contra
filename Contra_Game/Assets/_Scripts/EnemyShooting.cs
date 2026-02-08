using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Настройки")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float timeBetweenShots = 2f;

    [Header("Дистанция Атаки")]
    public float activationDistance = 10f; // Добавь такую же цифру, как в Enemy

    private float nextFireTime;
    private Transform player;

    void Start()
    {
        // Находим игрока
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        // 1. Проверяем расстояние до игрока
        float distance = Vector2.Distance(transform.position, player.position);

        // 2. Если мы СЛИШКОМ ДАЛЕКО — выходим, не стреляем
        if (distance > activationDistance)
        {
            return;
        }

        // 3. Если близко — стреляем по таймеру
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + timeBetweenShots;
            Shoot();
        }
    }

    void Shoot()
    {
        if (firePoint != null && bulletPrefab != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}