using UnityEngine;

[CreateAssetMenu(menuName = "Wave/wave properties", order = 51)]
public class WaveProperties : ScriptableObject
{
    [Header("Ammunitions")]
    [SerializeField, Range(0, 3)] private int _ammo556Count;
    [SerializeField, Range(0, 3)] private int _ammo45Count;
    [SerializeField, Range(0, 3)] private int _ammo762Count;
    
    [Header("Heal")]
    [SerializeField, Range(0, 1)] private int _aidKitCount;
    [SerializeField, Range(0, 2)] private int _painkillerCount;
    [SerializeField, Range(0, 3)] private int _bandageCount;
    [SerializeField, Range(0, 3)] private int _energyDrinkCount;

    [Header("Weapon")]
    [SerializeField, Range(0, 1)] private int _weaponAKMCount;
    [SerializeField, Range(0, 1)] private int _weaponM4A4Count;
    [SerializeField, Range(0, 1)] private int _weaponM4A1SCount;
    [SerializeField, Range(0, 1)] private int _weaponColtCount;
    [SerializeField, Range(0, 1)] private int _weaponKnifeCount;

    [Header("Zombie")]
    [SerializeField, Range(0, 100)] private int _zombieRomeroCount;
    [SerializeField, Range(0, 70)] private int _zombieGirlCount;
    [SerializeField, Range(0, 50)] private int _zombiePoliceCount;
    [SerializeField, Range(0, 20)] private int _zombieMaynardCount;
    [SerializeField, Range(0, 1)] private int _zombieSkeletonBossCount;

    [Header("Other")]
    [SerializeField] private float _zombieSpawnDelay;
    [SerializeField] private int _startDelay;

    public int Ammo556Count => _ammo556Count;
    public int Ammo46Count => _ammo45Count;
    public int Ammo762Count => _ammo762Count;
    public int AidKitCount => _aidKitCount;
    public int PainkillerCount => _painkillerCount;
    public int BandageCount => _bandageCount;
    public int EnergyDrinkCount => _energyDrinkCount;
    public int WeaponAKMCount => _weaponAKMCount; 
    public int WeaponM4A4Count => _weaponM4A4Count; 
    public int WeaponM4A1SCount => _weaponM4A1SCount;
    public int WeaponColtCount => _weaponColtCount;
    public int WeaponKnifeCount => _weaponKnifeCount;
    public int ZombieRomeroCount => _zombieRomeroCount; 
    public int ZombieGirlCount => _zombieGirlCount;
    public int ZombiePolicemanCount => _zombiePoliceCount;
    public int ZombieMaynardCount => _zombieMaynardCount; 
    public int ZombieSkeletonBossCount => _zombieSkeletonBossCount;
    public int StartDelay => _startDelay;
    public float ZombieSpawnDelay => _zombieSpawnDelay;
}
