using UnityEngine;

[CreateAssetMenu(menuName = "Zombie/properties", order = 51)]
public class ZombieProperties : ScriptableObject
{
    [SerializeField] private ZombieType _zombieType;
    [SerializeField] private float _health;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _damage;
    [SerializeField] private float _secondsBeforeDamage;
    [SerializeField] private float _secondsBeforeDestroy;
    [SerializeField] private int _reward;

    public ZombieType Type => _zombieType;
    public float Health => _health;
    public float AttackDealy => _attackDelay;
    public float Damage => _damage;
    public float SecondsBeforeDamageTarget => _secondsBeforeDamage;
    public float SecondsBeforeDestroy => _secondsBeforeDestroy;
    public int Reward => _reward;
}