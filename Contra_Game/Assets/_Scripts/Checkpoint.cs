using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Говорим игроку: "Теперь твой дом здесь"
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            player.currentCheckpoint = this.transform;

            Debug.Log("Сохранено! Новая точка возрождения.");

            // Отключаем этот чекпоинт, чтобы не срабатывал 100 раз
            GetComponent<Collider2D>().enabled = false;
        }
    }
}