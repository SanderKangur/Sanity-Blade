using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public static ProjectileEnemy Instance;
    public float Speed;
    public float Damage;
    public Transform Target;

    private Vector3 _moveDirection;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _moveDirection = (Target.position - transform.position);
        _moveDirection.z = 0;
        _moveDirection.Normalize();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + _moveDirection * Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Walls")
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.Health -= Damage;
            GameObject.Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Walls"))
        {
            GameObject.Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Objects" || collision.gameObject.tag == "Drop")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
        }
    }

    private void OnBecameInvisible()
    {
        GameObject.Destroy(this.gameObject);
    }
}
