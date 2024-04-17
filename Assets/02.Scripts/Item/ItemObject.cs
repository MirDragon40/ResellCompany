using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    public enum ItemState
    {
        Idle,
        Held,
        Stored
    }


    public ItemType ItemType;
    private ItemState _itemState = ItemState.Idle;

    private bool _isTriggered = false;
    private float _progress = 0;

    public Transform PlayerHand;
    private const float LERP_DURATION = 0.3f;

    public int itemValue;
    public string itemName;

    public GameObject Interaction_UI;
    public TextMeshProUGUI Interaction_Text;



    private void Awake()
    {
        if (ItemType == ItemType.JewelItem)
        {
            itemValue = Random.Range(50, 85);
            itemName = "보석";
        }
        if(ItemType == ItemType.SilverCoin)
        {
            itemValue = Random.Range(20, 70);
            itemName = "은화";
        }
        if(ItemType == ItemType.GoldCoin)
        {
            itemValue = Random.Range(60, 120);
            itemName = "금화";

        }


    }

    private void Start()
    {
        Interaction_UI.SetActive(false);
    }

    private void Update()
    {
        switch (_itemState)
        {
            case ItemState.Idle:
                break;

            case ItemState.Held:
                break;

            case ItemState.Stored:
                break;

        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            // 플레이어와 닿으면 트리거 상태를 true로 설정
            
            _isTriggered = true;
            Interaction_UI.SetActive(true);
            Interaction_Text.text = "아이템 줍기 : [E]";

            col.GetComponent<PlayerItemGetAbility>().SetItemObject(this);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && _isTriggered)
            {
               // ItemManager.Instance.AddItem(ItemType);
                Interaction_UI.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            _isTriggered = false;
            Interaction_UI.SetActive(false);

            col.GetComponent<PlayerItemGetAbility>().RemoveItemObject();

        }
    }

    public void Init()
    {
        
    }

}
