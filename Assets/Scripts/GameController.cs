using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject Door, Exit;
    public List<string> Levels;

    private void Awake()
    {
        Instance = this;
        Door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] getCount = GameObject.FindGameObjectsWithTag("Enemy");
        int count = getCount.Length;

        if(count == 0)
        {
            Exit.SetActive(false);
            Door.SetActive(true);
        }
    }
}
