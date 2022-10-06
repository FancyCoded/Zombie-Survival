using UnityEngine;
using System;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Movement _playerMovement;
    [SerializeField] private Player _player;
    [SerializeField] private Part[] _parts = new Part[4];
    [SerializeField] private float _spreadSpeed;
    [SerializeField] private float _spreadSpeedFactor;
    [SerializeField] private float _normalSpread = 20;

    private float _currentSpread;
    private float _targetSpread;
    private float _spreaderpTime;

    private void Update()
    {
        if (_player.IsAiming)
            _targetSpread = _normalSpread; 
        if (_playerMovement.PlayerMovement > 0 && _player.IsAiming == false)
            _targetSpread = (_playerMovement.PlayerMovement + _normalSpread) * _spreadSpeed;
        else
            _targetSpread = _normalSpread;

        SetSpread();
    }

    private void SetSpread()
    {
        _spreaderpTime = _spreadSpeed * _spreadSpeedFactor;
        _currentSpread = Mathf.Lerp(_currentSpread, _targetSpread, _spreaderpTime);

        foreach (var part in _parts)
            part.RectTransform.anchoredPosition = part.Position * _currentSpread;
    }

    [Serializable]
    public class Part
    {
        public RectTransform RectTransform;
        public Vector2 Position;
    }
}
