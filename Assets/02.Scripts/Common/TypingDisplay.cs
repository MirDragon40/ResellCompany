using UnityEngine;
using UnityEngine.UI; // TextMesh Pro 네임스페이스를 사용합니다.
using System.Collections;
using TMPro;
using UnityEngine.Playables;

public class TypingDisplay : MonoBehaviour
{

    public Text typingText; // Text 컴포넌트를 참조합니다.
    public string currentText = ""; // 현재 입력된 텍스트를 저장합니다.
    private bool isCursorVisible; // 커서의 가시성을 결정하는 bool 변수입니다.
    private float cursorBlinkRate = 0.5f; // 커서 깜빡임 속도입니다.
    private string cursorChar = "|"; // 커서 문자입니다.



    private void Start()
    {
        // 커서 깜빡임 코루틴 시작
        StartCoroutine(CursorBlink_Coroutine());
    }

    void Update()
    {
        // canType이 true일 때만 키보드 입력을 받는다.
        if (Terminal.Instance.IsUsingTerminal)
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\n' || c == '\r') // 마지막에 입력된 문자가 엔터 키일 경우
                {
                    if (currentText == "Store" || currentText == "store")
                    {
                        Debug.Log("Store 입력 감지!");
                        // Store 관련 이벤트 처리 로직
                        Terminal.Instance.HandleStoreCommand();

                        
                    }
                    // "Axe" 명령어 처리
                    else if (currentText == "Axe"|| currentText == "axe")
                    {
                        if (Terminal.Instance.Player.Stat.MoneyCount < 30)
                        {
                            StartCoroutine(Terminal.Instance.CantBuy_Coroutine());
                        }
                        else
                        {
                            Debug.Log("Axe 입력 감지!");
                            // Axe 관련 이벤트 처리 로직
                            Terminal.Instance.HandleAxeCommand();
                            StartCoroutine(Terminal.Instance.BoughtItem_Coroutine());

                        }
                    }
                    // "Flashlight" 명령어 처리
                    else if (currentText == "Flashlight" || currentText == "flashlight")
                    {
                        if (Terminal.Instance.Player.Stat.MoneyCount < 25)
                        {
                            StartCoroutine(Terminal.Instance.CantBuy_Coroutine());
                        }
                        else
                        {
                            Debug.Log("Flashlight 입력 감지!");
                            // Flashlight 관련 이벤트 처리 로직
                            Terminal.Instance.HandleFlashlightCommand();
                            StartCoroutine(Terminal.Instance.BoughtItem_Coroutine());

                            //Terminal.Instance.Player.Stat.MoneyCount -= 25;
                        }
                    }
                    else
                    {
                        if (Terminal.Instance.Terminal_text1.enabled && !Terminal.Instance.Terminal_text2.enabled)
                        {
                            StartCoroutine(Terminal.Instance.Notification_Coroutine_text1());
                        }
                        if(Terminal.Instance.Terminal_text2.enabled && !Terminal.Instance.Terminal_text1.enabled)
                        {
                            StartCoroutine(Terminal.Instance.Notification_Coroutine_text2());
                        }

                    }

                    currentText = ""; // 텍스트 초기화
                    continue; // 엔터 키는 텍스트에 추가하지 않습니다.
                }
                // Backspace 키 처리
                if (c == '\b' && currentText.Length != 0)
                {
                    currentText = currentText.Substring(0, currentText.Length - 1);
                }
                // 일반 문자 입력 처리
                else if (c != '\b')
                {
                    currentText += c;
                }
            }
        }
        // 커서를 텍스트 끝에 추가하거나 제거합니다.
        typingText.text = isCursorVisible ? currentText + cursorChar : currentText;
    }

    IEnumerator CursorBlink_Coroutine()
    {
        // 무한 루프로 커서 깜빡임을 처리합니다.
        while (true)
        {
            // 커서 가시성을 반전시킵니다.
            isCursorVisible = !isCursorVisible;
            // 설정된 깜빡임 속도만큼 대기합니다.
            yield return new WaitForSeconds(cursorBlinkRate);
        }
    }

}