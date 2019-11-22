using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zerg : MonoBehaviour
{
    public float Speed;
    public float Lives = 100f;

    private Transform _target;
    private Rigidbody2D _rigidBody;


    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if(Speed == 0)
        {

        }
      
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

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 7, ForceMode2D.Impulse);
            //_rigidBody.AddForce(new Vector2(_rigidBody.velocity.x * -1, _rigidBody.velocity.y * -1));
        }
        if (collision.gameObject.tag == "Projectile")
        {
            Damage(Projectile.Instance.Damage);
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 3, ForceMode2D.Impulse);
        }
        if (collision.gameObject.tag == "Objects")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true); 
        }
    }
}
