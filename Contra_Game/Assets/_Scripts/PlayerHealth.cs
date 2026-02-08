using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Здоровье")]
    public int health = 3;
    public Image[] hearts;
    public Transform currentCheckpoint;

    [Header("Неуязвимость")]
    public float invulnerabilityDuration = 2f; // Сколько секунд длится бессмертие
    public float flashSpeed = 0.1f; // Как быстро мигать

    private bool isInvulnerable = false; // Сейчас бессмертен?
    private SpriteRenderer spriteRenderer; // Чтобы мигать картинкой
    private Collider2D myCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        // 1. ЕСЛИ БЕССМЕРТЕН — ВЫХОДИМ, УРОН НЕ ПРОХОДИТ
        if (isInvulnerable) return;

        health -= damage;
        UpdateHeartsUI();

        if (health > 0)
        {
            Respawn();
        }
        else
        {
            GameOver();
        }
    }

    void Respawn()
    {
        // 1. Телепортация
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }
        else
        {
            transform.position = Vector3.zero;
        }

        // 2. Сброс физики
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // 3. Сброс камеры
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null) cam.ResetCamera();

        // 4. ЗАПУСКАЕМ НЕУЯЗВИМОСТЬ
        StartCoroutine(BecomeInvulnerable());

        Debug.Log("Возрождение с щитом!");
    }

    IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;

        // Цикл мигания
        for (float i = 0; i < invulnerabilityDuration; i += flashSpeed)
        {
            // Выключаем картинку
            if (spriteRenderer != null) spriteRenderer.enabled = false;

            yield return new WaitForSeconds(flashSpeed / 2);

            // Включаем картинку
            if (spriteRenderer != null) spriteRenderer.enabled = true;

            yield return new WaitForSeconds(flashSpeed / 2);
        }

        // В конце обязательно включаем всё обратно
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        isInvulnerable = false;
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
        SceneManager.LoadScene("GameOver");
    }
}