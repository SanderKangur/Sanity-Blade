using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Brother : MonoBehaviour
{
    public float Speed = 3f;
    public float Lives = 300f;
    public GameObject Drop, Melee;
    public ProjectileEnemy Projectile;
    public SpellData SpellData;
    public AudioClipGroup Monster;
    [Range(0, 10)]
    public float Knockback;
    public GameObject Canvas;

    private float _fireRate = 5f;
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
        _nextFire = Time.time + 10f;
        _nextSpecial = Time.time + 5f;
    }

    void Update()
    {
        BossUIController.Instance.SetHealth((int)Lives);
        bool isMoving = false;
        if (_knockback <= 0)
        {
            if (Vector2.Distance(transform.position, _target.position) > 2)
            {
                isMoving = true;
                GetComponent<SpriteRenderer>().flipX = _target.position.x > transform.position.x;               
                transform.position = Vector2.MoveTowards(transform.position, _target.position, Speed * Time.deltaTime);

            }


        }
        _knockback -= Time.deltaTime;

        if (_target.position.x >= transform.position.x && !_attackRight || _target.position.x < transform.position.x && _attackRight)
        {
            _attackRight = !_attackRight;
            Melee.GetComponent<CapsuleCollider2D>().gameObject.transform.Rotate(0, 180, 0);
        }


        if (Time.time > _nextFire && !_specialAnim)
        {
            if (!_fireAnim)
            {                
                _animator.SetTrigger("Attack");
                _fireAnim = true;
            }

            if(Time.time > _nextFire + 1f)
            {
                _nextFire = Time.time + _fireRate;
                _fireAnim = false;
            }
        }

        if (Time.time > _nextSpecial && !_fireAnim)
        {
            if (!_specialAnim)
            {
                _animator.SetTrigger("Special");
                _specialAnim = true;
                _knockback = 1f;
            }
            if (Time.time > _nextSpecial + 1f)
            {
                _nextSpecial = Time.time + _specialRate;
                _specialAnim = false;
                Shoot();
                Shoot();
                Shoot();

            }
        }

        if (Lives <= 0)
        {
            Canvas.SetActive(true);
            Time.timeScale = 0f;

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
        projectile.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        projectile.transform.position = this.transform.position;

    }

    public void Damage(float dam)
    {
        Monster?.Play();
        Lives -= dam;
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

        if (collision.gameObject.tag == "Objects" || collision.gameObject.tag == "Drop")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
        }
    }

    private void OnDestroy()
    {
        if (PlayerPrefs.GetInt(SceneManager.GetActiveScene().name) == 1) Drop = null;

        if (Drop != null)
        Instantiate(Drop, transform.position, Drop.transform.rotation);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

    }

}
