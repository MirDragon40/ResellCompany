using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum SpiderState
{
    Idle,
    Patrol,
    Trace,
    Return,
    Attack,
    Hit,
    Death,
}


public class SpiderMove : MonoBehaviour, IDamaged
{

    public Stat Stat;

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private SpiderState _currentState = SpiderState.Idle;

    // 체력
    public Slider HealthSliderUI;

    // 이동
    public Vector3 StartPosition;

    // 공격
    public const float AttackDelay = 1f;
    private float _attackTimer = 0f;

    // AI
    private Player _target;
    public float FindDistance = 10f;
    public float AttackDistance = 7f;
    public float MoveDistance = 40f;
    public const float TOLERANCE = 0.1f;        //목적지 도착 판단하는 오차 범위 상수
    private const float IDLE_DURATION = 3f;
    private float _idleTimer;

    // 랜덤하게 순찰지점 설정
    private Vector3 Destination;
    public float MovementRange = 30f; // 순찰범위

    // 넉백
    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;



    public void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _target = GetComponent<Player>();


    }

    private void Start()
    {
        Destination = _navMeshAgent.transform.position;
        StartPosition = transform.position;

        Init();
    }

    public void Init()
    {
        _idleTimer = 0f;
        Stat.Health = Stat.MaxHealth;
    }

    private void Update()
    {
        HealthSliderUI.value = (float)Stat.Health / (float)Stat.MaxHealth;
        if (_currentState == SpiderState.Trace || _currentState == SpiderState.Attack)
        {
            LookAtPlayerSmoothly();
        }

        switch (_currentState)
        {
            case SpiderState.Idle:
            {
                Idle();
                break;
            }
            case SpiderState.Trace:
            {
                Trace();
                break;
            }
            case SpiderState.Patrol:
            {
                Patrol();
                break;
            }
            case SpiderState.Return:
            {
                Return();
                break;
            }
            case SpiderState.Attack:
            {
                Attack();
                break;
            }
            case SpiderState.Hit:
            {
                Hit(); 
                break;
            }
            case SpiderState.Death:
            {
                Death(); 
                break;
            }
        }
    }

    private void Idle()
    {
        _idleTimer += Time.deltaTime;

        if (_idleTimer >= IDLE_DURATION)
        {
            _idleTimer = 0f;
            PlayAnimation("Run");
            _currentState = SpiderState.Patrol;
        }

        if (Vector3.Distance(_target.transform.position, transform.position) <= FindDistance && _idleTimer >= IDLE_DURATION / 2)
        {
            PlayAnimation("Run");
            _currentState = SpiderState.Trace;
        }

    }

    private void Trace()
    {
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();

        _navMeshAgent.stoppingDistance = AttackDistance;
        _navMeshAgent.destination = _target.transform.position;

        // 플레이어와의 거리가 공격 범위 내에 있는지 확인
        if (Vector3.Distance(_target.transform.position, transform.position) <= AttackDistance)
        {
            // 공격 범위 내에 있으면 Attack 상태로 전환
            if (_currentState != SpiderState.Attack)   // 현재 상태가 Attack이 아닐 때만 전환
            {
                int index = Random.Range(1, 3);
                PlayAnimation($"Attack{index}");
                _currentState = SpiderState.Attack;
            }
        }
        else if (Vector3.Distance(_target.transform.position, transform.position) >= FindDistance)
        {
            // 플레이어와의 거리가 찾기 범위를 벗어나면 Comeback 상태로 전환
            PlayAnimation("Walk");
            _currentState = SpiderState.Return;
        }

    }


    private void Patrol()
    {

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            MoveToRandomPosition();
        }
        // 플레이어가 감지 범위 내에 있으면 상태를 Trace로 변경하여 플레이어를 추적
        if (Vector3.Distance(_target.transform.position, transform.position) <= FindDistance)
        {
            
            PlayAnimation("Run");
            _currentState = SpiderState.Trace;
        }

        // 추가: Patrol 상태에서 일정 시간 대기 후 Comeback으로 전환
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            StartCoroutine(WaitAndReturn_Coroutine());
        }
    }

    private void Return()
    {
        Vector3 dir = StartPosition - this.transform.position;
        dir.y = 0;
        dir.Normalize();

        _navMeshAgent.stoppingDistance = TOLERANCE;

        _navMeshAgent.destination = StartPosition;

        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= TOLERANCE)
        {
            PlayAnimation("Idle");
            _currentState = SpiderState.Idle;
        }

    }

    private void Attack()
    {

        float distanceToTarget = Vector3.Distance(_target.transform.position, transform.position);
        _attackTimer += Time.deltaTime;

        if (_attackTimer >= AttackDelay)
        {
            Vector3 lookAtTarget = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
            transform.LookAt(lookAtTarget);

            if (distanceToTarget <= AttackDistance)
            {
                int index = Random.Range(1, 3);
                PlayAnimation($"Attack{index}");
            }

            _attackTimer = 0;
        }
        if (distanceToTarget > AttackDistance || distanceToTarget > FindDistance)
        {
            _attackTimer = 0f;
            PlayAnimation("Run");
            _currentState = SpiderState.Trace;
        }

    }

    private void Hit()
    {
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;

            Vector3 dir = transform.position - _target.transform.position;
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }
        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);
        PlayAnimation("Hit");
        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;
            PlayAnimation("Run");
            _currentState = SpiderState.Trace;
        }
    }

   public void Damaged(int damage)
    {
        Stat.Health -= damage;
        if(Stat.Health <= 0)
        {
            _currentState = SpiderState.Death;
            Death();
        }
        else
        {
            PlayAnimation("Hit");
            _currentState = SpiderState.Hit;
        }
    }


    private Coroutine _dieCoroutine;
    public void Death()
    {
        PlayAnimation("Death");
        if(_dieCoroutine == null)
        {
            _dieCoroutine = StartCoroutine(Death_Coroutine());
        }
    }
    private IEnumerator Death_Coroutine()
    {
        _navMeshAgent.isStopped = true;
        _navMeshAgent.ResetPath();

        yield return new WaitForSeconds(3f);

        HealthSliderUI.gameObject.SetActive(false);
        Destroy(gameObject);

    }


    public void PlayerAttack()
    {
        IDamaged playerDamaged = _target.GetComponent<IDamaged>();

        if(playerDamaged != null)
        {
            playerDamaged.Damaged(Stat.Damage);
            _attackTimer = 0f;
        }
    }

    void LookAtPlayerSmoothly()
    {
        Vector3 directionToTarget = _target.transform.position - transform.position;
        directionToTarget.y = 0; //수평 회전만
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5); // 회전 속도 조절
    }

    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }

    private void MoveToRandomPosition()
    {
        // 일정 범위 내에서 랜덤한 위치로 이동
        Vector3 randomDirection = Random.insideUnitSphere * MovementRange;
        randomDirection += _navMeshAgent.destination;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, MovementRange, NavMesh.AllAreas);
        Vector3 targetPosition = hit.position;
        _navMeshAgent.SetDestination(targetPosition);
        Destination = targetPosition;

    }

    private IEnumerator WaitAndReturn_Coroutine()
    {
        yield return new WaitForSeconds(2f);
        PlayAnimation("Walk");
        _currentState = SpiderState.Return;
    }


}
