using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public abstract class Zombie : MonoBehaviour
{
    private const string Attacking = "Attack";
    private const string Death = "Death";
    private const int IgnoreRaycastLayerIndex = 2;
    private const int ZombieLayerIndex = 8;

    [SerializeField] private ZombieProperties _properties;
    [SerializeField] private EnemyHitMarker _hitMarker;

    private Player _target;
    private AudioSource _audioSource;
    private Animator _animator;
    private float _health;
    private float _minHealth = 0;
    private float _maxHealth;
    private bool _isDied;

    public Player Target => _target;
    public ZombieProperties Properties => _properties;
    public bool IsDied => _isDied;

    public UnityAction<Zombie> Died;
    public UnityAction<int> RewardForKill;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _maxHealth = _health = _properties.Health;
    }

    private void OnEnable() =>
        _audioSource.Play();

    public void TakeDamage(float damage)
    {
        if (_health > damage)
            _health = Mathf.Clamp(_health - damage, _minHealth, _maxHealth);
        else
            StartCoroutine(Die());
    }

    public void Initialize(Player player) => _target = player;

    public void Attack()
    {
        transform.LookAt(_target.transform.position);
        _animator.SetTrigger(Attacking);

        StartCoroutine(DamageTarget());
    }

    private IEnumerator DamageTarget()
    {
        WaitForSeconds seconds = new WaitForSeconds(_properties.SecondsBeforeDamageTarget);

        yield return seconds;

        if (_hitMarker.IsCollidedPlayer)
            _target.TakeDamage(_properties.Damage);
    }

    private IEnumerator Die()
    {
        WaitForSeconds seconds = new WaitForSeconds(_properties.SecondsBeforeDestroy);
        gameObject.layer = IgnoreRaycastLayerIndex;
        _isDied = true;

        Died?.Invoke(this);
        RewardForKill?.Invoke(_properties.Reward);
        _animator.SetTrigger(Death);
        _audioSource.Stop();
        yield return seconds;
        
        gameObject.SetActive(false);
        gameObject.layer = ZombieLayerIndex;
        _isDied = false;
        gameObject.layer = ZombieLayerIndex;
        _health = _maxHealth = _properties.Health;
    }
}