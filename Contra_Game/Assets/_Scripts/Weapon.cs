using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint; // Откуда вылетает
    public GameObject bulletPrefab; // Что вылетает

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Левая кнопка мыши или Ctrl
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // 1. Создаем пулю и запоминаем её в переменную newBullet
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 2. Проверяем: если игрок (на котором висит этот скрипт) смотрит влево...
        // (То есть его Scale по X меньше нуля)
        if (transform.localScale.x < 0)
        {
            // ...то разворачиваем пулю на 180 градусов по оси Y
            newBullet.transform.Rotate(0f, 180f, 0f);
        }
    }
}