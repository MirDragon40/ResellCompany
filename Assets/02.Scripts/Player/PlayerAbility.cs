using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAbility : Player
{
    protected Player _owner { get; private set; }

    protected void Awake()
    {
        _owner = GetComponentInParent<Player>();
    }


}
