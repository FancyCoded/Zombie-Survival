using UnityEngine;

[RequireComponent(typeof(MoveTransitToAttack))]
public class AttackTransitToMove : Transition
{
    [SerializeField] private float _extraTransitionDistance = 0.5f;

    private float _distance;
    private MoveTransitToAttack _moveTransitToAttack;

    private void Start() =>
        _moveTransitToAttack = GetComponent<MoveTransitToAttack>();

    private void Update()
    {
        _distance = Vector3.Distance(transform.position, Target.transform.position);

        if (_distance - _extraTransitionDistance > _moveTransitToAttack.TargetDistance)
            NeedTransit = true;
    }
}