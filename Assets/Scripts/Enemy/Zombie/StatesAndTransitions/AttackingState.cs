using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class AttackingState : State
{
    private Zombie _zombie;
    private float _elapsedTime;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _elapsedTime = _zombie.Properties.AttackDealy;
    }

    private void Update()
    {
        if (_zombie.IsDied)
            return;

        if(_elapsedTime >= _zombie.Properties.AttackDealy)
        {
            _zombie.Attack();
            _elapsedTime = 0;
        }

        _elapsedTime += Time.deltaTime;
    }
}