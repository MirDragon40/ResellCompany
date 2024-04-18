using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public PlayerItemHoldAbility PlayerItemHoldAbility;

    public List<ItemObject> itemObjects; // 아이템 리스트
    public UI_InventorySlot[] Slots; // 인벤토리 슬롯의 이미지 배열

    public int selectedIndex = 0;  // 현재 선택된 슬롯의 인덱스


    private void Start()
    {

        InitInventory();


    }
    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        UpdateSelection(scroll);
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

    void UpdateSelection(float scroll)
    {
        if (scroll != 0)
        {
            Slots[selectedIndex].SetScale(1f); // 현재 선택된 슬롯의 스케일을 원래대로

            if (scroll > 0)
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = Slots.Length - 1;
            }
            else if (scroll < 0)
            {
                selectedIndex++;
                if (selectedIndex >= Slots.Length) selectedIndex = 0;
            }

            Slots[selectedIndex].SetScale(1.2f); // 새로 선택된 슬롯의 스케일을 키움
        }
    }

    void UpdateSelectedItem()
    {
        // 여기서 현재 선택된 아이템 정보를 PlayerItemHoldAbility에 전달
       // PlayerItemHoldAbility.SetCurrentItem(inventoryItems[selectedItemIndex]);
    }
}
