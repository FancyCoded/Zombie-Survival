using System.Collections.Generic;
using UnityEngine;

public class AmmunitionSpawner : AmmunitionPool
{
    private const int InitialAmmoCount = 10;
    
    [SerializeField] private Ammunition[] _templates;
    [SerializeField] private List<Transform> _spawnPoints;

    private List<Transform> _spawnedPoints;

    private void Awake()
    {
        Initialize(_templates, InitialAmmoCount);
        _spawnedPoints = new List<Transform>(_spawnPoints.Count);
    }

    public void SpawnAmmunitionsFor(Wave wave)
    {
        for (int i = 0; i < _templates.Length; i++)
        {
            if (_templates[i] is Ammunition ammunition)
            {
                if (ammunition.Properties.Type == AmmunitionType.Caliber45)
                    SpawnAmmunitions(ammunition, wave.Properties.Ammo46Count);

                if (ammunition.Properties.Type == AmmunitionType.Caliber556)
                    SpawnAmmunitions(ammunition, wave.Properties.Ammo556Count);

                if (ammunition.Properties.Type == AmmunitionType.Caliber762)
                    SpawnAmmunitions(ammunition, wave.Properties.Ammo762Count);
            }
        }
    }

    public void SpawnAmmunition(Ammunition template, Vector3 position)
    {
        Ammunition ammunition = GetAmmunition(template);

        ammunition.transform.SetPositionAndRotation(position, ammunition.transform.rotation);
        ammunition.gameObject.SetActive(true);
    }

    private void SpawnAmmunitions(Ammunition template, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, _spawnPoints.Count);
            SpawnAmmunition(template, _spawnPoints[randomIndex].position);

            _spawnedPoints.Add(_spawnPoints[randomIndex]);
            _spawnPoints.RemoveAt(randomIndex);

            if (_spawnPoints.Count == 0)
                for (int x = 0; x < _spawnedPoints.Count; x++)
                    _spawnPoints.Add(_spawnedPoints[x]);
        }
    }
}