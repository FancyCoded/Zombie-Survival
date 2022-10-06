using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class StateMachine : MonoBehaviour
{
    [SerializeField] private State _firstState;
   
    private Player _target;
    private State _currentState;

    private void Start()
    {
        _target = GetComponent<Zombie>().Target;
        ResetMachine(_firstState);
    }

    public void Update()
    {
        if (_currentState == null)
            return;

        if (_currentState.TryGetNextState(out State nextState))
            Transit(nextState);
    }

    private void Transit(State nextState)
    {
        if (_currentState != null)
            _currentState.Exit();

        _currentState = nextState;
        _currentState.Enter(_target);
    }

    private void ResetMachine(State startState)
    {
        _currentState = startState;

        if(_currentState != null)
            _currentState.Enter(_target);
    }
}
