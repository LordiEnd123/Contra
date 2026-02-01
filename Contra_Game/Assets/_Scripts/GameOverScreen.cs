using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Кнопка "Попробовать снова"
    public void RestartLevel()
    {
        // Загружаем сцену с уровнем (она у нас под индексом 1 в Build Settings)
        // Или можно использовать SceneManager.LoadScene("SampleScene");
        SceneManager.LoadScene(1);
    }

    // Кнопка "В меню"
    public void GoToMenu()
    {
        // Загружаем меню (индекс 0)
        SceneManager.LoadScene(0);
    }
}