using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Oyun sahnenin Build Settings'teki tam adi
    [SerializeField] private string gameSceneName = "SampleScene";

    void Start()
    {
        // Oyun basladiginda imleci goster
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

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