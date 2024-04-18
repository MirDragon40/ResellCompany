using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Accessibility;

public class HouseDoorController : MonoBehaviour
{
    // 플레이어가 가까이 다가가면 문열기
    private Animator _animator;
    public GameObject Interaction_UI;
    public TextMeshProUGUI Interaction_Text;

    private bool _isOpened = false;
  

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _isOpened = false;
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interaction_UI.SetActive(true);
            if (!_isOpened)
            { 
                Interaction_Text.text = "문열기: [E]";
            }
            else
            {
                Interaction_Text.text = "문닫기: [E]";

            }


        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isOpened)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Interaction_UI.SetActive(false);
                    _isOpened = true;
                    _animator.SetTrigger("Open");

                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Interaction_UI.SetActive(false);
                    _isOpened = false;
                    _animator.SetTrigger("Close");

                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interaction_UI.SetActive(false);

    }
}
