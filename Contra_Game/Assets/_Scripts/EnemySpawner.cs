using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Кого спавним")]
    public GameObject enemyPrefab; // Сюда перетащи префаб Бегуна

    [Header("Настройки")]
    public float spawnInterval = 2f;    // Как часто (сек)
    public float activationDistance = 15f; // С какого расстояния начинать
    public bool spawnFacingLeft = true; // ГАЛОЧКА: Если враг должен бежать ВЛЕВО

    [Header("Лимиты")]
    public int maxEnemiesToSpawn = 5;   // Сколько всего врагов выпустит этот спавнер (0 = бесконечно)

    private Transform player;
    private float timer;
    private int spawnedCount = 0;

    void Start()
    {
        // Ищем игрока
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        timer = spawnInterval; // Чтобы первый враг появился не мгновенно, а через паузу
    }

    void Update()
    {
        if (player == null) return;

        // Если лимит исчерпан (и он не бесконечный) — выключаемся
        if (maxEnemiesToSpawn > 0 && spawnedCount >= maxEnemiesToSpawn)
        {
            return; // Больше не работаем
        }

        // 1. Проверяем дистанцию
        float distance = Vector2.Distance(transform.position, player.position);

        // 2. Если игрок близко — тикает таймер
        if (distance < activationDistance)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                SpawnEnemy();
                timer = spawnInterval; // Сброс таймера
            }
        }
    }

    void SpawnEnemy()
    {
        // Создаем врага в позиции спавнера
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // ПОВОРОТ: Если нужно бежать влево, разворачиваем врага на 180 градусов
        if (spawnFacingLeft)
        {
            newEnemy.transform.Rotate(0, 180, 0);
        }

        spawnedCount++;
    }

    // РИСУЕМ ИКОНКУ В РЕДАКТОРЕ (чтобы ты видел, где стоит спавнер)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawWireSphere(transform.position, activationDistance); // Показывает радиус работы
    }
}