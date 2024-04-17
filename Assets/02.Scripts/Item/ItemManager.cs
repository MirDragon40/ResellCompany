using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    public List<ItemObject> items = new List<ItemObject>(4);



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 아이템을 인벤토리에 추가
    public void AddItem(ItemObject itemToAdd)
    {
        // 인벤토리에 빈 공간이 있는지 확인
        if (items.Count < 4)
        {
            items.Add(itemToAdd);
            // 아이템 추가 성공
            
        }
        // 인벤토리가 가득 찼을 경우
       
    }

    // 아이템을 인벤토리에서 제거
    public void RemoveItem(ItemObject itemToRemove)
    {
        items.Remove(itemToRemove);
    }

}


