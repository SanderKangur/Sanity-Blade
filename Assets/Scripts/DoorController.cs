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
            SceneManager.LoadScene(GameController.Instance.NextRoom, LoadSceneMode.Single);
        }
    }
}