using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon, IDamageable
{
    private const string IsStabbing = "IsStabbing";

    [SerializeField] private MeleeWeaponProperties _properties;
    [SerializeField] private PlayerWeaponHitMarker _hitMarker;
    
    private Animator _animator;
    private IEnumerator _stabbing;
    private float _lastHitTime;

    public MeleeWeaponProperties Properties => _properties;
    public PlayerWeaponHitMarker HitMarker => _hitMarker;

    public void Initialize(Animator animator) => _animator = animator;

    public void Hit()
    {
        if(_lastHitTime + _properties.HitDuration < Time.time)
        {
            if (Properties.Type == MeleeWeaponType.Knife)
            {
                if (_stabbing != null)
                    StopCoroutine(_stabbing);

                _stabbing = Stabbing();
                StartCoroutine(_stabbing);
            }

            _lastHitTime = Time.time;
        }
    }

    public void EnemyHitted()
    {
        AudioSource.PlayOneShot(_properties.MeatHitSound);
        HitMarker.Zombie.TakeDamage(_properties.Damage);
    }

    private IEnumerator Stabbing()
    {
        _animator.SetBool(IsStabbing, true);
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool(IsStabbing, false);
    }
}