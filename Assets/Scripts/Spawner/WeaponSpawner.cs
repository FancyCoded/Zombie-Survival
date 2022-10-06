using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : WeaponPool
{
    private const int InitialWeaponCount = 1;

    [SerializeField] private Weapon[] _templates;
    [SerializeField] private List<Transform> _spawnPoints;

    private List<Transform> _spawnedPoints;

    public void Awake()
    {
        Initialize(_templates, InitialWeaponCount);
        _spawnedPoints = new List<Transform>(_spawnPoints.Count);
    }

    public void SpawnWeaponsFor(Wave wave)
    {
        for (int i = 0; i < _templates.Length; i++)
        {
            if(_templates[i] is FireArm fireArm)
            {
                if (fireArm.Properties.Type == FireArmType.M4A4)
                    SpawnWeapons(fireArm, wave.Properties.WeaponM4A4Count);

                if (fireArm.Properties.Type == FireArmType.AKM)
                    SpawnWeapons(fireArm, wave.Properties.WeaponAKMCount);

                if (fireArm.Properties.Type == FireArmType.M4A1S)
                    SpawnWeapons(fireArm, wave.Properties.WeaponM4A1SCount);

                if (fireArm.Properties.Type == FireArmType.Colt)
                    SpawnWeapons(fireArm, wave.Properties.WeaponColtCount);
            }

            if(_templates[i] is MeleeWeapon meleeWeapon)
            {
                if (meleeWeapon.Properties.Type == MeleeWeaponType.Knife)
                    SpawnWeapons(meleeWeapon, wave.Properties.WeaponKnifeCount);
            }
        }
    }

    public void SpawnWeapon(Weapon weapon, Vector3 position, Quaternion rotation)
    {
        Weapon newWeapon = GetWeapon(weapon);

        newWeapon.transform.SetPositionAndRotation(position, rotation);
        newWeapon.gameObject.SetActive(true);
    }

    public Weapon SpawnAndGetWeapon(Weapon weapon)
    {
        Weapon newWeapon = GetWeapon(weapon);

        newWeapon.gameObject.SetActive(true);
        return newWeapon;
    }

    private void SpawnWeapons(Weapon weapon, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, _spawnPoints.Count);
            SpawnWeapon(weapon, _spawnPoints[randomIndex].position, Quaternion.Euler(0, 0, 60));

            _spawnedPoints.Add(_spawnPoints[randomIndex]);
            _spawnPoints.RemoveAt(randomIndex);
            
            if (_spawnPoints.Count == 0)
                for (int x = 0; x < _spawnedPoints.Count; x++)
                    _spawnPoints.Add(_spawnedPoints[x]);
        }
    }
}