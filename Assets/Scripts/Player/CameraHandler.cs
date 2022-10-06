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

    private float _mouseX;
    private float _mouseY;
    private float _smoothX;
    private float _smoothY;
    private float _smoothXVelocity;
    private float _smoothYVelocity;
    private float _lookAngle; 
    private float _tiltAngle;
    private bool _isHandled = true;

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
        float targetX = _cameraStatus.NormalX;
        float targetY = _cameraStatus.NormalY;
        float targetZ = _cameraStatus.NormalZ;

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
        _mouseX = Input.GetAxis(MouseX);
        _mouseY = Input.GetAxis(MouseY);

        if (_cameraStatus.SmoothTime > 0)
        {
            _smoothX = Mathf.SmoothDamp(_smoothX, _mouseX, ref _smoothXVelocity, _cameraStatus.SmoothTime);
            _smoothY = Mathf.SmoothDamp(_smoothY, _mouseY, ref _smoothYVelocity, _cameraStatus.SmoothTime);
        }
        else
        {
            _smoothX = _mouseX;
            _smoothY = _mouseY;
        }

        _lookAngle += _smoothX * _cameraStatus.XRotationSpeed;
        Quaternion targetRotation = Quaternion.Euler(0, _lookAngle, 0);
        _holderTransform.rotation = targetRotation;

        _tiltAngle -= _smoothY * _cameraStatus.YRotationSpeed;
        _tiltAngle = Mathf.Clamp(_tiltAngle, _cameraStatus.MinAngle, _cameraStatus.MaxAngle);
        _pivot.localRotation = Quaternion.Euler(_tiltAngle, 0, 0);
    } 

    private void SetTargetLook()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        float time = Time.deltaTime * _lerpTimeMyltiplyer;

        if (Physics.Raycast(ray, out hit))
            _targetLook.position = Vector3.Lerp(_targetLook.position, hit.point, time);
    }
}
