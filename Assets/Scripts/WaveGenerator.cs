using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] private AmmunitionSpawner _ammunitionSpawner;
    [SerializeField] private HealerSpawner _healerSpawner;
    [SerializeField] private WeaponSpawner _weaponSpawner;
    [SerializeField] private ZombieSpawner _zombieSpawner;
    [SerializeField] private List<Wave> _waves;

    private Wave _currentWave;
    private int _currentWaveIndex;
    private int _nextWaveIndex;
    private int _aliveZombieCount;

    public int WaveCount => _waves.Count;

    public UnityAction<int> WaveNumberChanged;
    public UnityAction<int> ZombieCountChanged;
    public UnityAction<int> WaveDelayStarted;
    public UnityAction WaveStarted;
    public UnityAction WaveEnded;
    public UnityAction GameEnded;

    private void Start() =>
        StartCoroutine(StartWaveWithDelay(_waves[0], 0));

    public void OnZombieDied(Zombie zombie)
    {
        if (zombie.Properties.Type == ZombieType.SkeletonBoss)
        {
            GameEnded?.Invoke();
            Zombie[] zombies = _zombieSpawner.GetComponentsInChildren<Zombie>();

            for (int i = 0; i < zombies.Length; i++)
                if (zombies[i].gameObject.activeSelf && zombies[i].Properties.Type != ZombieType.SkeletonBoss)
                    zombies[i].gameObject.SetActive(false);
        }

        zombie.RewardForKill?.Invoke(zombie.Properties.Reward);
        zombie.Died -= OnZombieDied;
        zombie.RewardForKill -= zombie.Target.Wallet.OnRewarded;
        _aliveZombieCount--;
        ZombieCountChanged?.Invoke(_aliveZombieCount);

        if(_aliveZombieCount == 0 && HasNextWave(out _nextWaveIndex))
        {
            WaveEnded?.Invoke();
            StartCoroutine(StartWaveWithDelay(_waves[_nextWaveIndex], _nextWaveIndex));
        }
    }

    private bool HasNextWave(out int nextWaveIndex)
    {
        nextWaveIndex = _currentWaveIndex + 1;
        return _waves.Count > _nextWaveIndex;
    }

    private void InitializeWave(int index)
    {
        int currentWaveNumber;

        _currentWave = _waves[index];
        _currentWaveIndex = index;
        currentWaveNumber = index + 1;
        _aliveZombieCount = _waves[index].GetZombieCount();
        ZombieCountChanged?.Invoke(_aliveZombieCount);

        WaveNumberChanged?.Invoke(currentWaveNumber);

        if (_zombieSpawner)
            _zombieSpawner.SpawnZombiesFor(_currentWave);
    }

    private IEnumerator StartWaveWithDelay(Wave wave, int targetWaveIndex)
    {
        WaitForSeconds startDelay = new WaitForSeconds(wave.Properties.StartDelay);

        _ammunitionSpawner.SpawnAmmunitionsFor(wave);
        _healerSpawner.SpawnHealersFor(wave);
        _weaponSpawner.SpawnWeaponsFor(wave);
        WaveDelayStarted?.Invoke(wave.Properties.StartDelay);
        yield return startDelay;

        InitializeWave(targetWaveIndex);
        WaveStarted?.Invoke();
    }
}