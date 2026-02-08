using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Кнопка "Попробовать снова"
    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    // Кнопка "В меню"
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}