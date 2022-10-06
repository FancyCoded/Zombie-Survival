using System.Collections.Generic;
using UnityEngine;

public class HealerSpawner : HealerPool
{
    private const int InitialHealerCount = 10;

    [SerializeField] private Healer[] _templates;
    [SerializeField] private List<Transform> _spawnPoints;
    private List<Transform> _spawnedPoints;

    private void Awake()
    {
        Initialize(_templates, InitialHealerCount);
        _spawnedPoints = new List<Transform>(_spawnPoints.Count);
    }

    public void SpawnHealersFor(Wave wave)
    {
        for (int i = 0; i < _templates.Length; i++)
        {
            if (_templates[i] is AidKit aidKit)
                SpawnHealers(aidKit, wave.Properties.AidKitCount);

            if (_templates[i] is Painkiller painkiller)
                SpawnHealers(painkiller, wave.Properties.PainkillerCount);

            if (_templates[i] is EnergyDrink energyDrink)
                SpawnHealers(energyDrink, wave.Properties.EnergyDrinkCount);

            if (_templates[i] is Bandage bandage)
                SpawnHealers(bandage, wave.Properties.BandageCount);
        }
    }

    public void SpawnHealer(Healer template, Vector3 position)
    {
        Healer healer = GetHealer(template);

        healer.transform.SetPositionAndRotation(position, healer.transform.rotation);
        healer.gameObject.SetActive(true);
    }

    private void SpawnHealers(Healer template, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, _spawnPoints.Count);
            SpawnHealer(template, _spawnPoints[randomIndex].position);

            _spawnedPoints.Add(_spawnPoints[randomIndex]);
            _spawnPoints.RemoveAt(randomIndex);

            if (_spawnPoints.Count == 0)
                for (int x = 0; x < _spawnedPoints.Count; x++)
                    _spawnPoints.Add(_spawnedPoints[x]);
        }
    }
}