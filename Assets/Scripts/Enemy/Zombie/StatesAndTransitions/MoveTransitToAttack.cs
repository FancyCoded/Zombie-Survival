using UnityEngine;

public class MoveTransitToAttack : Transition
{
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    private float _randomDistance;

    public float TargetDistance { get; private set; }

    private void Start() =>
        _randomDistance = Random.Range(_minDistance, _maxDistance);

    private void Update()
    {
        TargetDistance = Vector3.Distance(transform.position, Target.transform.position);

        if (TargetDistance <= _randomDistance)
            NeedTransit = true;
    }
}
