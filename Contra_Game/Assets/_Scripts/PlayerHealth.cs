using UnityEngine;
using UnityEngine.SceneManagement; // Нужно для перезагрузки сцены

public class PlayerHealth : MonoBehaviour
{
    public int health = 3; // 3 жизни

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Ай! Осталось жизней: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Перезагружаем текущую сцену (как будто рестарт)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}