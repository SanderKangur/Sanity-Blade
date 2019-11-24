using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float Speed;
    public float Lives = 100f;

    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _knockback = 0;


    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (_knockback <= 0)
        {
            if (Vector2.Distance(transform.position, _target.position) > 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
            }
        }
        _knockback -= Time.deltaTime;

        if(Lives <= 0)
        {
            PlayerController.Instance.Health = PlayerController.Instance.Health + 10;
            GameObject.Destroy(this.gameObject);
        }
    }

    public void Damage(float dam)
    {
        Lives -= dam;
    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 2, ForceMode2D.Impulse);
            _knockback = 0.3f;
          
        }
        if (collision.gameObject.tag == "Projectile")
        {
            Damage(Projectile.Instance.Damage);
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 2, ForceMode2D.Impulse);
            _knockback = 0.3f;
        }

        if (collision.gameObject.tag == "Melee")
        {
            Damage(50);
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 2, ForceMode2D.Impulse);
            _knockback = 0.3f;
        }
    }

    
}
