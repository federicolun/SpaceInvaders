using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPause;
    private bool gamePaused;

    private void Start()
    {
        menuPause.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }     
    }

    public void GameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

    public void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        menuPause.SetActive(true);
    }

    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        menuPause.SetActive(false);
    }

}
