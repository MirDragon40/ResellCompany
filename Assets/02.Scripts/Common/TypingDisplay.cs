using UnityEngine;
using UnityEngine.UI; // TextMesh Pro 네임스페이스를 사용합니다.
using System.Collections;
using TMPro;

public class TypingDisplay : MonoBehaviour
{
    public Text typingText; // TextMeshPro UI 컴포넌트를 참조합니다.
    public bool canType; // 타이핑 가능 여부를 결정하는 bool 변수입니다.
    private string currentText = ""; // 현재 입력된 텍스트를 저장합니다.
    private bool isCursorVisible; // 커서의 가시성을 결정하는 bool 변수입니다.
    private float cursorBlinkRate = 0.5f; // 커서 깜빡임 속도입니다.
    private string cursorChar = "|"; // 커서 문자입니다.

    private void Start()
    {
        // 커서 깜빡임 코루틴 시작
        StartCoroutine(CursorBlink());
    }

    void Update()
    {
        // canType이 true일 때만 키보드 입력을 받습니다.
        if (canType)
        {
            foreach (char c in Input.inputString)
            {
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

    IEnumerator CursorBlink()
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