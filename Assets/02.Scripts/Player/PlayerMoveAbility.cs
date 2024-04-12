using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{

    private float _gravity = -9.8f;  // 중력 변수
    private float _yVelocity = -1f;

    private CharacterController _characterController;
    private Animator _animator;

    public bool IsJumping => !_characterController.isGrounded;


    public void Start()
    {
        _owner.Stat.Stamina = _owner.Stat.MaxStamina;

        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _animator.SetBool("Crouched", false);
    }

    public void Update()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //  '캐릭터가 바라보는 방향'을 기준으로 방향을 설정.
        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir); // 글로벌 좌표계 (세상의 동서남북)

        
        _animator.SetFloat("Move", dir.magnitude * 0.7f);



        // 중력 적용
        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;

        // 스테미나 구현
        float moveSpeed = _owner.Stat.MoveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && _owner.Stat.Stamina > 0 && !_animator.GetBool("Crouched"))
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

        // 이동속도에 따라 그 방향으로 이동
        _characterController.Move(dir * (moveSpeed * Time.deltaTime));

        bool haveJumpStamina = _owner.Stat.Stamina >= _owner.Stat.JumpConsumeStamina;
        if (_characterController.isGrounded && Input.GetKeyDown(KeyCode.Space) && haveJumpStamina)
        {
            _yVelocity = _owner.Stat.JumpPower;
            _owner.Stat.Stamina -= _owner.Stat.JumpConsumeStamina;
        }

        // 앉기 적용
        if (Input.GetKeyDown(KeyCode.LeftControl) && !_animator.GetBool("Crouched"))
        {
            _animator.SetBool("Crouched", true);
            _animator.SetTrigger("StandingToCrouched");
            


        }
        else if(Input.GetKeyDown(KeyCode.LeftControl) && _animator.GetBool("Crouched"))
        {
            _animator.SetBool("Crouched", false);
            _animator.SetTrigger("CrouchedToStanding");


        }

        if (_animator.GetBool("Crouched"))
        {

        }
    }

}
