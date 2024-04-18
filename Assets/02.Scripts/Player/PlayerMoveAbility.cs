using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private float _gravity = -9.8f;  // 중력 변수
    private float _yVelocity = 0f;   // Y축 속도 (중력 및 점프를 위한 변수)

    private CharacterController _characterController;
    private Animator _animator;

    public bool IsJumping => !_characterController.isGrounded; // 점프 중인지 확인하는 프로퍼티

    public void Start()
    {
        _owner.Stat.Stamina = _owner.Stat.MaxStamina; // 시작 시 스태미나를 최대치로 설정

        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (!Terminal.Instance.IsUsingTerminal)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // '캐릭터가 바라보는 방향'을 기준으로 방향 설정
            Vector3 dir = new Vector3(h, 0, v);
            dir.Normalize();
            dir = Camera.main.transform.TransformDirection(dir); // 글로벌 좌표계로 변환

            // 이동 애니메이션 속도 설정 (Y축은 제외하고 계산)
            _animator.SetFloat("Move", Mathf.Clamp01(new Vector3(h, 0, v).magnitude) * 0.7f);

            // 스테미나 및 달리기 구현
            float moveSpeed = _owner.Stat.MoveSpeed;
            if (Input.GetKey(KeyCode.LeftShift) && _owner.Stat.Stamina > 0)
            {
                moveSpeed = _owner.Stat.RunSpeed;
                _animator.SetFloat("Move", 1);
                _owner.Stat.Stamina -= Time.deltaTime * _owner.Stat.RunConsumeStamina;
            }
            else
            {
                _owner.Stat.Stamina += Time.deltaTime * _owner.Stat.RunRecoveryStamina;
                if (_owner.Stat.Stamina >= _owner.Stat.MaxStamina)
                {
                    _owner.Stat.Stamina = _owner.Stat.MaxStamina;
                }
            }

            // 땅에 닿았는지 확인 후 점프
            if (_characterController.isGrounded)
            {
                _yVelocity = 0; // 땅에 닿으면 Y축 속도를 0으로 초기화

                bool haveJumpStamina = _owner.Stat.Stamina >= _owner.Stat.JumpConsumeStamina;
                if (Input.GetKeyDown(KeyCode.Space) && haveJumpStamina)
                {
                    _yVelocity = _owner.Stat.JumpPower; // 점프력 적용
                    _owner.Stat.Stamina -= _owner.Stat.JumpConsumeStamina;
                }
            }

            // 중력 적용
            _yVelocity += _gravity * Time.deltaTime;
            dir.y = _yVelocity; // 최종적으로 이동 방향에 Y축 속도 (중력 및 점프 포함) 적용

            // 이동 실행
            _characterController.Move(dir * (moveSpeed * Time.deltaTime));
        }
    }

}


