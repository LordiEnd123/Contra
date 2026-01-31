using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    // Новая настройка: включен ли дробовик?
    public bool isSpreadGun = false;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Если включен режим Spread Gun (Дробовик)
        if (isSpreadGun)
        {
            // Создаем 3 пули с разным поворотом (0, +15 градусов, -15 градусов)
            CreateBullet(0);
            CreateBullet(15);
            CreateBullet(-15);
        }
        else
        {
            // Обычный режим - одна пуля прямо
            CreateBullet(0);
        }
    }

    // Вспомогательная функция, чтобы не писать одно и то же 3 раза
    void CreateBullet(float angleOffset)
    {
        // Берем текущий поворот дула
        Quaternion rotation = firePoint.rotation;

        // Добавляем к нему угол (поворачиваем пулю)
        // Euler(0, 0, angle) поворачивает по оси Z (как стрелку часов)
        rotation *= Quaternion.Euler(0, 0, angleOffset);

        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, rotation);

        // Твой фикс для поворота влево (оставляем как было)
        if (transform.localScale.x < 0)
        {
            newBullet.transform.Rotate(0f, 180f, 0f);
        }
    }

    // Метод, который вызовет коробка с бонусом, чтобы включить этот режим
    public void ActivateSpreadGun()
    {
        isSpreadGun = true;
    }
}