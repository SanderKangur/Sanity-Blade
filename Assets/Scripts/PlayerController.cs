﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;
    public Projectile Projectile;
    public GameObject Melee, Defence;
    public Bomb Bomb;
    public Freeze Freeze;

    public float Speed;
    public float Health;
    public float FireRate;
    public float MeleeSpeed;

    public ItemData ItemData, EmptyItemSlot;
    public WeaponData WeaponData;
    public PotionData PotionData, EmptyPotionSlot;
    public SpellData SpellData;

    public ParticleSystem freezeParticles;

    public AudioSource Walk, Boop, Potion, Death, FreezeSound, DefenceSound;
    public AudioClipGroup Oof, Sword, Fireball, Click;

    private bool _inv = false;
    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;
    private Vector2 _projectilePos;
    private float _nextFire = 0;
    private float _timerInv = 2.0f;
    private float _knockback = 0;
    private float _meleeTimer = 0.2f;
    private bool _isMelee = false;
    private const float AntiHealth = 2.1f;
    private bool _attackRight = true;
    private bool _isDead = false;
    private float _defenceTimer;
    


    private void Awake()
    {
        Events.OnStartRoom += StartRoom;
    }
    private void OnDestroy()
    {
        Events.OnStartRoom -= StartRoom;
    }

    void Start()
    {
        Defence.gameObject.SetActive(false);
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        FireRate = SpellData.FireRate;
        MeleeSpeed = WeaponData.Speed;
        Melee.gameObject.SetActive(false);
        Melee.GetComponentInChildren<SpriteRenderer>().sprite = WeaponData.Sprite;
        Instance = this;
        UpdateSprites();

    }

    public void StartRoom(PlayerInfo data)
    { 
        Speed = data.Speed;
        Health = data.Health;      

        ItemData = data.ItemData;
        WeaponData = data.WeaponData;
        PotionData = data.PotionData;
        SpellData = data.SpellData;
        UpdateSprites();
    }
    void UpdateSprites()
    {
        if (ItemData != null && InventoryUIController.Instance.ItemSprite != null) 
            InventoryUIController.Instance.ItemSprite.sprite = ItemData.Sprite;
        if (WeaponData != null && InventoryUIController.Instance.WeaponSprite != null)
            InventoryUIController.Instance.WeaponSprite.sprite = WeaponData.Sprite;
        if (PotionData != null && InventoryUIController1.Instance.PotionSprite != null)
            InventoryUIController1.Instance.PotionSprite.sprite = PotionData.Sprite;
        if (SpellData != null && InventoryUIController1.Instance.SpellSprite != null)
            InventoryUIController1.Instance.SpellSprite.sprite = SpellData.Sprite;
    }
    void Update()
    {
        
        if(Health <= 0)
        {
           
            if (_timerInv == 2.0f)
            {
                _animator.SetTrigger("RIP");
                Death.Play();
                _isDead = true;

            }
            _timerInv -= Time.deltaTime;
            _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

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

        if (_inv && Health > 0)
        {UIController.Instance.SetHealth((int) Health);
            
            UIController.Instance.SetHealth((int) Health);
            _timerInv -= Time.deltaTime;
            if (_timerInv < 0)
            {
                _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                _inv = false;
                _timerInv = 2.0f;
            }
        }

        if (!_isDead)
        {
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

            if (Input.GetButton("Fire2") && Time.time > _nextFire)
            {

                _nextFire = Time.time + FireRate;
                Fireball?.Play();
                Shoot();
                _animator.SetTrigger("Projectile");
            }

            if (Input.GetButton("Fire1") && !_isMelee)
            {
                Sword?.Play();
                _isMelee = true;
                _meleeTimer = 0.5f;
                Melee.gameObject.SetActive(true);
            }
            if (_isMelee)
            {
                if (_meleeTimer > 0)
                {
                    if (velX > 0 || velX == 0 && _attackRight)
                    {
                        Melee.transform.localScale = new Vector3(1, 1, 1);
                        Melee.transform.Rotate(new Vector3(0, 0, -240) * MeleeSpeed * Time.deltaTime);
                        _attackRight = true;
                    }
                    else
                    {
                        Melee.transform.localScale = new Vector3(1, -1, 1);
                        Melee.transform.Rotate(new Vector3(0, 0, 240) * MeleeSpeed * Time.deltaTime);
                        _attackRight = false;
                    }
                    _meleeTimer -= Time.deltaTime;
                }
                else
                {
                    _isMelee = false;
                    if (velX > 0)
                    {
                        Melee.transform.eulerAngles = new Vector3(0, 0, 100);
                    }
                    else
                    {
                        Melee.transform.eulerAngles = new Vector3(0, 0, 70);
                    }
                    Melee.gameObject.SetActive(false);

                }
            }
            _animator.SetBool("Walk", isMoving);

            if (Input.GetKeyDown("q") && PotionData.Boost != 0)
            {

                Health += PotionData.Boost;
                Potion.Play();
                PotionData = EmptyPotionSlot;
                UpdateSprites();
            }
            if (Input.GetKeyDown("e") && ItemData != null)
            {
                Throw();
                ItemData = null;
                Click.Play();
                CameraShaker.Instance.ShakeOnce(0.2f, 0.2f, 0.3f, 0.3f);

                ItemData = EmptyItemSlot;
                UpdateSprites();
            }

            _defenceTimer -= Time.deltaTime;
            if(_defenceTimer <= 0) Defence.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(_knockback <= 0)
        _rigidBody.MovePosition(_rigidBody.position + _moveVelocity * Time.fixedDeltaTime);
    }

    void Throw()
    {
        if (ItemData.TypeDescription.Equals("bomb"))
        {
            Bomb bomb = GameObject.Instantiate<Bomb>(Bomb);
            bomb.GetComponent<SpriteRenderer>().sprite = ItemData.Sprite;
            bomb.transform.position = this.transform.position;
        }

        if (ItemData.TypeDescription.Equals("freeze"))
        {
            FreezeSound?.Play();
            freezeParticles?.Play();
            GameObject.Instantiate<Freeze>(Freeze);
        }

        if (ItemData.TypeDescription.Equals("defence"))
        {
            Defence.gameObject.SetActive(true);
            _defenceTimer = 5f;
            DefenceSound?.Play();
        }
    }

    void Shoot()
    {

        Projectile projectile = GameObject.Instantiate<Projectile>(Projectile);
        projectile.GetComponent<SpriteRenderer>().sprite = SpellData.Sprite;
        projectile.Damage = SpellData.Damage;
        projectile.Speed = SpellData.Speed;
        projectile.transform.position = this.transform.position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MeleeEnemy" && !_inv)
        {
            Debug.Log("meleeenemy");
            Vector2 dir = collision.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 2, ForceMode2D.Impulse);
            _knockback = 0.2f;

            _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Oof.Play();
            Health -= collision.gameObject.GetComponentInParent<Damage>().EnemyMeleeDamage;
            _inv = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_inv)
        {
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 2, ForceMode2D.Impulse);
            _knockback = 0.2f;
            CameraShaker.Instance.ShakeOnce(0.2f, 0.3f, 0.3f, 0.3f);

            _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
            Oof.Play();
            Health -= collision.gameObject.GetComponent<Damage>().EnemyCollisionDamage;
            _inv = true;
            
        }

        if (collision.gameObject.CompareTag("ProjectileEnemy") && !_inv)
        {
            Vector2 dir = collision.GetContact(0).point - (Vector2)this.transform.position;
            dir = -dir.normalized;
            _rigidBody.AddForce(dir * 2, ForceMode2D.Impulse);
            _knockback = 0.2f;
            CameraShaker.Instance.ShakeOnce(0.5f, 0.5f, 0.3f, 0.3f);


            Oof.Play();
            Health -= collision.gameObject.GetComponent<ProjectileEnemy>().Damage;
            _inv = true;
            _spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }

        if (collision.gameObject.tag == "Drop")
        {
            string type = collision.gameObject.GetComponent<Drop>().Type;
            if (type.Equals("Weapon"))
            {
                WeaponData = collision.gameObject.GetComponent<Drop>().Weapon;
                MeleeSpeed = WeaponData.Speed;
                Melee.GetComponentInChildren<SpriteRenderer>().sprite = WeaponData.Sprite;
                Boop.Play();
                UpdateSprites();
            }
            if (type.Equals("Item"))
            {
               ItemData = collision.gameObject.GetComponent<Drop>().Item;
                Boop.Play();
                UpdateSprites();
            }
            if (type.Equals("Spell"))
            {
                SpellData = collision.gameObject.GetComponent<Drop>().Spell;
                Boop.Play();
                UpdateSprites();
            }
            if (type.Equals("Potion"))
            {
                PotionData = collision.gameObject.GetComponent<Drop>().Potion;
                Boop.Play();
                UpdateSprites();
            }
        }
    }
}
