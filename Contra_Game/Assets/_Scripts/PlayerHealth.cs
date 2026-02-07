using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3; // Жизни
    public Image[] hearts; // Сердечки UI
    public Transform currentCheckpoint; // Точка возрождения

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
        // 1. Переносим игрока на точку возрождения (если она есть)
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }
        else
        {
            // Если чекпоинт еще не взят, кидаем в начало координат (на всякий случай)
            transform.position = Vector3.zero;
        }

        // 2. Сбрасываем инерцию (чтобы не летел после телепорта)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // 3. --- ВОТ ЭТО ГЛАВНОЕ: СБРОС КАМЕРЫ ---
        // Находим главную камеру и вызываем наш новый метод ResetCamera()
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
        {
            cam.ResetCamera();
        }
        // ----------------------------------------

        Debug.Log("Игрок возродился, камера сброшена!");
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
        SceneManager.LoadScene("GameOver"); // Убедись, что сцена добавлена в Build Settings
    }
}