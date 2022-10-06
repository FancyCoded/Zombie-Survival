using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Health))]
public class Health : MonoBehaviour, IStepHealing
{
    private const float _maxAmount = 100;
    private const float _minAmount = 0;
    private const string Death = "Death";

    private Animator _animator;
    [SerializeField] private float _amount;
    private bool _isDied = false;

    public float Amount => _amount;
    public float MaxAmount => _maxAmount;
    public bool IsDied => _isDied;

    public UnityAction<float> HealthChanged;
    public UnityAction Died;
    public UnityAction HealthReduced;

    private void Awake() => _animator = GetComponent<Animator>();

    private void Start() => HealthChanged?.Invoke(_amount);

    public void TakeDamage(float damage)
    {
        if (_amount > damage)
        {
            _amount = Mathf.Clamp(_amount - damage, _minAmount, _maxAmount);
            HealthChanged?.Invoke(_amount);
            HealthReduced?.Invoke();
        }
        else
            Die();
    }

    public void TakeHeal(float healthAmount, float healStep, float healDelay)
    {
        if(healStep > 0 && healDelay > 0)
            StartCoroutine(StepHealing(healthAmount, healStep, healDelay));
        else
        {
            _amount = Mathf.Clamp(_amount + healthAmount, _minAmount, _maxAmount);
            HealthChanged?.Invoke(_amount);
        }
    }

    public IEnumerator StepHealing(float healtAmount, float healStep, float healDelay)
    {
        WaitForSeconds delay = new WaitForSeconds(healDelay);

        for (float i = healStep; i <= healtAmount; i += healStep)
        {
            _amount = Mathf.Clamp(_amount + healStep, _minAmount, _maxAmount);
            HealthChanged?.Invoke(_amount);
            yield return delay;
        }
    }

    private void Die()
    {
        _isDied = true;
        Died?.Invoke();
        _animator.SetTrigger(Death);
    }
}