using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    [Header("Настройки Босса")]
    public float maxHealth = 100f; // Сделал 100, для проверки проще
    public float activationDistance = 15f; // Дистанция пробуждения
    public string nextLevelName = "Level2"; // Убедись, что сцена так называется

    private float currentHealth;
    private Transform player;
    private bool isBossActive = false; // Спит или нет

    [Header("Связи (UI и Компоненты)")]
    public Slider healthBar; // Ссылка на слайдер жизни
    public GameObject winText; // Текст "YOU WIN"
    public GameObject visualRoot; // (Необязательно) Если у босса много частей, кинь сюда родителя графики

    void Start()
    {
        currentHealth = maxHealth;

        // 1. Ищем игрока
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // 2. Прячем полоску жизни и текст победы в начале
        if (healthBar != null) healthBar.gameObject.SetActive(false);
        if (winText != null) winText.SetActive(false);

        // Настраиваем слайдер
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        // Если босс еще спит...
        if (!isBossActive && player != null)
        {
            // Меряем расстояние
            float distance = Vector2.Distance(transform.position, player.position);

            // Если подошли близко -> БУДИМ
            if (distance < activationDistance)
            {
                ActivateBoss();
            }
        }
    }

    void ActivateBoss()
    {
        isBossActive = true;

        // Показываем полоску жизни
        if (healthBar != null) healthBar.gameObject.SetActive(true);

        Debug.Log("БОСС: Я ВИЖУ ТЕБЯ! БИТВА НАЧАЛАСЬ!");
    }

    public void TakeDamage(int damage)
    {
        // ГЛАВНОЕ ИСПРАВЛЕНИЕ:
        // Если босс спит — урон не проходит!
        if (!isBossActive) return;

        currentHealth -= damage;

        // Обновляем слайдер
        if (healthBar != null) healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("БОСС ПОВЕРЖЕН!");

        // 1. Показываем победный текст
        if (winText != null) winText.SetActive(true);

        // 2. Прячем слайдер
        if (healthBar != null) healthBar.gameObject.SetActive(false);

        // 3. "Удаляем" босса визуально, но НЕ выключаем скрипт
        // Выключаем коллайдер, чтобы пули пролетали сквозь труп
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Выключаем картинку (SpriteRenderer)
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr != null) spr.enabled = false;

        // Выключаем всех детей (пушки, турели), если они есть внутри
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // 4. Запускаем таймер перехода (4 секунды)
        Invoke("LoadNextLevel", 4f);
    }

    void LoadNextLevel()
    {
        Debug.Log("Загрузка уровня: " + nextLevelName);
        // Грузим сцену. Убедись, что добавил Level2 в File -> Build Settings!
        SceneManager.LoadScene(nextLevelName);
    }
}