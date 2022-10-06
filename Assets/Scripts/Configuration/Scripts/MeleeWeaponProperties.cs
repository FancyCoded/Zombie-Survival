using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Properties/melee weapon", order = 51)]
public class MeleeWeaponProperties : ItemProperties
{
    [SerializeField] private MeleeWeaponType _type;
    [SerializeField] HoldType _holdType;
    [SerializeField] private AudioClip _meatHitSound;
    [SerializeField] private float _damage;
    [SerializeField] private float _hitDuration;

    public MeleeWeaponType Type => _type;
    public HoldType HoldType => _holdType;
    public AudioClip MeatHitSound => _meatHitSound;
    public float Damage => _damage;
    public float HitDuration => _hitDuration;
}