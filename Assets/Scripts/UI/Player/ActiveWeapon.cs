using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _ammunitionsInMagazine;
    [SerializeField] private TMP_Text _stack;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerInventory.WeaponChanged += OnWeaponChanged;
        _playerInventory.WeaponAmmunitionsCountChanged += OnWeaponAmmunitionsCountChanged;
    }
    
    private void OnDisable()
    {
        _playerInventory.WeaponChanged -= OnWeaponChanged;
        _playerInventory.WeaponAmmunitionsCountChanged -= OnWeaponAmmunitionsCountChanged;

        if (_playerInventory.CurrentWeapon && _playerInventory.CurrentWeapon is IRealoadable weapon)
            weapon.AmmunitionCountInMagazineChanged -= OnAmmunitionCountInMagazineChanged;
    }

    private void OnAmmunitionCountInMagazineChanged(int ammunitionsInMagazine) =>
        _ammunitionsInMagazine.text = ammunitionsInMagazine.ToString();

    private void OnWeaponAmmunitionsCountChanged(int ammunitionStackCount) =>
        _stack.text = ammunitionStackCount.ToString();

    private void OnWeaponChanged(Weapon weapon, int ammunitionsStack)
    {
        if (weapon)
        {
            if (weapon is IRealoadable realoadableWeapon)
                realoadableWeapon.AmmunitionCountInMagazineChanged += OnAmmunitionCountInMagazineChanged;

            if(weapon is FireArm fireArm)
            {
                _icon.sprite = fireArm.Properties.Icon;
                _ammunitionsInMagazine.text = fireArm.AmmunitionCountInMagazine.ToString();
                _stack.text = ammunitionsStack.ToString();
            }

            if(weapon is MeleeWeapon meleeWeapon)
            {
                _icon.sprite = meleeWeapon.Properties.Icon;
                _ammunitionsInMagazine.text = "0";
                _stack.text = "0";
            }
        }
        else
        {
            _icon.sprite = null;
            _ammunitionsInMagazine.text = "0";
            _stack.text = "0";
        }
    }
}