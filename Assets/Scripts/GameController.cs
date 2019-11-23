using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject Exit;
    public List<string> Levels;
    public AudioSource DoorOpen;
    public AudioSource Ambience;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Exit.SetActive(false);
        Ambience.Play();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] getCount = GameObject.FindGameObjectsWithTag("Enemy");
        int count = getCount.Length;

        if(count == 0 && !Exit.activeSelf)
        {
            DoorOpen.Play();
            Exit.SetActive(true);
        }
    }
}
