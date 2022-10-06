using UnityEngine;

[CreateAssetMenu(menuName = "Player/properties", order = 51)]
public class PlayerProperties : ScriptableObject
{
    [Header("M4A4")]
    [SerializeField] private Vector3 _rightHandPositionM4A4;
    [SerializeField] private Vector3 _rightHandRotationM4A4;
    [SerializeField] private Vector3 _positionM4A4;
    [SerializeField] private Vector3 _rotationM4A4;

    [Header("M4A1-S")]
    [SerializeField] private Vector3 _rightHandPositionM4A1S;
    [SerializeField] private Vector3 _rightHandRotationM4A1S;
    [SerializeField] private Vector3 _positionM4A1S;
    [SerializeField] private Vector3 _rotationM4A1S;

    [Header("AKM")]
    [SerializeField] private Vector3 _rightHandPositionAKM;
    [SerializeField] private Vector3 _rightHandRotationAKM;
    [SerializeField] private Vector3 _positionAKM;
    [SerializeField] private Vector3 _rotationAKM;

    [Header("Colt")]
    [SerializeField] private Vector3 _rightHandPositionColt;
    [SerializeField] private Vector3 _rightHandRotationColt;
    [SerializeField] private Vector3 _positionColt;
    [SerializeField] private Vector3 _rotationColt;

    [Header("Knife")]
    [SerializeField] private Vector3 _positionKnife;
    [SerializeField] private Vector3 _rotationKnife;

    public void GetFireArmState(FireArm weapon, out Vector3 rightHandPosition, out Vector3 rightHandRotation, out Vector3 position, out Quaternion rotation)
    {
        rightHandPosition = new Vector3();
        rightHandRotation = new Vector3();
        position = new Vector3();
        rotation = new Quaternion();

            switch (weapon.Properties.Type)
            {
                case FireArmType.AKM:
                    rightHandPosition = _rightHandPositionAKM;
                    rightHandRotation = _rightHandRotationAKM;
                    position = _positionAKM;
                    rotation = Quaternion.Euler(_rotationAKM);
                    break;
                case FireArmType.M4A4:
                    rightHandPosition = _rightHandPositionM4A4;
                    rightHandRotation = _rightHandRotationM4A4;
                    position = _positionM4A4;
                    rotation = Quaternion.Euler(_rotationM4A4);
                    break;
                case FireArmType.M4A1S:
                    rightHandPosition = _rightHandPositionM4A1S;
                    rightHandRotation = _rightHandRotationM4A1S;
                    position = _positionM4A1S;
                    rotation = Quaternion.Euler(_rotationM4A1S);
                    break;
                case FireArmType.Colt:
                    rightHandPosition = _rightHandPositionColt;
                    rightHandRotation = _rightHandRotationColt;
                    position = _positionColt;
                    rotation = Quaternion.Euler(_rotationColt);
                    break;
            }
    }

    public void GetMeleeWeaponState(MeleeWeapon weapon, out Vector3 position, out Quaternion rotation)
    {
        position = new Vector3();
        rotation = new Quaternion();

        if (weapon is MeleeWeapon meleeWeapon)
        {
            switch (meleeWeapon.Properties.Type)
            {
                case MeleeWeaponType.Knife:
                    position = _positionKnife;
                    rotation = Quaternion.Euler(_rotationKnife);
                    break;
            }
        }
    }
}
