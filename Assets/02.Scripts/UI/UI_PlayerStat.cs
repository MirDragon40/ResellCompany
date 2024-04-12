using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UI_PlayerStat : MonoBehaviour
{
    public static UI_PlayerStat Instance { get; private set; }

    public Player Player;
    public Image Staminabar_Image;
    public CanvasGroup PlayerDamaged_Image;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

    }
    void Update()
    {
        if (Player == null)
        {
            return;
        }

        Staminabar_Image.fillAmount = Player.Stat.Stamina / Player.Stat.MaxStamina;
        PlayerDamaged_Image.alpha = 1/ (Player.Stat.Health / Player.Stat.MaxHealth);

        Debug.Log(Player.Stat.Stamina / Player.Stat.MaxStamina);
    }
}

