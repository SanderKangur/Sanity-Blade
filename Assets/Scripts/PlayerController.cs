using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Projectile Projectile;
    public float Speed;
    public float Health;
    public float FireRate;

    public Rigidbody2D _rigidBody;
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;
    private Vector2 _projectilePos;
    private float _nextFire = 0;
    private float _velX;
    private bool _facingRight = true;
  




    // Start is called before the first frame update
    void Start()
    {
        //_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        _velX = Input.GetAxisRaw("Horizontal");
        _moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _moveVelocity = _moveInput.normalized * Speed;
        if (Input.GetButtonDown("Fire1") && Time.time > _nextFire)
        {
          
            _nextFire = Time.time + FireRate;
            Shoot();
        }

    }
    void LateUpdate()
    {
        Vector3 localScale = transform.localScale;
        if (_velX > 0)
        {
            _facingRight = true;
        }
        else if (_velX < 0)
        {
            _facingRight = false;
        }
        if (((_facingRight) && (localScale.x < 0)) || ((!_facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }
    private void FixedUpdate()
    {
        _rigidBody.MovePosition(_rigidBody.position + _moveVelocity * Time.fixedDeltaTime);

    }

    void Shoot()
    {

        Projectile projectile = GameObject.Instantiate<Projectile>(Projectile);
        projectile.transform.position = this.transform.position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Walls")
        {
            Debug.Log("lol");
            _rigidBody.velocity = Vector2.zero;
            _moveInput = Vector2.zero;
            _moveVelocity = Vector2.zero;
        }
    }
}
