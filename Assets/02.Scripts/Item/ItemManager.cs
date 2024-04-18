using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{

    public static ItemManager Instance { get; private set; }

    public List<ItemObject> ItemObjects;

    private int inventoryNum = 4;

    public bool HaveAxe = false;
    public bool HaveFlashlight = false;

    public GameObject AxeImage;
    public GameObject FlashlightImage;

    public GameObject AxeItemObject_camera;
    public GameObject FlashlightItemObject_camera;

    public GameObject AxeItemObject_Hand;
    public GameObject Flashlight_Hand;

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

        ItemObjects = new List<ItemObject>(inventoryNum);
        
    }

    private void Start()
    {
        HaveAxe = false;
        HaveFlashlight = false;
    }

    // 아이템을 인벤토리에 추가
    public void AddItem(ItemObject itemToAdd)
    {
       
    }


    // 아이템을 인벤토리에서 제거
    public void RemoveItem(ItemObject itemToRemove)
    {
        ItemObjects.Remove(itemToRemove);

    }


}


