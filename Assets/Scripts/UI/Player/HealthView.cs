using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Slider _bar;
    [SerializeField] private Health _health;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Image _redScreenSplatter;
    [SerializeField] private float _redScreenSplatterDuration;

    private IEnumerator _changeBarValue;
    private IEnumerator _redScreenSplatterEffect;

    private void OnEnable()
    {
        _health.HealthChanged += OnHealthChanged;
        _health.HealthReduced += OnHealthReduced;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChanged;
        _health.HealthReduced -= OnHealthReduced;
    }

    private void OnHealthReduced()
    {
        if (_redScreenSplatterEffect != null)
            StopCoroutine(_redScreenSplatterEffect);

        _redScreenSplatterEffect = RedScreenSplatter();
        StartCoroutine(_redScreenSplatterEffect);
    }

    private void Start() => _bar.maxValue = _health.MaxAmount;

    private void OnHealthChanged(float targetHealth)
    {
        if (_changeBarValue != null)
            StopCoroutine(_changeBarValue);

        _changeBarValue = ChangeHitPoints(targetHealth);
        StartCoroutine(_changeBarValue);
    }

    private IEnumerator RedScreenSplatter()
    {
        Color color = _redScreenSplatter.color;
        color = new Color(color.r, color.g, color.b, 1);

        for (float runningTime = 0; runningTime < _redScreenSplatterDuration; runningTime += Time.deltaTime)
        {
            float normalizedRunningTime;
            normalizedRunningTime = runningTime / _redScreenSplatterDuration;

            _redScreenSplatter.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), normalizedRunningTime);
            yield return null;
        }
    }
    
    private IEnumerator ChangeHitPoints(float targetHitPoints)
    {
        float deltaFactor = 15;
        float maxDelta = Time.deltaTime * deltaFactor;

        while (_bar.value != targetHitPoints)
        {
            _bar.value = Mathf.MoveTowards(_bar.value, targetHitPoints, maxDelta);
            _healthText.text = _bar.value.ToString("F0");

            yield return null;
        }
    }
}
