using UnityEngine;
using UnityEngine.UI; // TextMesh Pro 네임스페이스를 사용합니다.
using System.Collections;
using TMPro;

public class TypingDisplay : MonoBehaviour
{
    public static TypingDisplay Instance { get; private set; }

    public Text typingText; // Text 컴포넌트를 참조합니다.
    public bool _canType; // 타이핑 가능 여부를 결정하는 bool 변수입니다.
    public string currentText = ""; // 현재 입력된 텍스트를 저장합니다.
    private bool isCursorVisible; // 커서의 가시성을 결정하는 bool 변수입니다.
    private float cursorBlinkRate = 0.5f; // 커서 깜빡임 속도입니다.
    private string cursorChar = "|"; // 커서 문자입니다.

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        // 커서 깜빡임 코루틴 시작
        StartCoroutine(CursorBlink_Coroutine());
    }

    void Update()
    {
        // canType이 true일 때만 키보드 입력을 받는다.
        if (_canType)
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\n' || c == '\r') // 마지막에 입력된 문자가 엔터 키일 경우
                {
                    if (currentText == "Axe")
                    {
                        // "item" 입력 시 실행할 이벤트
                        Debug.Log("Item 입력 감지!");
                        // 원하는 이벤트 코드를 여기에 추가하세요.
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