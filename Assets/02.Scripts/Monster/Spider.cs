using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Spider : MonoBehaviour, IDamaged
{
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

    public Canvas MyCanvas;
    public Slider HealthSliderUI;


    private SpiderState _state = SpiderState.Idle;

    private Animator _animator;
    private NavMeshAgent _agent;

    public SphereCollider CharacterDetectCollider;
    private Player _target;

    public Stat Stat;

    // [Idle]
    public float TraceDetectRange = 8f;
    public float IdleMaxTime = 10f;
    private float _idleTime = 0f;

    // [Patrol]
    public Transform PatrolDestination;

    // [Return]
    private Vector3 _startPosition;

    // [Attack]
    public float AttackDistance = 3f;
    private float _attackTimer = 0f;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CharacterDetectCollider.radius = TraceDetectRange;
        Stat.Init();
    }

    private void Update()
    {
        MyCanvas.transform.forward = Camera.main.transform.forward;
        HealthSliderUI.value = (float)Stat.Health / Stat.MaxHealth;

        if (_state == SpiderState.Trace || _state == SpiderState.Attack)
        {
            LookAtPlayerSmoothly();
        }

        switch (_state)
        {
            case SpiderState.Idle:
                {
                    Idle();
                    break;
                }

            case SpiderState.Patrol:
                {
                    Patrol();
                    break;
                }

            case SpiderState.Trace:
                {
                    Return();
                    break;
                }

            case SpiderState.Return:
                {
                    Trace();
                    break;
                }

            case SpiderState.Attack:
                {
                    Attack();
                    break;
                }

            case SpiderState.Hit:
                {
                    Hit(Stat.Damage);
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
        _idleTime += Time.deltaTime;
        if(_idleTime >= IdleMaxTime)
        {
            _idleTime = 0f;
            SetRandomPatrolDestination();
            _state = SpiderState.Patrol;
            PlayAnimation("Walk");
            Debug.Log("Idle -> Patrol");
        }

        _target = FindTarget(TraceDetectRange);
        if(_target != null)
        {
            _startPosition = transform.position;
            SetRandomPatrolDestination();
            _state = SpiderState.Trace;
            PlayAnimation("Walk");
            Debug.Log("Idle -> Trace");
        }
    }

    private void Patrol()
    {
        if(PatrolDestination == null)
        {
           // PatrolDestination = GameObject.Find("Patrol").transform;
        }

        // [패트롤 구역]까지 간다.
        //_agent.destination = PatrolDestination.position;
        //_agent.stoppingDistance = 0f;

        // IF [플레이어]가 [감지 범위]안에 들어오면 플레이어 (추적 상태로 전이)
        _target = FindTarget(TraceDetectRange);
        if (_target != null)
        {
            _state = SpiderState.Trace;
            PlayAnimation("Walk");
            Debug.Log("Patrol -> Trace");
        }

        // IF [패트롤 구역]에 도착하면 (복귀 상태로 전이)
        if (!_agent.pathPending && _agent.remainingDistance <= 0.1f)
        {
            _state = SpiderState.Return;
            PlayAnimation("Walk");
            Debug.Log("Patrol -> Return");
        }

    }


    private void Return()
    {
        // [시작 위치]까지 간다. 
        _agent.destination = _startPosition;
        _agent.stoppingDistance = 0f;

        if (!_agent.pathPending && _agent.remainingDistance <= 0.1f)
        {
            _state = SpiderState.Idle;
            PlayAnimation("Idle");
            Debug.Log("Return -> Idle");
        }

        // IF [플레이어]가 [감지 범위]안에 들어오면 플레이어 (추적 상태로 전이)
        _target = FindTarget(TraceDetectRange);
        if(_target != null)
        {
            _state = SpiderState.Trace;
            PlayAnimation("Run");
            Debug.Log("Return -> Trace");
        }
    }

    private void Trace()
    {
        // 타겟이 게임에서 나가면 복귀
        if(_target == null)
        {
            Debug.Log("Trace -> Return");
            _state = SpiderState.Return;
            return;
        }

        // 타겟이 너무 멀어지면 복귀
        _agent.destination = _target.transform.position;
        if(GetDistance(_target.transform) > TraceDetectRange)
        {
            Debug.Log("Trace -> Patrol");
            _startPosition = transform.position;
            SetRandomPatrolDestination();
            _state = SpiderState.Patrol;
            return;
        }

        // 타겟이 가까우면 공격 상태로 전이 
        if(GetDistance(_target.transform) <= AttackDistance)
        {
            _agent.isStopped = true;
            Debug.Log("Trace -> Attack");
            _animator.Play("Idle");

            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.stoppingDistance = AttackDistance;

            _state = SpiderState.Attack;
            return;
        }
    }

    private void Attack()
    {
        // 타겟이 null이면 복귀
        if(_target == null)
        {
            Debug.Log("Trace -> Return");
            _agent.isStopped = false;
            _startPosition = transform.position;
            SetRandomPatrolDestination();
            _state = SpiderState.Trace;
            return;
        }

        // 타겟이 공격 범위에서 벗어나면 복귀
        _agent.destination = _target.transform.position;
        if(GetDistance(_target.transform) > AttackDistance)
        {
            Debug.Log("Trace -> Return");
            _agent.isStopped = false;
            _startPosition = transform.position;
            _state = SpiderState.Idle;
            return;
        }

        _attackTimer += Time.deltaTime;
        if(_attackTimer >= Stat.AttackCoolTime)
        {
            transform.LookAt(_target.transform);
            _attackTimer = 0f;
            PlayerAttack();

            int index = Random.Range(1, 3);
            PlayAnimation($"Attack{index}");
        }
    }



    private void Hit(int damage)
    {
        Damaged(damage);
    }


    public void Damaged(int damage)
    {
        if(_state == SpiderState.Death)
        {
            return;
        }


    }

    private void Death()
    {
        _state = SpiderState.Death;
        PlayAnimation("Death");

        StartCoroutine(Death_Coroutine());
    }

    private IEnumerator Death_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }


    // [보조 메서드]

    private Player FindTarget(float distance)
    {
        // 대상 캐릭터가 null이 아니고, 죽지 않았으며, 특정 거리 내에 있는지 확인
        if (_target != null  && Vector3.Distance(transform.position,_target.transform.position) <= distance)
        {
            return _target;
        }

        return null;
    }

    private void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }

    private float GetDistance(Transform otherTransform)
    {
        return Vector3.Distance(transform.position, otherTransform.position);
    }

    public void PlayerAttack()
    {
        IDamaged playerDamaged = _target.GetComponent<IDamaged>();
        if(playerDamaged != null)
        {
            playerDamaged.Damaged(_target.Stat.Damage);

        }
    }

    private void SetRandomPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10; // 반경 10m 내의 랜덤한 방향과 거리
        randomDirection += transform.position; // 현재 몬스터의 위치에 이를 더해 랜덤 위치를 얻음

        NavMeshHit hit; // NavMesh 샘플링 결과를 저장할 구조체
        Vector3 finalPosition = Vector3.zero; // 최종 목적지 위치

        // NavMesh.SamplePosition: 주어진 위치에서 가장 가까운 NavMesh 위의 한 점 찾는다.
        // randomDirection: 랜덤 위치, hit: 샘플링 결과, 10.0f: 최대 거리, NavMesh.AllAreas: 모든 영역
        if (NavMesh.SamplePosition(randomDirection, out hit, 10.0f, NavMesh.AllAreas))
        {
            finalPosition = hit.position; // 샘플링에 성공했다면, 해당 위치를 최종 목적지로 설정
        }

        // 여기서 finalPosition을 사용하여 몬스터의 목적지를 설정
        // 예를 들어, NavMeshAgent 컴포넌트를 사용하는 경우:
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(finalPosition); // 최종 목적지로 이동을 시작
        }
    }

    void LookAtPlayerSmoothly()
    {
        Vector3 directionToTarget = _target.transform.position - transform.position;
        directionToTarget.y = 0; //수평 회전만
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5); // 회전 속도 조절
    }

}
