using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;
    public Projectile Projectile;
    public float Speed;
    public float Health;
    public float FireRate;
    public AudioSource Oof;
    public AudioSource Brap;
    public AudioSource Walk;
    public CapsuleCollider2D Collider;
    public AudioClipGroup Sword, Fireball;
    public Sprite _projectileSprite, _weaponSprite;

    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;
    private Vector2 _projectilePos;
    private float _nextFire = 0;
    private bool _inv = false;
    private float _timerInv = 2.0f;
    private float _knockback = 0;
    private float _meleeTimer = 0.2f;
    private bool _isMelee = false;
    private const float AntiHealth = 2.1f;




    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Collider.gameObject.SetActive(false);
        Instance = this;
        

    }

    void Update()
    {
        if(Health <= 0)
        {
           
            if (_timerInv == 2.0f)
            {
                _animator.SetTrigger("RIP");
               
            }
            _timerInv -= Time.deltaTime;
            _spriteRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            if (_timerInv < 0)
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }
        if (Health > 0)
        {
            Health -= AntiHealth * Time.deltaTime;
            UIController.Instance.SetHealth((int)Health);

        }

        if (_inv && Health != 0)
        {
            
            UIController.Instance.SetHealth((int) Health);
            _timerInv -= Time.deltaTime;
            if (_timerInv < 0)
            {
                _spriteRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                _inv = false;
                _timerInv = 2.0f;
            }
        }

        float velX = Input.GetAxisRaw("Horizontal");
        float velY = Input.GetAxisRaw("Vertical");
        bool isMoving = false;

        if (_knockback <= 0)
        {
            if (Mathf.Abs(velX) > float.Epsilon || Mathf.Abs(velY) > float.Epsilon)
            {
                _spriteRenderer.flipX = velX < 0;
                isMoving = true;
                Walk.volume = Random.Range(0.8f, 1.0f);
                Walk.pitch = Random.Range(0.8f, 1.2f);
                if (!Walk.isPlaying)
                {
                    Walk.Play();
                }
            }
            _moveInput = new Vector2(velX, velY);
            _moveVelocity = _moveInput.normalized * Speed;
            
        }
        _knockback -= Time.deltaTime;

        if (Input.GetButtonDown("Fire2") && Time.time > _nextFire)
        {
          
            _nextFire = Time.time + FireRate;
            Fireball?.Play();
            Shoot();
            _animator.SetTrigger("Projectile");
        }

        
        if (Input.GetButtonDown("Fire1") && !_isMelee)
        {
            Sword?.Play();
            _isMelee = true;
            _meleeTimer = 0.2f;
            Collider.gameObject.SetActive(true);

            int random = Random.Range(1, 5);
            if (random == 1)
            {
                _animator.SetTrigger("Critical");
            }
            else
            {
                _animator.SetTrigger("Melee");
            }
        }
        if (_isMelee)
        {
            if (_meleeTimer > 0)
            {
                _meleeTimer -= Time.deltaTime;
            }
            else
            {
                _isMelee = false;
                Collider.gameObject.SetActive(false);
            }
        }
        _animator.SetBool("Walk", isMoving);


    }

    private void FixedUpdate()
    {
        if(_knockback <= 0)
        _rigidBody.MovePosition(_rigidBody.position + _moveVelocity * Time.fixedDeltaTime);
    }


    void Shoot()
    {

        Projectile projectile = GameObject.Instantiate<Projectile>(Projectile);
        projectile.GetComponent<SpriteRenderer>().sprite = _projectileSprite; 
        projectile.transform.position = this.transform.position;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_inv)
        {
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 2, ForceMode2D.Impulse);
            _knockback = 0.2f;

            Oof.Play();
            Health -= 20;
            _inv = true;
            _spriteRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }

        if (collision.gameObject.tag == "Drop")
        {
            string type = collision.gameObject.GetComponent<Drop>().Type;
            if (type.Equals("Weapon"))
            {
                _weaponSprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            }
            if (type.Equals("Projectile"))
            {
               _projectileSprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }
}
