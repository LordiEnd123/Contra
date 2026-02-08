using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;

    [Header("Настройки Времени")]
    public float bonusDuration = 10f; // Сколько секунд работает бонус
    private float currentBonusTimer;  // Таймер обратного отсчета

    [Header("Префабы пуль")]
    public GameObject bulletPrefab; // Обычная
    public GameObject laserPrefab;  // Лазер (L)
    public GameObject firePrefab;   // Фаербол (F)

    // Режимы стрельбы
    private bool isSpreadGun = false;
    private bool isMachineGun = false;
    private bool isLaserGun = false;
    private bool isFireGun = false;

    private float timeBetweenShots;
    public float startTimeBetweenShots; // Задержка для обычного оружия

    void Update()
    {
        // Логика бонуса
        if (currentBonusTimer > 0)
        {
            currentBonusTimer -= Time.deltaTime; // Отнимаем время

            if (currentBonusTimer <= 0)
            {
                ResetWeapons(); // Сбрасываем оружие
                Debug.Log("Время бонуса истекло! Возврат к обычной пушке.");
            }
        }
        // -----------------------------

        // Таймер стрельбы
        if (timeBetweenShots <= 0)
        {
            // Пулемёт
            if (isMachineGun && Input.GetButton("Fire1"))
            {
                Shoot(bulletPrefab, 0);
                timeBetweenShots = 0.1f;
            }
            // Всё остальное
            else if (Input.GetButtonDown("Fire1"))
            {
                ShootLogic(); // Выбираем, чем стрелять
                timeBetweenShots = startTimeBetweenShots;
            }
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    // Логика выбора пули
    void ShootLogic()
    {
        if (isSpreadGun)
        {
            // Дробовик
            SpawnBullet(bulletPrefab, 0);
            SpawnBullet(bulletPrefab, 15);
            SpawnBullet(bulletPrefab, -15);
        }
        else if (isLaserGun)
        {
            SpawnBullet(laserPrefab, 0);
        }
        else if (isFireGun)
        {
            SpawnBullet(firePrefab, 0);
        }
        else
        {
            // Обычная стрельба
            SpawnBullet(bulletPrefab, 0);
        }
    }

    // Вспомогательный метод для создания пули
    void SpawnBullet(GameObject prefab, float angleOffset)
    {
        // Если стрелять нечем, выходим, чтобы не было ошибок
        if (prefab == null) return;

        GameObject newBullet = Instantiate(prefab, firePoint.position, firePoint.rotation);

        // Поворот для дробовика
        if (angleOffset != 0) newBullet.transform.Rotate(0, 0, angleOffset);

        // Разворот пули
        if (transform.localScale.x < 0)
        {
            newBullet.transform.Rotate(0, 180, 0);
        }
    }

    // Специальный метод для Пулемета
    void Shoot(GameObject prefab, float angleOffset)
    {
        SpawnBullet(prefab, angleOffset);
    }


    public void ActivateSpreadGun()
    {
        ResetWeapons();
        isSpreadGun = true;
        currentBonusTimer = bonusDuration;
    }

    public void ActivateMachineGun()
    {
        ResetWeapons();
        isMachineGun = true;
        currentBonusTimer = bonusDuration;
    }

    public void ActivateLaserGun()
    {
        ResetWeapons();
        isLaserGun = true;
        currentBonusTimer = bonusDuration;
    }

    public void ActivateFireGun()
    {
        ResetWeapons();
        isFireGun = true;
        currentBonusTimer = bonusDuration;
    }

    // Сброс всего в обычное оружие
    void ResetWeapons()
    {
        isSpreadGun = false;
        isMachineGun = false;
        isLaserGun = false;
        isFireGun = false;
        currentBonusTimer = 0;
    }
}