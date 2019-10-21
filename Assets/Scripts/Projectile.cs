using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Projectile Instance;
    public float Speed;
    public float Damage;

    private Vector3 _moveDirection;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        _moveDirection.z = 0;
        _moveDirection.Normalize();
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

    private void OnBecameInvisible()
    {
        GameObject.Destroy(this.gameObject);
    }
}
