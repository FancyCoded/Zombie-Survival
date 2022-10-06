using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IdleState : State
{
    private const string Vertical = "Vertical";

    private Animator _amimator;

    private void Awake() => _amimator = GetComponent<Animator>();

    private void OnEnable() => _amimator.SetFloat(Vertical, 0f);

    private void OnDisable() => _amimator.SetFloat(Vertical, 0);
}
