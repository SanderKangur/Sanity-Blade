using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zerg : MonoBehaviour
{
    public float Speed;
    public float Lives = 100f;
    public GameObject Drop;

    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _knockback = 0;
    public AudioClipGroup Monster;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if(_knockback <= 0)
        {
            if (Vector2.Distance(transform.position, _target.position) > 1)
            {
                GetComponent<SpriteRenderer>().flipX = _target.position.x > transform.position.x;
                transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
            }
        }
        _knockback -= Time.deltaTime;
        

        if(Lives <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void Damage(float dam)
    {
        Monster?.Play();
        Lives -= dam;
    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 3, ForceMode2D.Impulse);
            _knockback = 0.3f;
        }
        if (collision.gameObject.tag == "Projectile")
        {
            Damage(Projectile.Instance.Damage);
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 3, ForceMode2D.Impulse);
            _knockback = 0.3f;
        }
        if (collision.gameObject.tag == "Objects" || collision.gameObject.tag == "Drop")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true); 
        }
    }

    private void OnDestroy()
    {
        if (Drop != null)
            Instantiate(Drop, transform.position, Drop.transform.rotation);
    }
}
