using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : Item
{
    protected const string Wood = "Wood";
    protected const string Stone = "Stone";
    protected const string Sand = "Sand";
    protected const string Metal = "Metal";
    protected const string WaterFilled = "WaterFilled";
    protected const string Meat = "Meat";

    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private WeaponIndex _index;

    private AudioSource _audioSource;
    protected Camera MainCamera;
    private bool _isPicked = false;

    public Transform LeftHandTarget => _leftHandTarget;
    public WeaponIndex Index => _index;
    public bool IsPicked => _isPicked;
    public void ToggleIsPicked() => _isPicked = !_isPicked;
    public AudioSource AudioSource => _audioSource;

    private void Awake() => _audioSource = GetComponent<AudioSource>();
}