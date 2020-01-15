using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRanged : MonoBehaviour
{
    public float Speed = 3f;
    public float Lives = 300f;
    public GameObject Drop;
    public ProjectileEnemy Projectile;
    public SpellData SpellData;
    public AudioClipGroup Monster;
    [Range(0, 10)]
    public float Knockback;

    private float _fireRate;
    private float _nextFire;
    private Animator _animator;
    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _knockback = 0;
    private bool _fireAnim = false;


    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator = GetComponent<Animator>();
        _nextFire = Time.time + Random.Range(2, 4);
    }

    void Update()
    {
       
        bool isMoving = false;
        if (_knockback <= 0)
        {
            if (Vector2.Distance(transform.position, _target.position) > 10)
            {
                isMoving = true;
                GetComponent<SpriteRenderer>().flipX = _target.position.x > transform.position.x;
                transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
            }
            else if(Vector2.Distance(transform.position, _target.position) > 7)
            {
                isMoving = false;
            }
            else
            {
                isMoving = true;
                GetComponent<SpriteRenderer>().flipX = _target.position.x > transform.position.x;
                transform.position = Vector2.MoveTowards(transform.position, -_target.position, Speed * Time.deltaTime);
            }


        }
        _knockback -= Time.deltaTime;

        if (Time.time > _nextFire)
        {
            if (!_fireAnim)
            {
                _knockback = 1.5f;
                _animator.SetTrigger("Attack");
                _fireAnim = true;
            }
            if (Time.time > _nextFire + 1.3)
            {
                _nextFire = Random.Range(2, 4);
                _nextFire = Time.time + _nextFire;
                _fireAnim = false;
                Shoot();
                Shoot();
            }
        }

        if (Lives <= 0)
        {
            PlayerController.Instance.Health = PlayerController.Instance.Health + 10;
            GameObject.Destroy(this.gameObject);
        }
        _animator.SetBool("Walk", isMoving);
    }

    void Shoot()
    {

        ProjectileEnemy projectile = GameObject.Instantiate<ProjectileEnemy>(Projectile);
        projectile.GetComponent<SpriteRenderer>().sprite = SpellData.Sprite;
        projectile.Damage = SpellData.Damage;
        projectile.Speed = SpellData.Speed;
        projectile.Target = _target;
        projectile.transform.position = this.transform.position;

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
