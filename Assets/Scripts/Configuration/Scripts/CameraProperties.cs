using UnityEngine;

[CreateAssetMenu(menuName = "Camera/Configuration", order = 51)]
public class CameraProperties : ScriptableObject
{
    [SerializeField] private Vector3 _normalPosition;
    [SerializeField] private Vector2 _rotationSpeed;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _movingSpeed;
    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _aimX;
    [SerializeField] private float _aimZ;

    public Vector3 NormalPosition => _normalPosition;
    public Vector3 RotationSpeed => _rotationSpeed;
    public float SmoothTime => _smoothTime;
    public float MovingSpeed => _movingSpeed;
    public float MinAngle => _minAngle;
    public float MaxAngle => _maxAngle;
    public float AimX => _aimX;
    public float AimZ => _aimZ;
}