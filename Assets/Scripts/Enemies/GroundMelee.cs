using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMelee : MonoBehaviour
{
    public float Speed = 3f;
    public float Lives = 100f;
    public GameObject Drop;
    public AudioClipGroup Monster;
    [Range(0, 10)]
    public float Knockback;
    public bool isFrozen = false;

    private float _fireRate = 2;
    private float _nextFire;
    private Animator _animator;
    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _knockback = 0;


    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator = GetComponent<Animator>();
        _nextFire = Time.time + 5;
    }

    void Update()
    {
        bool isMoving = false;
        _knockback -= Time.deltaTime;
        if (_knockback <= 0 && !isFrozen)
        {
            if (Vector2.Distance(transform.position, _target.position) > 1)
            {
                isMoving = true;
                GetComponent<SpriteRenderer>().flipX = _target.position.x > transform.position.x;
                transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
            }

        }

        if (isFrozen) _animator.speed = 0;
        else _animator.speed = 1;


        if (Lives <= 0)
        {
            PlayerController.Instance.Health = PlayerController.Instance.Health + 10;
            GameObject.Destroy(this.gameObject);
        }
        _animator.SetBool("Walk", isMoving);
    }

    public void Damage(float dam)
    {
        Monster?.Play();
        Lives -= dam;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * Knockback, ForceMode2D.Impulse);
            _knockback = 0.3f;

        }
        if (collision.gameObject.tag == "Projectile")
        {
            Damage(collision.gameObject.GetComponent<Projectile>().Damage);
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * Knockback, ForceMode2D.Impulse);
            _knockback = 0.3f;
        }

        if (collision.gameObject.tag == "Drop")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Melee")
        {
            Debug.Log("Melee");
            Damage(PlayerController.Instance.WeaponData.Damage);
            Vector2 dir = collision.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * Knockback, ForceMode2D.Impulse);
            _knockback = 0.3f;
        }

        if (collision.gameObject.tag == "Bomb")
        {
            Damage(100);
            Vector2 dir = collision.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * Knockback, ForceMode2D.Impulse);
            _knockback = 0.3f;
        }
    }

    private void OnDestroy()
    {
        if (Drop != null)
            Instantiate(Drop, transform.position, Drop.transform.rotation);
    }

}
