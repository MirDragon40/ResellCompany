using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScanImage : MonoBehaviour
{
    public Image scanImage;

    public TextMeshProUGUI ItemName_text;
    public TextMeshProUGUI ItemValue_text;

    private ItemObject itemObject;
    

    private void Start()
    {
        scanImage.gameObject.SetActive(false);
        itemObject = GetComponentInParent<ItemObject>();

        ItemName_text.text = $"{itemObject.itemName}";
        ItemValue_text.text = $"가치:${itemObject.itemValue}";
    }

    private void Update()
    {
        OnUI_ScanImage();


    }

    public void OnUI_ScanImage()
    {
        if (UI_PlayerScan.Instance._isScanning)
        {
            scanImage.gameObject.SetActive(true);
        }
        else
        {
            scanImage.gameObject.SetActive(false);
        }
    }
}
