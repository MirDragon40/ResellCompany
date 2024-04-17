using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    private SpiderMove _owner;

    private void Start()
    {
        _owner = GetComponentInParent<SpiderMove>();

    }

    public void AttackEvent()
    {
        _owner.PlayerAttack();
    }
}
