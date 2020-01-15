using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AltDoorController : MonoBehaviour
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
            PlayerPrefs.SetString("previousRoom", SceneManager.GetActiveScene().name);

            SceneManager.LoadScene(GameController.Instance.AltRoom, LoadSceneMode.Single);
        }
    }
}