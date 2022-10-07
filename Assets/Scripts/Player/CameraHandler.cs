using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _pivot;
    [SerializeField] private Transform _holderTransform;
    [SerializeField] private Transform _targetLook;
    [SerializeField] private Player _player;
    [SerializeField] private Pause _pause;
    [SerializeField] private CameraProperties _cameraStatus;
    [SerializeField] private float _lerpTimeMyltiplyer = 100;
    [SerializeField] private bool _isLeftPivot;

    private float _lookAngle; 
    private float _tiltAngle;
    private bool _isHandled = true;

    private Vector2 _smooth;
    private Vector2 _mouse;
    private Vector2 _smoothVelocity; 

    public bool IsHandled => _isHandled;

    private void Update()
    {
        if (_isHandled && _pause.IsActive == false)
        {
            HandlePosition();
            HandleRotation(); 
        }

        if(_player)
            _holderTransform.position = Vector3.Lerp(_holderTransform.position, _player.transform.position, 1);
    }

    public void ToggleAcitvity() => _isHandled = !_isHandled;

    public void SetActivity(bool isActive = true) => _isHandled = isActive;

    private void HandlePosition()
    {
        float targetX = _cameraStatus.NormalPosition.x;
        float targetY = _cameraStatus.NormalPosition.y;
        float targetZ = _cameraStatus.NormalPosition.z;

        if (_player.IsAiming)
        {
            targetX = _cameraStatus.AimX;
            targetZ = _cameraStatus.AimZ;
        }

        if (_isLeftPivot)
            targetX = -targetX;

        Vector3 newCameraPostion = _cameraTransform.localPosition;
        newCameraPostion.z = targetZ;

        _pivot.localPosition = new Vector3(targetX, targetY);
        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, newCameraPostion, _cameraStatus.MovingSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        _mouse = new Vector2(Input.GetAxis(MouseX), Input.GetAxis(MouseY));

        if (_cameraStatus.SmoothTime > 0)
        {
            _smooth.x = Mathf.SmoothDamp(_smooth.x, _mouse.x, ref _smoothVelocity.x, _cameraStatus.SmoothTime);
            _smooth.y = Mathf.SmoothDamp(_smooth.y, _mouse.y, ref _smoothVelocity.y, _cameraStatus.SmoothTime);
        }
        else
        {
            _smooth.x = _mouse.x;
            _smooth.y = _mouse.y;
        }

        _lookAngle += _smooth.x * _cameraStatus.RotationSpeed.x;
        Quaternion targetRotation = Quaternion.Euler(0, _lookAngle, 0);
        _holderTransform.rotation = targetRotation;

        _tiltAngle -= _smooth.y * _cameraStatus.RotationSpeed.y;
        _tiltAngle = Mathf.Clamp(_tiltAngle, _cameraStatus.MinAngle, _cameraStatus.MaxAngle);
        _pivot.localRotation = Quaternion.Euler(_tiltAngle, 0, 0);
    } 
}
