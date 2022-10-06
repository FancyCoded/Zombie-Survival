using TMPro;
using UnityEngine;

public class WaveView : MonoBehaviour
{
    [SerializeField] private WaveGenerator _waveGenerator;
    [SerializeField] private TMP_Text _currentWave;
    [SerializeField] private TMP_Text _waveCount;
    [SerializeField] private TMP_Text _aliveZombieCount;
    
    private void OnEnable()
    {
        _waveGenerator.WaveNumberChanged += OnWaveNumberChanged;
        _waveGenerator.ZombieCountChanged += OnZombieCountChanged;

        _waveCount.text = _waveGenerator.WaveCount.ToString();
    }

    private void OnDisable()
    {
        _waveGenerator.WaveNumberChanged -= OnWaveNumberChanged;
        _waveGenerator.ZombieCountChanged -= OnZombieCountChanged;
    }
    
    private void OnZombieCountChanged(int count) => _aliveZombieCount.text = count.ToString();

    private void OnWaveNumberChanged(int number) => _currentWave.text = number.ToString();
}
