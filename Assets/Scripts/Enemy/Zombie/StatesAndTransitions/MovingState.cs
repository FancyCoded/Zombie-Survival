using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Zombie))]
public class MovingState : State
{
    private const string Vertical = "Vertical";

    [SerializeField] private float _speed;

    private Animator _animator;
    private NavMeshAgent _agent;
    private Zombie _zombie;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable() => _animator.SetFloat(Vertical, _speed);

    private void OnDisable() => _animator.SetFloat(Vertical, 0);

    private void Update() =>
        _agent.SetDestination(Target.transform.position);
}
