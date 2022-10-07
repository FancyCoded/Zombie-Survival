using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FootstepSwapper))] 
[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour
{
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    private const string IsStabbing = "IsStabbing";

    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private float _maxMoveableSlopeAngle;
    [SerializeField] private List<AudioClip> _footstepSounds = new List<AudioClip>();

    [Header("Movement")]
    [SerializeField] private float _movementMyltiplyer = 50;
    [SerializeField] private float _sprintMovementMyltiplyer = 2;
    [SerializeField] private float _rotationSpeed = 0.3f;

    [Header("Ground Check")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private float _sphereCastRadius;
    [SerializeField] private float _sphereCastMotionDistance;
    [SerializeField] private RaycastHit _groundCheckHit;
    [SerializeField] private Transform _transformCenter;

    [Header("Gravity")]
    [SerializeField] private float _gravity = 0;
    [SerializeField] private float _gravityFallTimer = 0;
    [SerializeField] private float _gravityFallCurrent = 0;
    [SerializeField] private float _gravityFallMin = -10;
    [SerializeField] private float _gravityFallMax = -50;
    [SerializeField] private float _gravityFallIncrementTime = 0.05f;
    [SerializeField] private float _gravityFallIncrementAmount = -5;

    private Player _player;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private AudioSource _audioSource;
    private FootstepSwapper _footstepSwapper;

    private float _verticalInput;
    private float _horizontalInput;
    private float _movement;
    private Vector3 _playerMove;
    private Vector3 _targetDirection;
    private Vector3 _directionOfRotation;

    public float PlayerMovement => _movement;
    public Transform TransformCenter => _transformCenter;
    public CapsuleCollider CapsuleCollider => _capsuleCollider;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _audioSource = GetComponent<AudioSource>();
        _footstepSwapper = GetComponent<FootstepSwapper>();
    }

    private void Start()
    {
        _sphereCastRadius = _capsuleCollider.radius;

        if (_footstepSwapper.IsLayerChanged(out FootstepCollection footstepCollection))
            SwapFootstepsCollection(footstepCollection);
    }

    private void FixedUpdate()
    {

        if (_animator.GetBool(IsStabbing) || _inventoryView.IsTrading())
        {
            _animator.SetFloat(Vertical, 0);
            _animator.SetFloat(Horizontal, 0);
            return;
        }

        Rotation();

        _isGrounded = IsGrounded();

        Move();

        _playerMove = GetGroundSlope();
        _playerMove.y = GetGravity() * _rigidbody.mass;

        _rigidbody.AddRelativeForce(_playerMove, ForceMode.Force);
    }

    private void SwapFootstepsCollection(FootstepCollection footstepCollection)
    {
        _footstepSounds.Clear();

        for (int i = 0; i < footstepCollection.FootstepSounds.Length; i++)
            _footstepSounds.Add(footstepCollection.FootstepSounds[i]);
    } 

    private void Move()
    {
        _verticalInput = Input.GetAxis(Vertical);
        _horizontalInput = Input.GetAxis(Horizontal);

        if (_isGrounded)
        {
            if (_player.IsAiming)
                AimingMove();
            else if (_player.IsSprinting)
                SetSpeed(true);
            else
                SetSpeed();
        }
    }

    private void AimingMove()
    {
        _animator.SetFloat(Vertical, _verticalInput);
        _animator.SetFloat(Horizontal, _horizontalInput);

        _playerMove = new Vector3(_horizontalInput * _movementMyltiplyer * _rigidbody.mass, 0, _verticalInput * _movementMyltiplyer * _rigidbody.mass);
    }

    private void Rotation()
    {
        _targetDirection = (_cameraHolder.forward * _verticalInput) + (_cameraHolder.right * _horizontalInput);
        _targetDirection.Normalize();

        _directionOfRotation = _cameraHolder.forward;

        if (_player.IsAiming == false)
            _directionOfRotation = _targetDirection;

        if (_directionOfRotation == Vector3.zero)
            _directionOfRotation = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(_directionOfRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed);
    }
    
    private void SetSpeed(bool IsSprinting = false)
    {
        if (IsSprinting)
        {
            _movement = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput)) * _sprintMovementMyltiplyer;
            _playerMove = new Vector3(0, 0, _movement * _movementMyltiplyer * _rigidbody.mass * _sprintMovementMyltiplyer);
        }
        else
        {
            _movement = Mathf.Clamp01(Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput));
            _playerMove = new Vector3(0, 0, _movement * _movementMyltiplyer * _rigidbody.mass);
        }

        _animator.SetFloat(Vertical, _movement);
    }

    // Animation event
    private void PlayFootstepSound()
    {
        if (_footstepSwapper.IsLayerChanged(out FootstepCollection footstepCollection))
            SwapFootstepsCollection(footstepCollection);

        if (_isGrounded == false)
            return;

        if (_footstepSounds.Count == 1)
            _audioSource.PlayOneShot(_footstepSounds[0]);
        else
        {
            int randomAudioIndex = Random.Range(1, _footstepSounds.Count);
            _audioSource.clip = _footstepSounds[randomAudioIndex];

            _audioSource.PlayOneShot(_audioSource.clip);

            _footstepSounds[randomAudioIndex] = _footstepSounds[0];
            _footstepSounds[0] = _audioSource.clip;
        }
    }

    private float GetGravity()
    {
        if (_isGrounded)
        {
            _gravityFallCurrent = _gravityFallMin;
            _gravity = 0;
        }
        else
        {
            _gravityFallTimer -= Time.fixedDeltaTime;

            if (_gravityFallTimer < 0)
            {
                if (_gravityFallCurrent > _gravityFallMax)
                    _gravityFallCurrent += _gravityFallIncrementAmount;

                _gravityFallTimer = _gravityFallIncrementTime;
            }
            
            _gravity = _gravityFallCurrent;
        }

        return _gravity;
    }

    private Vector3 GetGroundSlope()
    {
        Vector3 calculatedPlayerMovement = _playerMove;

        if (_isGrounded)
        {
            Vector3 localGroundNormal = _rigidbody.transform.InverseTransformDirection(_groundCheckHit.normal);
            float groundSlopeAngle = Vector3.Angle(localGroundNormal, _rigidbody.transform.up);

            if (groundSlopeAngle != 0)
            {
                Quaternion slopeAngleRotation = Quaternion.FromToRotation(_rigidbody.transform.up, localGroundNormal);
                calculatedPlayerMovement = slopeAngleRotation * calculatedPlayerMovement;

                // Formula for reduce speed by ground angle
                float relativeSlopeAngle = Vector3.Angle(calculatedPlayerMovement, _rigidbody.transform.up) - 90;
                
                if(relativeSlopeAngle < 0)
                    calculatedPlayerMovement += calculatedPlayerMovement * relativeSlopeAngle / _maxMoveableSlopeAngle;
            }
        }

        return calculatedPlayerMovement;
    }

    private bool IsGrounded()
    {
        _sphereCastMotionDistance = _capsuleCollider.bounds.extents.y - _sphereCastRadius + _groundCheckDistance;
        return Physics.SphereCast(_transformCenter.position, _sphereCastRadius, Vector3.down, out _groundCheckHit, _sphereCastMotionDistance);
    }
}