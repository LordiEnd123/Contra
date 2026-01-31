using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // НОВОЕ: Нужно для работы с картинками UI

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;

    // НОВОЕ: Массив, куда мы положим наши картинки сердечек
    [Header("UI Settings")]
    public Image[] hearts;

    // НОВОЕ: При старте проверяем, чтобы количество сердечек на экране
    // совпадало с количеством жизней в настройках.
    void Start()
    {
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // НОВОЕ: Сразу после получения урона обновляем UI
        UpdateHeartsUI();

        if (health <= 0)
        {
            Die();
        }
    }

    // НОВОЕ: Метод, который рисует сердечки
    void UpdateHeartsUI()
    {
        // Пробегаемся по всем картинкам сердечек в массиве
        for (int i = 0; i < hearts.Length; i++)
        {
            // Если номер сердечка меньше текущего здоровья - включаем его.
            // Если больше - выключаем.
            if (i < health)
            {
                hearts[i].enabled = true; // Показываем
            }
            else
            {
                hearts[i].enabled = false; // Скрываем
            }
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}