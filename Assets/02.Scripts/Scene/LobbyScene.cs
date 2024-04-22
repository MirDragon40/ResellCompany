using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    private enum SceneNames
    {
        Lobby,
        Main
    }

    public void OnGameStartButton()
    {
        SceneManager.LoadScene((int)SceneNames.Main);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene((int)SceneNames.Lobby);
    }


    public void OnGameExitButton()
    {
        Application.Quit();

#if UNITY_EDITOR
        // 유니티 에디터 에서 실행했을 경우 종료하는 방법
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

}
