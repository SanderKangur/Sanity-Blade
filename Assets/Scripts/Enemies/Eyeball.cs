using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeball : MonoBehaviour
{
    public float Speed = 3f;
    public float Lives = 300f;
    public GameObject Drop;
    public ProjectileEnemy Projectile;
    public SpellData SpellData;
    public AudioClipGroup Monster;
    [Range(0, 10)]
    public float Force;

    private float _fireRate = 2;
    private float _nextFire;
    private float _specialRate = 10;
    private float _nextSpecial;
    private bool _specialAnim = false;
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
        _nextSpecial = Time.time + 13;
    }

    void Update()
    {
        bool isMoving = false;
        if (_knockback <= 0)
        {
            if (Vector2.Distance(transform.position, _target.position) > Random.Range(5, 10))
            {
                isMoving = true;
                GetComponent<SpriteRenderer>().flipX = _target.position.x > transform.position.x;
                transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);

            }


        }
        _knockback -= Time.deltaTime;

        if (Time.time > _nextFire) { 
            _nextFire = Time.time + _fireRate;
            Shoot();
            _animator.SetTrigger("Attack");
        }

        if (Time.time > _nextSpecial)
        {
            if (!_specialAnim)
            {
                _animator.SetTrigger("Special");
                _specialAnim = true;
            }
            if (Time.time > _nextSpecial + 1)
            {
                _nextSpecial = Time.time + _specialRate;
                _specialAnim = false;
                Shoot();
                Shoot();
                Shoot();
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
        if(Drop != null)
        Instantiate(Drop, transform.position, Drop.transform.rotation);
    }

}
