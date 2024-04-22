using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CollectedMoney : MonoBehaviour
{
    public TextMeshProUGUI CollectedMoney;
    public TextMeshProUGUI NeedtoCollectMoneyCount;
    public Player player;

    private void Start()
    {
       
    }

    private void Update()
    {
        CollectedMoney.text = $"모은 아이템 가치: ${player.Stat.CollectedMoneyCount}";
        NeedtoCollectMoneyCount.text = $"할당량: ${player.Stat.NeedToCollectMoneyCount}";

    }


}
