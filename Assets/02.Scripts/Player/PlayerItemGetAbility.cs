using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemGetAbility : PlayerAbility
{

    private ItemObject _currentItemObject = null;

    public void SetItemObject(ItemObject itemObject)
    {
        _currentItemObject = itemObject;
    }

    public void RemoveItemObject()
    {
        _currentItemObject = null;
    }

    private void Update()
    {
        if( _currentItemObject == null ) 
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.E)) 
        {

            ItemManager.Instance.AddItem(_currentItemObject.ItemType);

            RemoveItemObject();
        }
    }
}
