using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicTracker : MonoBehaviour
{
    [SerializeField] private AudioClip[] _actionTracks;
    [SerializeField] private AudioClip _coolDownClip;
    [SerializeField] private WaveGenerator _waveGenerator;

    private AudioSource _audioSource;

    public AudioSource AudioSource => _audioSource;

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    private void OnEnable()
    {
        _waveGenerator.WaveDelayStarted += OnWaveDelayStarted;
        _waveGenerator.WaveStarted += OnWaveStarted;
        _waveGenerator.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        _waveGenerator.WaveDelayStarted -= OnWaveDelayStarted;
        _waveGenerator.WaveStarted -= OnWaveStarted;
        _waveGenerator.GameEnded -= OnGameEnded;
    }

    private void OnGameEnded() => _audioSource.Stop();

    private void OnWaveStarted()
    {
        int randomIndex = Random.Range(1, _actionTracks.Length);
        _audioSource.clip = _actionTracks[randomIndex];
       
        _audioSource.Play();

        _actionTracks[randomIndex] = _actionTracks[0];
        _actionTracks[0] = _audioSource.clip;
    }

    private void OnWaveDelayStarted(int delay) => StartCoroutine(PlayCoolDownClip(delay));

    private IEnumerator PlayCoolDownClip(int waveDelay)
    {
        WaitForSeconds delay = new WaitForSeconds(waveDelay);

        _audioSource.clip = _coolDownClip;
        _audioSource.Play();

        yield return delay;
        _audioSource.Stop();
    }
}