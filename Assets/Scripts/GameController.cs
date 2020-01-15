using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject Exit;
    public GameObject AltExit;
    public string NextRoom;
    public string AltRoom;

    public AudioSource DoorOpen;
    public AudioSource Ambience;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Exit.SetActive(false);
        AltExit.SetActive(false);
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
            AltExit.SetActive(true);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("Level " + level);
        if (level > 1)
        {
            PlayerInfo info = new PlayerInfo();
            PlayerController player = PlayerController.Instance;
            info.Health = player.Health;
            info.Speed = player.Speed;
            info.FireRate = player.FireRate;
            info.WeaponData = player.WeaponData;
            info.ItemData = player.ItemData;
            info.SpellData = player.SpellData;
            info.PotionData = player.PotionData;
            Events.StartRoom(info);
            
        }
    }

}
