using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public class PlayerIK : MonoBehaviour
{
    private const string IsHoldingWeapon = "HoldingWeapon";
    private const string HoldingType = "HoldType";
    private const string Reloaded = "Reloaded"; 

    [SerializeField] private Transform _targetLook;
    [SerializeField] private float _rightHandWeight;
    [SerializeField] private float _leftHandWeight;

    private Player _player;
    private Animator _animator;
    private Transform _leftHandTarget;
    private Transform _rightShoulder;
    private Transform _aimPivot;
    private Transform _rightHandTransform;
    private Transform _leftHandTransform;
    private Quaternion _leftHandRotation;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();    
        
        _rightShoulder = _animator.GetBoneTransform(HumanBodyBones.RightShoulder);

        _aimPivot = new GameObject("aim pivot").transform;
        _aimPivot.parent = transform;

        _rightHandTransform = new GameObject("right hand position").transform;
        _rightHandTransform.parent = _aimPivot;

        _leftHandTransform = new GameObject("left hand position").transform;
        _leftHandTransform.parent = _aimPivot;
    }

    private void Update()
    {
        if(_leftHandTarget != null)
        {
            _leftHandRotation = _leftHandTarget.rotation;
            _leftHandTransform.position = _leftHandTarget.position;
        }

        if (_animator.GetBool(IsHoldingWeapon) && _player.IsAiming)
        {
            _rightHandWeight = Mathf.MoveTowards(_rightHandWeight, 1, 0.1f);
            _leftHandWeight = Mathf.MoveTowards(_leftHandWeight, 1, 0.1f);
        }
        else
        {
            _rightHandWeight = Mathf.MoveTowards(_rightHandWeight, 0, 0.2f);
            _leftHandWeight = Mathf.MoveTowards(_leftHandWeight, 0, 0.2f);
        }
    }

    private void OnAnimatorIK()
    {
        _aimPivot.position = _rightShoulder.position;

        if (_animator.GetBool(IsHoldingWeapon) && _player.IsAiming)
        {
            _aimPivot.LookAt(_targetLook);
            SetAnimatorIKHands(1, 0.4f, 1, _leftHandWeight);
        }
        else if (_animator.GetBool(Reloaded) == false)
            SetAnimatorIKHands(0.3f, 0, 0.3f, 0);
        else if (_animator.GetInteger(HoldingType) == ((int)HoldType.TwoHandHolding))
            SetAnimatorIKHands(0.3f, 0, 0.3f, 1);
        else
            SetAnimatorIKHands(0.3f, 0, 0.3f, _leftHandWeight);
    }
    
    public void SetLeftHandTarget(Transform leftHandTarget) => _leftHandTarget = leftHandTarget;

       
    
    public void SetRightHandTransform(Vector3 position, Vector3 rotation)
    {
        _rightHandTransform.localPosition = position;
        _rightHandTransform.localRotation = Quaternion.Euler(rotation);
    }

    private void SetAnimatorIKHands(float weight, float bodyWeight, float headWeight, float leftHandWeight)
    {
        _animator.SetLookAtWeight(weight, bodyWeight, headWeight);
        _animator.SetLookAtPosition(_targetLook.position);

        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTransform.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandRotation);

        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightHandWeight);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _rightHandWeight);
        _animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTransform.position);
        _animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTransform.rotation);
    }
}