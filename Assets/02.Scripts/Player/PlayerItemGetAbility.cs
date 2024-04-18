using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemGetAbility : MonoBehaviour
{
    public ItemObject CurrentItemObject = null;

    public void SetItemObject(ItemObject itemObject)
    {
        CurrentItemObject = itemObject;

        if(itemObject.ItemType == ItemType.Axe)
        {
            ItemManager.Instance.HaveAxe = true;
        }
        else if (itemObject.ItemType == ItemType.Flashlight)
        {
            ItemManager.Instance.HaveFlashlight = true;

        }

    }

    public void RemoveItemObject()
    {
        CurrentItemObject = null;
    }

    private void Update()
    {
        if (CurrentItemObject == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ItemManager.Instance.AddItem(CurrentItemObject);
            //RemoveItemObject();
        }
    }
}
