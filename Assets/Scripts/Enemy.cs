using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public float Lives = 100f;

    private Transform _target;


    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
      
        if (Vector2.Distance(transform.position, _target.position) > 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
        }

        if(Lives <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void Damage(float dam)
    {
        Lives -= dam;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Projectile")
        {
            Damage(Projectile.Instance.Damage);
        }
    }



}
