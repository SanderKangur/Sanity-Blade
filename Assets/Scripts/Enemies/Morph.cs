using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morph : MonoBehaviour
{
    public float Speed = 5f;
    public float Lives = 300f;
    public GameObject Drop;
    public ProjectileEnemy Projectile;
    public SpellData SpellData;
    public AudioClipGroup Monster;
    [Range(0, 10)]
    public float Force;

    private float _dragonTimer;
    private float _fairyTimer = 5f;
    private bool _isDragon = false;

    private float _fireRate = 1f;
    private float _nextFire;
    private float _specialRate = 5f;
    private float _nextSpecial;
    private bool _specialAnim = false;
    private bool _fireAnim = false;
    private Animator _animator;
    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _knockback = 0f;
    private bool _attackRight = false;


    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator = GetComponent<Animator>();
        _nextFire = Time.time + 5f;
        _nextSpecial = Time.time + 5f;
    }

    void Update()
    {
        BossUIController.Instance.SetHealth((int)Lives);
        if (!_isDragon)
        {
            Debug.Log(_fairyTimer);
            bool isMoving = false;
            if (_knockback <= 0)
            {
                if (Vector2.Distance(transform.position, _target.position) > 20)
                {
                    isMoving = true;
                    GetComponent<SpriteRenderer>().flipX = _target.position.x > transform.position.x;
                    transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);
                }
                else if (Vector2.Distance(transform.position, _target.position) > 15)
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
            _animator.SetBool("Walk", isMoving);
        }

        _fairyTimer -= Time.deltaTime;
        if (_fairyTimer <= 0 && !_isDragon)
        {
            Debug.Log("kek");
            _animator.SetTrigger("Morph");
            _dragonTimer = 10f;
            _isDragon = true;
        }

        if (_isDragon)
        {
            if (Time.time > _nextFire)
            {
                if (!_fireAnim)
                {
                    _animator.SetTrigger("Attack");
                    _fireAnim = true;
                }

                if (Time.time > _nextFire + 0.5f)
                {
                   
                    _nextFire = Time.time + _fireRate;
                    _fireAnim = false;
                    Shoot();
                    Shoot();
                    Shoot();
                }
            }
        }

        _dragonTimer -= Time.deltaTime;
        if (_dragonTimer < 0 && _isDragon)
        {
            _animator.SetTrigger("MorphBack");
            _fairyTimer = 5f;
            Lives += 100;
            _isDragon = false;
        }

        if (Lives <= 0)
        {
            PlayerController.Instance.Health = PlayerController.Instance.Health + 10;
            GameObject.Destroy(this.gameObject);
        }  
    }

    void Shoot()
    {

        ProjectileEnemy projectile = GameObject.Instantiate<ProjectileEnemy>(Projectile);
        projectile.GetComponent<SpriteRenderer>().sprite = SpellData.Sprite;
        projectile.Damage = SpellData.Damage;
        projectile.Speed = SpellData.Speed;
        projectile.Target = _target;
        projectile.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
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
            _rigidBody.AddForce(dir * Force, ForceMode2D.Impulse);
            _knockback = 0.3f;

        }
        if (collision.gameObject.tag == "Projectile")
        {
            Damage(collision.gameObject.GetComponent<Projectile>().Damage);
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * Force, ForceMode2D.Impulse);
            _knockback = 0.3f;
        }

        if (collision.gameObject.tag == "Melee")
        {
            Damage(PlayerController.Instance.WeaponData.Damage);
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * Force, ForceMode2D.Impulse);
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
