using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public PlayerAttackAbility MyPlayerAttackAbility;

    public void OnTriggerEnter(Collider other)
    {
        MyPlayerAttackAbility.OnTriggerEnter(other);
    }
}
