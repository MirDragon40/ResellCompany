using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerScan : MonoBehaviour
{
    public static UI_PlayerScan Instance { get; private set; }

    public Image UI_playerScan;



    //[HideInInspector]
    public bool _isScanning;
    public bool _isScanCoolTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _isScanning = false;
        _isScanCoolTime = false;
        UI_playerScan.enabled = false;
    }

    private void Update()
    {
        ShowScan();
    }

    private void ShowScan()
    {
        if (Input.GetMouseButtonDown(1) && !_isScanning && !_isScanCoolTime)
        {
            UI_playerScan.enabled = true;
            StartCoroutine(Scan_Coroutine(5f, 2f));
        }
    }

    private IEnumerator Scan_Coroutine(float ScanTime, float ScanCoolTime)
    {
        _isScanning = true;
        _isScanCoolTime = true;

        yield return new WaitForSeconds(ScanTime);
        UI_playerScan.enabled = false;
        _isScanning = false;
        

        yield return new WaitForSeconds(ScanCoolTime);
        _isScanCoolTime = false;
    }
}
