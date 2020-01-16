using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] List;
    public GameObject Drop;
    void Start()
    {
        
        GameObject monster = List[Random.Range(0, List.Length)];
        var kek = GameObject.Instantiate(monster);       
        kek.transform.position = this.transform.position;

        var random = Random.Range(0, 2);
        Debug.Log(random);
        if (random > 0) kek.GetComponent<Melee>().Drop = Drop;
    }

}
