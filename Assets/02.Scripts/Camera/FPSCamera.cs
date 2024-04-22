using UnityEngine;

// 1인칭 슈팅 

public class FPSCamera : MonoBehaviour
{


    public float RotationSpeed = 200;  // 초당 200초까지 회전 가능한 속도
    // 누적할 x각도와 y 각도
    public float _mx = 0;
    public float _my = 0;

    /** 카메라 이동 **/
    // 목표: 카메라를 캐릭터의 눈으로 이동시키고싶다.
    // 필요속성:
    // - 캐릭터의 눈 위치
    public Transform Target;


    private void Start()
    {
        // 마우스 커서를 숨기는 코드
        Cursor.visible = false;
        // 마우스를 고정시키는 코드
        Cursor.lockState = CursorLockMode.Locked;
        transform.localPosition = Target.transform.position;
    }

    private void LateUpdate()

    {
        /*
        if (GameManager.Instance.State != GameState.Go)
        {
            return;
        }
        */

        // 1. 캐릭터의 눈 위치로 카메라를 이동시킨다. 
        transform.localPosition = Target.transform.position;
        //Debug.Log(Target.transform.position);
        // 1. 마우스를 입력(Drag) 받는다.
        float mouseX = Input.GetAxis("Mouse X");  // 방향에 따라 -1 ~ 1 사이의 값 변환
        float mouseY = Input.GetAxis("Mouse Y");

        // 2. 마우스 입력 값을 이용해 회전 방향을 구한다. 
        Vector3 rotationDir = new Vector3(mouseX, mouseY, z: 0);
        // rotationDir.Normalize();   // 정규화


        _mx += +rotationDir.x * RotationSpeed * Time.deltaTime;
        _my += +rotationDir.y * RotationSpeed * Time.deltaTime;


        _my = Mathf.Clamp(value: _my, min: -90f, max: 90f);


        transform.eulerAngles = new Vector3(x: -_my, y: _mx, z: 0);



       

    }
}
