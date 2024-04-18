using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerItemHoldAbility : PlayerAbility
{
    public UI_Inventory Inventory;
    public ItemType CurrentItem;
    public int CurrentItemValue;

    private void Start()
    {
        CurrentItem = 0;
    }

    private void Update()
    {
    }
    void SetCurrentItem(ItemObject itemObject)
    {
       
    }
}
