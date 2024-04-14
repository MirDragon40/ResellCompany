using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamaged
{
    public Stat Stat;

    private void Awake()
    {
        Stat.Init();
    }

    public void Damaged(int damage)
    {

    }

}
