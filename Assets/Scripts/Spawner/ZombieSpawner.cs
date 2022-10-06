using System.Collections;
using UnityEngine;

public class ZombieSpawner : ZombiePool
{
    private const int InitialRomeroCount = 40;
    private const int InitialGirlCount = 40;
    private const int InitialPolicemanCount = 30;
    private const int InitialMaynardCount = 20;
    private const int InitialSkeletonBossCount = 1;

    [SerializeField] private WaveGenerator _waveGenerator;
    [SerializeField] private Zombie[] _templates;
    [SerializeField] private Transform[] _spawnPoints;

    private void OnEnable() => Player.Health.Died += OnTargetDied;

    private void OnDisable() => Player.Health.Died -= OnTargetDied;

    public void Awake() =>
        Initialize(_templates, InitialRomeroCount, InitialGirlCount, InitialPolicemanCount, InitialMaynardCount, InitialSkeletonBossCount);

    public void SpawnZombiesFor(Wave wave)
    {
        float delay = wave.Properties.ZombieSpawnDelay;

        for (int i = 0; i < _templates.Length; i++)
        {
            if (_templates[i] is Romero romero)
                StartCoroutine(SpawnZombiesByDelay(romero, wave.Properties.ZombieRomeroCount, delay));

            if (_templates[i] is Girl girl)
                StartCoroutine(SpawnZombiesByDelay(girl, wave.Properties.ZombieGirlCount, delay));

            if (_templates[i] is Policeman policeman)
                StartCoroutine(SpawnZombiesByDelay(policeman, wave.Properties.ZombiePolicemanCount, delay));

            if (_templates[i] is Maynard maynard)
                StartCoroutine(SpawnZombiesByDelay(maynard, wave.Properties.ZombieMaynardCount, delay));

            if (_templates[i] is SkeletonBoss skeletonBoss)
                StartCoroutine(SpawnZombiesByDelay(skeletonBoss, wave.Properties.ZombieSkeletonBossCount, delay));
        }
    }

    public void SpawnZombie(Zombie template, Vector3 position, Quaternion rotation)
    {
        Zombie zombie = GetZombie(template);

        zombie.Initialize(Player);
        zombie.Died += _waveGenerator.OnZombieDied;
        zombie.RewardForKill += Player.Wallet.OnRewarded;
        zombie.transform.SetPositionAndRotation(position, rotation);
        zombie.gameObject.SetActive(true);
    }
  
    private IEnumerator SpawnZombiesByDelay(Zombie template, int count, float delay)
    {
        WaitForSeconds spawnDelay = new WaitForSeconds(delay);
        int randomIndex;

        for (int i = 0; i < count; i++)
        {
            randomIndex = Random.Range(1, _spawnPoints.Length);
            SpawnZombie(template, _spawnPoints[randomIndex].position, Quaternion.identity);
            yield return spawnDelay;
        }
    }

    private void OnTargetDied() => StopAllCoroutines();
}