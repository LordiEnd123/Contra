using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // НУЖНО ДЛЯ ПЕРЕХОДА НА УРОВЕНЬ 2

public class BossController : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 1000f;
    public float activationDistance = 15f; // С какого расстояния появляется полоска
    public string nextLevelName = "Level2"; // Имя следующего уровня

    private float currentHealth;
    private Transform player; // Чтобы знать, где игрок
    private bool isBossActive = false;

    [Header("UI Link")]
    public Slider healthBar;
    public GameObject winText;

    void Start()
    {
        currentHealth = maxHealth;

        // 1. Ищем игрока по тегу (Убедись, что у игрока тег Player!)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // 2. Сразу ПРЯЧЕМ все элементы интерфейса
        if (healthBar != null) healthBar.gameObject.SetActive(false);
        if (winText != null) winText.SetActive(false);

        // Настраиваем слайдер (даже если он скрыт)
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        // Если босс еще спал, проверяем, не подошел ли игрок
        if (!isBossActive && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            // Если игрок ближе чем 15 метров -> ПРОСЫПАЕМСЯ
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
        // Если в нас выстрелили издалека - тоже активируемся
        if (!isBossActive) ActivateBoss();

        currentHealth -= damage;

        if (healthBar != null) healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("БОСС ПОВЕРЖЕН!");

        // 1. Показываем "ПОБЕДА"
        if (winText != null) winText.SetActive(true);

        // 2. ОТЦЕПЛЯЕМ ТОЛЬКО ИГРОКА (Аккуратно!)
        if (player != null)
        {
            player.SetParent(null); // Игрок становится сам по себе
        }

        // 3. А теперь выключаем Босса целиком
        // Это автоматически отключит и картинку, и коллайдер, И ВСЕ ТУРЕЛИ внутри
        gameObject.SetActive(false);

        // 4. Таймер перехода
        Invoke("LoadNextLevel", 4f);
    }

    void LoadNextLevel()
    {
        // Грузим сцену с именем "Level2"
        SceneManager.LoadScene(nextLevelName);
    }
}