using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    private Vector3 _moveDirection;

    // Start is called before the first frame update
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
