using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] private List<Transition> _transitions;

    protected Player Target;

    public void Enter(Player target)
    {
        if(enabled == false)
        {
            enabled = true;
            Target = target;

            foreach (var transition in _transitions)
            {
                transition.enabled = true;
                transition.Initialize(Target);
            }
        }
    }

    public void Exit()
    {
        foreach (var transition in _transitions)
            transition.enabled = false;

        enabled = false;
    }

    public bool TryGetNextState(out State state)
    {
        state = null;

        foreach (var transition in _transitions)
            if (transition.NeedTransit)
                return state = transition.TargetState;

        return state != null;
    }
}
