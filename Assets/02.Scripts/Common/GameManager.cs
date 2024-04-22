using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Timeline;


public enum GameState
{
    Go, // 시작
    Pause,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State = GameState.Go;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        
    }
}
