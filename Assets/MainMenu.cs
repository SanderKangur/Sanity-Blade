using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject nextLevel;
    private void Update()
    {
        Time.timeScale = 1f;
        if (PlayerPrefs.GetInt("unlocked") == 1)
        {
            nextLevel.SetActive(true);
        }
        else
        {
            nextLevel.SetActive(false);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlayerPrefs.DeleteAll();

    }

    public void PlayGameX()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 17);
        PlayerPrefs.DeleteAll();
    }


    public void QuitGame()
    {
        Debug.Log("Quit the game");
        Application.Quit(0);

    }
}
