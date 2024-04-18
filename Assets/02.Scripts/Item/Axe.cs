using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public PlayerAttackAbility MyPlayerAttackAbility;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 발생");
        MyPlayerAttackAbility.OnTriggerEnter(other);
    }
}
