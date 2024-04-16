using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Animator playerAnimator; // 플레이어의 Animator 컴포넌트
    public float walkShakeAmount = 0.1f; // 걷기 시 카메라 흔들림 정도
    public float runShakeAmount = 0.2f; // 뛰기 시 카메라 흔들림 정도
    public float shakeFrequency = 2f; // 흔들림의 빈도

    private Vector3 originalPos; // 카메라의 원래 위치

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float moveSpeed = playerAnimator.GetFloat("Move"); // Animator의 Move 파라미터 값 가져오기

        if (moveSpeed > 0)
        {
            float shakeAmount = Mathf.Lerp(walkShakeAmount, runShakeAmount, (moveSpeed - 0.7f) / (1f - 0.7f));
            // Move 값이 0.7과 1 사이일 때 walkShakeAmount와 runShakeAmount 사이를 보간

            float shakeOffsetX = Mathf.Sin(Time.time * shakeFrequency) * shakeAmount;
            float shakeOffsetY = Mathf.Cos(Time.time * shakeFrequency) * shakeAmount;
            // Sin과 Cos 함수를 사용하여 시간에 따라 변화하는 흔들림 값 계산

            transform.localPosition = originalPos + new Vector3(shakeOffsetX, shakeOffsetY, 0);
            // 원래 위치에 흔들림 값을 더하여 카메라 위치 업데이트
        }
        else
        {
            transform.localPosition = originalPos; // 플레이어가 움직이지 않을 때는 카메라를 원래 위치로
        }
    }
}