using UnityEngine;

public class TraderPathFinder : MonoBehaviour
{
    [SerializeField] private ParticleSystem _pathFinder;
    [SerializeField] private WaveGenerator _waveGenerator;
    [SerializeField] private Trader _trader;
    [SerializeField] private PlayerInput _playerInput;

    private void OnEnable()
    {
        _waveGenerator.WaveEnded -= OnWaveEnded;
        _waveGenerator.WaveStarted -= OnWaveStarted;
        _playerInput.TraderFound -= OnTraderFound;
    }

    private void OnDisable()
    {
        _waveGenerator.WaveEnded -= OnWaveEnded;
        _waveGenerator.WaveStarted -= OnWaveStarted;
        _playerInput.TraderFound -= OnTraderFound;
    }
   
    private void OnTraderFound() =>
        _pathFinder.gameObject.SetActive(false);

    private void OnWaveStarted() =>
        _pathFinder.gameObject.SetActive(false);

    private void OnWaveEnded() =>
        _pathFinder.gameObject.SetActive(true);
}
