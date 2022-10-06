using UnityEngine;

[CreateAssetMenu(menuName = "Camera/Configuration", order = 51)]
public class CameraProperties : ScriptableObject
{
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _movingSpeed;
    [SerializeField] private float _xRotaionSpeed;
    [SerializeField] private float _yRotationSpeed;
    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _normalX;
    [SerializeField] private float _normalY;
    [SerializeField] private float _normalZ;
    [SerializeField] private float _aimX;
    [SerializeField] private float _aimZ;

    public float SmoothTime => _smoothTime;
    public float MovingSpeed => _movingSpeed;
    public float XRotationSpeed => _xRotaionSpeed;
    public float YRotationSpeed => _yRotationSpeed;
    public float MinAngle => _minAngle;
    public float MaxAngle => _maxAngle;
    public float NormalX => _normalX;
    public float NormalY => _normalY;
    public float NormalZ => _normalZ;
    public float AimX => _aimX;
    public float AimZ => _aimZ;
}