using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public List<ItemType> items;
    private int inventoryNum = 4;

    public List<int> itemValues;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        items = new List<ItemType>(inventoryNum);
        itemValues = new List<int>(inventoryNum);
    }

    // 아이템을 인벤토리에 추가
    public void AddItem(ItemType itemToAdd, int itemValue)
    {
        // 인벤토리에 빈 공간이 있는지 확인
        if (items.Count < inventoryNum && items.Count >= 0)
        {
            items.Add(itemToAdd);
            // 아이템 가치도 함께 저장
            itemValues.Add(itemValue);
            // 아이템 추가 성공
        }
    }


    // 아이템을 인벤토리에서 제거
    public void RemoveItem(ItemType itemToRemove)
    {
        items.Remove(itemToRemove);
    }


}


