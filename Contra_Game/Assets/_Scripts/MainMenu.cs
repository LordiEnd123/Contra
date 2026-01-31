using UnityEngine;
using UnityEngine.SceneManagement; // Нужно для загрузки сцен

public class MainMenu : MonoBehaviour
{
    // Эту функцию мы привяжем к кнопке "ИГРАТЬ"
    public void PlayGame()
    {
        // Загружаем следующую сцену по списку (после меню идет 1 уровень)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Эту функцию мы привяжем к кнопке "ВЫХОД"
    public void QuitGame()
    {
        Debug.Log("ИГРА ЗАКРЫЛАСЬ!"); // В редакторе игра не закроется, поэтому пишем в консоль
        Application.Quit();
    }
}