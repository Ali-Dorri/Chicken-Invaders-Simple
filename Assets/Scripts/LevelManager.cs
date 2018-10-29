using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
     public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadOption()
    {
        SceneManager.LoadScene("Option");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
