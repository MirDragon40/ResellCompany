using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int itemPrice;

    private void Awake()
    {
        itemPrice = Random.Range(50, 101);
        Debug.Log(itemPrice);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어와 닿으면 트리거 상태를 true로 설정

            _isTriggered = true; 

            if (Input.GetKeyDown(KeyCode.E))
            {
                _progress += Time.deltaTime / LERP_DURATION;
                transform.position = Vector3.Lerp(transform.position, PlayerHand.position, _progress);

            }


        }
    }

    public void Init()
    {
        
    }

}
