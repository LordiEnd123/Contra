using UnityEngine;
using UnityEngine.UI; // ОБЯЗАТЕЛЬНО: Нужно для работы UI
using UnityEngine.SceneManagement; // Нужно, если хочешь переход на следующий уровень

public class FlyingBoss : MonoBehaviour
{
    [Header("Характеристики")]
    public float speed = 3f;
    public int damage = 1;
    public float maxHealth = 20f; // Жизни (float для слайдера удобнее)

    private float currentHealth;

    [Header("Дистанция")]
    public float activationDistance = 15f;

    [Header("UI (Интерфейс)")]
    public Slider healthBar;   // Сюда перетащи Слайдер
    public GameObject winText; // Сюда перетащи Текст победы
    public string nextLevelName = "MainMenu"; // Куда переходим после победы

    private Transform player;
    private bool isActivated = false;

    void Start()
    {
        currentHealth = maxHealth;

        // Ищем игрока
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // --- НАСТРОЙКА UI ---
        // 1. Скрываем UI в начале уровня
        if (healthBar != null) healthBar.gameObject.SetActive(false);
        if (winText != null) winText.SetActive(false);

        // 2. Настраиваем значения слайдера
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        if (player == null) return;

        // ЛОГИКА АКТИВАЦИИ
        if (!isActivated)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            // Если игрок подошел — активируем босса
            if (distance < activationDistance)
            {
                ActivateBoss();
            }
        }
        else
        {
            // ПРЕСЛЕДОВАНИЕ
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // ПОВОРОТ ЛИЦА
            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void ActivateBoss()
    {
        isActivated = true;
        // Показываем полоску жизни, когда босс проснулся
        if (healthBar != null) healthBar.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // Если стрельнули издалека — будим
        if (!isActivated) ActivateBoss();

        currentHealth -= damageAmount;

        // Обновляем слайдер
        if (healthBar != null) healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Показываем победу
        if (winText != null) winText.SetActive(true);

        // 2. Прячем полоску жизни
        if (healthBar != null) healthBar.gameObject.SetActive(false);

        // 3. Выключаем босса визуально (исчезает), но скрипт еще работает
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // 4. Через 4 секунды идем в меню (или следующий уровень)
        Invoke("LoadNextLevel", 4f);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }
}