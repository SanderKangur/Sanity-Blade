using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ProjectileEnemy")
        {
            GameObject.Destroy(collision.gameObject);
        }
    }

}