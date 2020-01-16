using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject Exit;
    public GameObject AltExit;
    public GameObject BackExit;
    public string NextRoom;
    public string AltRoom;
    public string BackRoom;
    public bool isCleared;

    public AudioSource DoorOpen;
    public AudioSource Ambience;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        Exit.SetActive(false);
        if (AltExit != null) AltExit.SetActive(false);
        if (BackExit != null) BackExit.SetActive(false);
        Ambience.Play();

       
    }

    // Update is called once per frame
    void Update()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
       
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name) == 1) isCleared = true;

        if(enemies.Length == 0 && !Exit.activeSelf)
        {
            DoorOpen.Play();
            isCleared = true;
            Exit.SetActive(true);

            if (AltExit != null) AltExit.SetActive(true);
            if (BackExit != null) BackExit.SetActive(true);


            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
        }

        if (isCleared)
        {
            foreach (GameObject enemy in enemies)
            {
                GameObject.Destroy(enemy.gameObject);
            }

        }
    }

    private void OnLevelWasLoaded(int level)
    {

        PlayerPrefs.SetString("currentRoom", SceneManager.GetActiveScene().name);

        string currentRoom = PlayerPrefs.GetString("currentRoom");
        string previousRoom = PlayerPrefs.GetString("previousRoom");

        //start scene
        if (currentRoom.Equals("No Name") && previousRoom.Equals("No Name"))
        {
            PlayerPrefs.SetString("currentRoom", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetString("previousRoom", SceneManager.GetActiveScene().name);
        }

        if (previousRoom.Equals(AltRoom)) {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float x = AltExit.transform.position.x;
            float y = AltExit.transform.position.y;
            float z = AltExit.transform.position.z;
            if (x < -8 ) player.transform.position = new Vector3(x + 2, y, z);
            if (x > -9) player.transform.position = new Vector3(x - 2, y, z);
            if (y < -1.5) player.transform.position = new Vector3(x, y + 1, z);
            if (y > 3) player.transform.position = new Vector3(x, y - 1, z);




        }

        if (previousRoom.Equals(NextRoom))
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float x = Exit.transform.position.x;
            float y = Exit.transform.position.y;
            float z = Exit.transform.position.z;

            if (x < -8) player.transform.position = new Vector3(x + 2, y, z);
            if (x > -9) player.transform.position = new Vector3(x - 2, y, z);
            if (y < -1.5) player.transform.position = new Vector3(x, y + 1, z);
            if (y > 3) player.transform.position = new Vector3(x, y - 1, z);


        }

        if (previousRoom.Equals(BackRoom))
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float x = BackExit.transform.position.x;
            float y = BackExit.transform.position.y;
            float z = BackExit.transform.position.z;

            if (x < -8) player.transform.position = new Vector3(x + 2, y, z);
            if (x > -9) player.transform.position = new Vector3(x - 2, y, z);
            if (y < -1.5) player.transform.position = new Vector3(x, y + 1, z);
            if (y > 3) player.transform.position = new Vector3(x, y - 1, z);


        }

        Debug.Log("current");
        Debug.Log(currentRoom);
        Debug.Log("previous");
        Debug.Log(previousRoom);

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
