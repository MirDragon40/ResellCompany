using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InventorySlot : MonoBehaviour
{
    public GameObject[] itemImages; // 아이템 타입에 따른 이미지 오브젝트 배열

    private UI_Inventory inventory;


    private void Start()
    {
        inventory = GetComponentInParent<UI_Inventory>();
    }

    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    // 슬롯에 아이템을 설정하는 메서드
    public void SetItem(ItemObject item)
    {
        // 우선 모든 자식 이미지를 비활성화
        foreach (var image in itemImages)
        {
            image.SetActive(false);
        }

        if ((int)item.ItemType == (int)ItemType.None)
        {
            // 아이템 타입이 None이면, 어떤 이미지도 표시하지 않는다.
            return;
        }
        else if ((int)item.ItemType == (int)ItemType.Axe)
        {
            itemImages[2].SetActive(true);
        }
        else if((int)item.ItemType == (int)ItemType.Flashlight)
        {
            itemImages[1].SetActive(true);
        }
        else
        {
            itemImages[0].SetActive(true);
        }
        
    }


}
