using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    private float _timer = 3f;

    void Start()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.9f, 1.0f, 1.0f);
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            enemy.GetComponent<GroundMelee>().isFrozen = true;
        }
    }

    void Update()
    {

        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation; 
                enemy.GetComponent<GroundMelee>().isFrozen = false;
            }
            GameObject.Destroy(this.gameObject);
        }
    }

}