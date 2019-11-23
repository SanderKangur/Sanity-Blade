using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    public Projectile Projectile;
    public float Speed;
    public float Health;
    public float FireRate;
    public AudioSource Oof;
    public AudioSource Brap;
    public Rigidbody2D _rigidBody;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;
    private Vector2 _projectilePos;
    private float _nextFire = 0;
    private bool _inv = false;
    private float _timerInv = 2.0f;
    private float _knockback = 0;



    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
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

        if (_inv && Health != 0)
        {

            _timerInv -= Time.deltaTime;
            if (_timerInv < 0)
            {
                _spriteRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                _inv = false;
                _timerInv = 2.0f;
            }
        }

        float _velX = Input.GetAxisRaw("Horizontal");
        bool moving = false;

        if (_knockback <= 0)
        {
            if (Mathf.Abs(_velX) > float.Epsilon)
            {
                _spriteRenderer.flipX = _velX < 0;
                moving = true;
            }
            _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            _moveVelocity = _moveInput.normalized * Speed;
            
        }
        _knockback -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && Time.time > _nextFire)
        {
          
            _nextFire = Time.time + FireRate;
            Shoot();
            _animator.SetTrigger("Projectile");
        }

        
        if (Input.GetButtonDown("Fire2") && Time.time > _nextFire)
        {
           
            _nextFire = Time.time + FireRate;
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
        _animator.SetBool("Walk", moving);

    }

    private void FixedUpdate()
    {
        if(_knockback <= 0)
        _rigidBody.MovePosition(_rigidBody.position + _moveVelocity * Time.fixedDeltaTime);
    }


    void Shoot()
    {
        Brap.Play();
        Projectile projectile = GameObject.Instantiate<Projectile>(Projectile);
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
    }
}
