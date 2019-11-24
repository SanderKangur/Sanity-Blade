using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    private string _activeScene;
    private void Awake()
    {
        _activeScene = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {

            int sceneIndex = GameController.Instance.Levels.IndexOf(_activeScene);
            string newScene = GameController.Instance.Levels[sceneIndex + 1];
            SceneManager.LoadScene(newScene, LoadSceneMode.Single);
        }
    }
}