using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
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
        Debug.Log($"{other.name}");
        if (other.CompareTag("Monster"))
        {
            Debug.Log("거미가 맞았다");

            SpiderMove spiderMove = other.GetComponent<SpiderMove>();
            IDamaged hitMonster = other.GetComponent<IDamaged>();
            if(hitMonster != null)
            {
                hitMonster.Damaged(spiderMove.Stat.Damage);
            }
        }
    }

    public void ActiveCollider()
    {
        WeaponCollider.enabled = true;
        Debug.Log("WeaponCollider.enabled");
    }

    public void InactiveCollider()
    {
        WeaponCollider.enabled = false;
        Debug.Log("WeaponCollider.enabled");

    }
}
