using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveTimer : MonoBehaviour
{
    private const string WaveIncomingTrigger = "WaveIncoming";
    private const string WaveIncoming = "wave imcoming\n get ready!";
    private const string WaveCompleted = "wave completed\n get to the trader\n";

    [SerializeField] private Image _timerImage;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _infoText;
    [SerializeField] private Animator _animator;
    [SerializeField] private WaveGenerator _waveGenerator;
    [SerializeField] private TMP_Text _tip;

    private void Awake()
    {
        _timer.gameObject.SetActive(false);
        _timerImage.gameObject.SetActive(false);
        _infoText.gameObject.SetActive(false);
        _tip.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _waveGenerator.WaveDelayStarted += OnWaveDelayStarted;
        _waveGenerator.WaveEnded += OnWaveEnded;
    }

    private void OnDisable()
    {
        _waveGenerator.WaveDelayStarted -= OnWaveDelayStarted;
        _waveGenerator.WaveEnded += OnWaveEnded;
    }

    private void OnWaveEnded() => StartCoroutine(WaveEnded());

    private IEnumerator WaveEnded()
    {
        WaitForSeconds activeDelay = new WaitForSeconds(2);
        WaitForSeconds tipActiveDelay = new WaitForSeconds(10);

        _infoText.text = WaveCompleted;
        _infoText.gameObject.SetActive(true);
        _tip.gameObject.SetActive(true);

        yield return activeDelay;
        _infoText.gameObject.SetActive(false);

        yield return tipActiveDelay;
        _tip.gameObject.SetActive(false);
    }

    private void OnWaveDelayStarted(int startDelaySeconds) => StartCoroutine(Timer(startDelaySeconds));

    private IEnumerator Timer(int seconds)
    {
        WaitForSeconds second = new WaitForSeconds(1);
        WaitForSeconds activeDelay = new WaitForSeconds(4.5f);

        _timer.gameObject.SetActive(true);
        _timerImage.gameObject.SetActive(true);

        for (int i = seconds; i >= 0; i--)
        {
            _timer.text = i.ToString();
            yield return second;
        }

        _timer.gameObject.SetActive(false);
        _timerImage.gameObject.SetActive(false);
        _infoText.text = WaveIncoming;
        _infoText.gameObject.SetActive(true);
        _animator.SetTrigger(WaveIncomingTrigger);

        yield return activeDelay;

        _infoText.gameObject.SetActive(false);
    }
}
