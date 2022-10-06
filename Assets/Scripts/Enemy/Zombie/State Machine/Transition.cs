using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    public Player Target { get; private set; }
    public State TargetState => _targetState;
    public bool NeedTransit { get; protected set; }

    private void OnEnable() => NeedTransit = false;

    public void Initialize(Player target) => Target = target;
}
