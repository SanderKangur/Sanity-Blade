using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public Collider2D AttackTrigger;

    private bool _isAttacking = false;
    private float _attackTimer = 0;
    private float _attackCooldown = 0.3f;
    private Animator _animator;

    private void Awake()
    {
        //_animator = GetComponent<Animator>();
        AttackTrigger.gameObject.SetActive(false);

    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire2") && !_isAttacking)
        {
            _isAttacking = true;
            _attackTimer = _attackCooldown;
            AttackTrigger.gameObject.SetActive(true);
        }

        if (_isAttacking)
        {
            if(_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime;
            }
            else
            {
                _isAttacking = false;
                AttackTrigger.gameObject.SetActive(false);
            }
        }
        //_animator.SetBool("Attacking", _isAttacking);
    }

}
