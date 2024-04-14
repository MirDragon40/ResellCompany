using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    public List<ItemObject> itemObjects; // 아이템 리스트
    public Image[] SlotImages; // 인벤토리 슬롯의 이미지 배열

    private void Start()
    {

        InitInventory();
    }

    private void InitInventory()
    {
        // 아이템 리스트와 UI 슬롯 이미지 연결
        for (int i = 0; i < SlotImages.Length; i++)
        {

        }
    }
}
