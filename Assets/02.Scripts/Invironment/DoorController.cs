using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject Interaction_UI;
    public TextMeshProUGUI Interaction_Text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interaction_UI.SetActive(true);

            Interaction_Text.text = "문열기: [E]";
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interaction_UI.SetActive(false);
                Destroy(this.gameObject);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interaction_UI.SetActive(false);

    }
}
