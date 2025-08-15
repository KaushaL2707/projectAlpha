using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene loading

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game quit!"); // Only shows in editor
    }
}
