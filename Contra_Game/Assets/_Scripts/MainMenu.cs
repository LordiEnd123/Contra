using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // "ÈÃĞÀÒÜ"
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // "ÂÛÕÎÄ"
    public void QuitGame()
    {
        Debug.Log("ÈÃĞÀ ÇÀÊĞÛËÀÑÜ!");
        Application.Quit();
    }
}