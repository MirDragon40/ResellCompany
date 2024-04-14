using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
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


    private SpiderState _state = SpiderState.Idle;

    private Animator _animator;
    private NavMeshAgent _agent;

    public SphereCollider CharacterDetectCollider;
    private Player _targetPlayer;

    public Stat Stat;

    // [Idle]
    public float TraceDetectRange = 8f;
    public float IdleMaxTime = 10f;
    private float _idleTime = 0f;



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
        switch (_state)
        {
            case SpiderState.Idle:
                break;

            case SpiderState.Patrol:
                break;

            case SpiderState.Trace:
                break;

            case SpiderState.Return:
                break;

            case SpiderState.Attack:
                break;

            case SpiderState.Hit:
                break;

            case SpiderState.Death:
                break;

        }
    }

    private void Idle()
    {

    }

}
