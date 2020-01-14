using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance;
    public float Damage;
    public ParticleSystem explosion;

    private Vector3 _moveDirection;
    private Rigidbody2D _rigidBody;
    IEnumerator explodeCoroutine;
    public bool doExplode;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        _moveDirection.z = 0;
        _moveDirection.Normalize();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // create coroutine
        doExplode = true;

        explodeCoroutine = Explode();

        // start coroutine
        StartCoroutine(explodeCoroutine);

    }

    IEnumerator Explode()
    {
            yield return new WaitForSeconds(2);
            Debug.Log("swag");
            explosion.Play();
            explosion.transform.SetParent(null);
            GameObject.Destroy(this.gameObject);

    }
    // Update is called once per frame
    void Update()
    {
       
        
    }
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
       /* if (collision.tag == "Enemy" || collision.tag == "Walls")
        {
            GameObject.Destroy(this.gameObject);
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Walls"))
        {
            GameObject.Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Objects" || collision.gameObject.tag == "Drop")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
        }*/
    }

    private void OnBecameInvisible()
    {
        GameObject.Destroy(this.gameObject);
    }
}