using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;

    [Header("Настройки Времени")]
    public float bonusDuration = 10f; // Сколько секунд работает бонус (строй в Инспекторе)
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
        // --- ЛОГИКА ТАЙМЕРА БОНУСА ---
        if (currentBonusTimer > 0)
        {
            currentBonusTimer -= Time.deltaTime; // Отнимаем время

            if (currentBonusTimer <= 0)
            {
                ResetWeapons(); // Время вышло! Сбрасываем оружие
                Debug.Log("Время бонуса истекло! Возврат к обычной пушке.");
            }
        }
        // -----------------------------

        // Таймер стрельбы
        if (timeBetweenShots <= 0)
        {
            // ПУЛЕМЕТ (Авто-огонь: держим кнопку)
            if (isMachineGun && Input.GetButton("Fire1"))
            {
                Shoot(bulletPrefab, 0); // Пулемет стреляет обычными пулями
                timeBetweenShots = 0.1f;
            }
            // ВСЕ ОСТАЛЬНЫЕ (Одиночный клик)
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
            // Дробовик (3 пули веером)
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
            // Обычная стрельба (если нет бонусов)
            SpawnBullet(bulletPrefab, 0);
        }
    }

    // Вспомогательный метод для создания пули
    void SpawnBullet(GameObject prefab, float angleOffset)
    {
        // Если стрелять нечем (забыл положить префаб), выходим, чтобы не было ошибок
        if (prefab == null) return;

        GameObject newBullet = Instantiate(prefab, firePoint.position, firePoint.rotation);

        // Поворот (для дробовика)
        if (angleOffset != 0) newBullet.transform.Rotate(0, 0, angleOffset);

        // РАЗВОРОТ ПУЛИ (если игрок смотрит влево через Scale)
        if (transform.localScale.x < 0)
        {
            newBullet.transform.Rotate(0, 180, 0);
        }
    }

    // Специальный метод для Пулемета (чтобы стрелять из Update)
    void Shoot(GameObject prefab, float angleOffset)
    {
        SpawnBullet(prefab, angleOffset);
    }

    // --- Методы включения (их вызывает PowerUp) ---

    public void ActivateSpreadGun()
    {
        ResetWeapons();
        isSpreadGun = true;
        currentBonusTimer = bonusDuration; // ЗАПУСКАЕМ ТАЙМЕР
    }

    public void ActivateMachineGun()
    {
        ResetWeapons();
        isMachineGun = true;
        currentBonusTimer = bonusDuration; // ЗАПУСКАЕМ ТАЙМЕР
    }

    public void ActivateLaserGun()
    {
        ResetWeapons();
        isLaserGun = true;
        currentBonusTimer = bonusDuration; // ЗАПУСКАЕМ ТАЙМЕР
    }

    public void ActivateFireGun()
    {
        ResetWeapons();
        isFireGun = true;
        currentBonusTimer = bonusDuration; // ЗАПУСКАЕМ ТАЙМЕР
    }

    // Сброс всего в "Обычный режим"
    void ResetWeapons()
    {
        isSpreadGun = false;
        isMachineGun = false;
        isLaserGun = false;
        isFireGun = false;
        currentBonusTimer = 0; // Обнуляем таймер
    }
}