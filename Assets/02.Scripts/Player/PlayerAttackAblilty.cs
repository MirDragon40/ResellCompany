using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAblilty : PlayerAbility
{
    private Animator _animator;

    private float _attackTimer = 0;

    public Collider WeaponCollider;
    public GameObject WeaponObject;

    private void Start()
    {
        {
            _animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        bool haveStamina = _owner.Stat.Stamina >= _owner.Stat.AttackConsumeStamina;
        if(Input.GetMouseButtonDown(0) && _attackTimer >= _owner.Stat.AttackCoolTime && haveStamina)
        {
            _owner.Stat.Stamina -= _owner.Stat.AttackConsumeStamina;

            _attackTimer = 0;

            _animator.SetTrigger("Attack");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    public void ActiveCollider()
    {
        WeaponCollider.enabled = true;

    }

    public void InactiveCollider()
    {
        WeaponCollider.enabled = false;
    }
}
