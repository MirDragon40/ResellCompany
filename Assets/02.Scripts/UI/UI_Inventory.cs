using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public List<ItemObject> itemObjects; // 아이템 리스트
    public UI_InventorySlot[] Slots; // 인벤토리 슬롯의 이미지 배열

    private void Start()
    {

        InitInventory();
    }

    private void InitInventory()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            if (i < itemObjects.Count)
            {
                Slots[i].SetItem(itemObjects[i]); // 아이템 리스트의 각 아이템을 슬롯에 설정
            }
            else
            {
                // 아이템 리스트에 해당 슬롯을 채울 아이템이 없다면, 빈 아이템을 설정
                Slots[i].SetItem(new ItemObject { ItemType = ItemType.None });
            }
        }
    }
}
