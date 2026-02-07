using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Сюда положим пулю
    public Transform firePoint;     // Сюда положим точку выстрела

    public float timeBetweenShots = 2f; // Пауза между выстрелами
    private float nextShotTime;

    void Update()
    {
        // Проверяем, пришло ли время стрелять
        if (Time.time > nextShotTime)
        {
            Shoot();
            // Ставим таймер на следующий выстрел
            nextShotTime = Time.time + timeBetweenShots;
        }
    }

    void Shoot()
    {
        // Создаем пулю в точке firePoint с поворотом firePoint
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}