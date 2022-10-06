using UnityEngine;

[CreateAssetMenu(menuName ="Weapon Properties/firearm", order = 51)]
public class FireArmProperties : ItemProperties
{
    [SerializeField] private HoldType _holdType;
    [SerializeField] private FireArmType _type;
    [SerializeField] private AmmunitionType _ammunitionType;
    [SerializeField] private ShootType _shootType;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _damage;
    [SerializeField] private int _shootRange;
    [SerializeField] private int _magazineSize;
    [SerializeField] private AmmunitionProperties _ammunitionProperties;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private ParticleSystem _meatHitEffect;
    [SerializeField] private GameObject _woodHitEffect;
    [SerializeField] private GameObject _metalHitEffect;
    [SerializeField] private GameObject _sandHitEffect;
    [SerializeField] private GameObject _stoneHitEffect;
    [SerializeField] private GameObject _barrelHitEffect;

    public AudioClip ShootSound => _shootSound;
    public AudioClip ReloadSound => _reloadSound;
    public HoldType HoldType => _holdType;
    public FireArmType Type => _type;
    public ShootType ShootType => _shootType;
    public AmmunitionProperties AmmunitionProperties => _ammunitionProperties;

    public float ShootDelay => _shootDelay;
    public float Damage => _damage;
    public int ShootRange => _shootRange;
    public int MagazineSize => _magazineSize;

    public GameObject WoodHitEffect => _woodHitEffect;
    public GameObject MetalHitEffect => _metalHitEffect;
    public GameObject SandHitEffect => _sandHitEffect;
    public GameObject StoneHitEffect => _stoneHitEffect;
    public GameObject BarrelHitEffect => _barrelHitEffect;
    public ParticleSystem MeatHitEffect => _meatHitEffect;
}