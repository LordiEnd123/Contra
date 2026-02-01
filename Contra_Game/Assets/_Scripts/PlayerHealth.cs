using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3; // Это наши ЖИЗНИ
    public Image[] hearts; // Сердечки UI

    // НОВОЕ: Сюда мы перетащим наш RespawnPoint
    public Transform currentCheckpoint;

    void Start()
    {
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHeartsUI();

        if (health > 0)
        {
            // ЖИЗНИ ЕСТЬ: Возрождаемся
            Respawn();
        }
        else
        {
            // ЖИЗНЕЙ НЕТ: Game Over
            GameOver();
        }
    }

    void Respawn()
    {
        // 1. Переносим игрока на точку возрождения
        transform.position = currentCheckpoint.position;

        // 2. (Опционально) Сбрасываем скорость, чтобы он не вылетел по инерции
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        Debug.Log("Игрок потерял жизнь и возродился!");
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) hearts[i].enabled = true;
            else hearts[i].enabled = false;
        }
    }

    void GameOver()
    {
        // Перезагрузка уровня полностью
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}