using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [Header("Public")]
    public float Health;
    public float MaxHealth = 100; 
    public float Stamina;
    public float MaxStamina = 100;


    [Header("PlayerMoveAbility")]
    public float MoveSpeed = 7;
    public float RunSpeed = 12;
    public float JumpPower = 7f;
    public float RunConsumeStamina = 10f; // 초당 스태미나 소모량
    public float RunRecoveryStamina = 10f;   // 초당 스태미나 충전량
    public float JumpConsumeStamina = 20f;


    [Header("PlayerRotateAbility")]
    public float RotationSpeed = 200;


    public float AttackCoolTime = 1f;
    public float AttackConsumeStamina = 20f;
    public int Damage;

    public int MoneyCount;
    public int CollectedMoneyCount;

    public void Init()
    {
        Health = MaxHealth;
        Stamina = MaxStamina;

    }
}
