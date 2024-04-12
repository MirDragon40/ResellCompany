using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerScan : MonoBehaviour
{
    public Image UI_playerScan;

    private bool _isScanning;

    private void Start()
    {
        _isScanning = false;
        UI_playerScan.enabled = false;
    }

    private void Update()
    {
        ShowScan();
    }

    private void ShowScan()
    {
        if (Input.GetMouseButtonDown(1) && !_isScanning)
        {
            UI_playerScan.enabled = true;
            StartCoroutine(Scan_Coroutine(5f, 2f));
        }
    }

    private IEnumerator Scan_Coroutine(float ScanTime, float ScanCoolTime)
    {
        _isScanning = true;

        yield return new WaitForSeconds(ScanTime);
        UI_playerScan.enabled = false;

        yield return new WaitForSeconds(ScanCoolTime);
        _isScanning = false;
    }
}
