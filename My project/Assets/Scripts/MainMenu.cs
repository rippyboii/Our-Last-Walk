using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Oyun sahnenin Build Settings'teki tam adi
    [SerializeField] private string gameSceneName = "SampleScene";

    // Start butonu bunu cagiracak
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Quit butonu bunu cagiracak
    public void QuitGame()
    {
        Debug.Log("Quit pressed");
        Application.Quit();
    }
}