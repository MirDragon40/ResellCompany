using System;
using UnityEngine;
using TMPro;
using System.Collections; // Added for using TextMeshPro

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // 타이머를 표시하는 TextMeshProUGUI
    public GameObject FiredUI;
    public GameObject WinUI;
    public Player player; // 플레이어 스크립트 참조

    private float startTime = 15 * 60; // 15분을 초로 변환
    private float timeRemaining;

    public LobbyScene LobbyScene;
    void Start()
    {
        timeRemaining = startTime;
        FiredUI.SetActive(false); // 게임오버 UI를 숨김
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            timerText.text = "00:00";
            CheckGameOverorWinCondition();
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void CheckGameOverorWinCondition()
    {
        // MoneyCount가 NeedToCollectMoneyCount보다 작으면 게임오버 UI를 활성화
        if (player.Stat.MoneyCount < player.Stat.NeedToCollectMoneyCount)
        {
            FiredUI.SetActive(true);
            StartCoroutine(GameOver_Coroutine());
        }
        else
        {
            WinUI.SetActive(true);
            StartCoroutine(GameOver_Coroutine());
        }
    }

    private IEnumerator GameOver_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        LobbyScene.RestartGame();
    }
}
