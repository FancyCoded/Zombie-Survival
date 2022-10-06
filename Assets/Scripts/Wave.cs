using UnityEngine;
using System;

[Serializable]
public class Wave
{
    [SerializeField] private WaveProperties _properties;

    private int _zombieCount;

    public WaveProperties Properties => _properties;

    public int GetZombieCount()
    {
        _zombieCount = _properties.ZombieGirlCount + _properties.ZombieRomeroCount + _properties.ZombiePolicemanCount +
            _properties.ZombieMaynardCount + _properties.ZombieSkeletonBossCount;
        return _zombieCount;
    }
}