using UnityEngine;
using UnityEngine.UI; // Обязательно! Нужно для работы с UI

public class BossController : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Link")]
    public Slider healthBar; // Сюда перетащим наш слайдер
    public GameObject winText; // Текст "YOU WIN", если захочешь

    void Start()
    {
        currentHealth = maxHealth;

        // --- ЖЕЛЕЗНОЕ ВЫКЛЮЧЕНИЕ ТЕКСТА ---
        // Даже если ты забыл убрать галочку, этот код её уберет сам.
        if (winText != null)
        {
            winText.SetActive(false);
        }

        // Настраиваем слайдер при старте
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    // Этот метод нужно вызывать из Пули (так же, как у обычных врагов)
    // Но так как у тебя пуля ищет "EnemyHealth", нам нужно быть хитрее.
    // (См. объяснение ниже)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Обновляем полоску
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("БОСС ПОВЕРЖЕН!");

        // 1. Показываем победу
        if (winText != null) winText.SetActive(true);

        // 2. Вместо удаления просто выключаем картинку и коллайдер
        // (чтобы звук или скрипты успели доработать, и игрок не исчез)
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Отключаем турели (детей)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // Если хочешь удалить совсем, делай это с задержкой, 
        // предварительно "отцепив" игрока, но пока хватит и выключения.
        // Destroy(gameObject, 2f); 
    }
}