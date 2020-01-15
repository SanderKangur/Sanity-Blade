using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance;
    public float Damage;
    public ParticleSystem explosion;
    public GameObject AoE;

    private Rigidbody2D _rigidBody;
    private float _timerAoE = 0.2f;
    private float _timerBomb = 2f;



    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        AoE.SetActive(false);
    }

    void Update()
    {
        if (AoE.activeSelf)
        {
            _timerAoE -= Time.deltaTime;
            if (_timerAoE < 0)
            {
                GameObject.Destroy(this.gameObject);
            }
        }


        _timerBomb -= Time.deltaTime;
        if (_timerBomb < 0)
        {
            AoE.SetActive(true);
            explosion.Play();
            this.GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}