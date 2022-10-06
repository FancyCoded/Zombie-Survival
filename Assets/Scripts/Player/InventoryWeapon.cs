using UnityEngine;

public partial class PlayerInventory : MonoBehaviour
{
    public bool CanReload()
    {
        if (_currentWeapon is IRealoadable weapon)
        {
            string ammunitionName = weapon.AmmunitionName;

            if (_items.ContainsKey(ammunitionName))
                return _items[ammunitionName].ItemCount >= 1 && IsFullMagazine(weapon) == false;
        }
        
        return false;
    }

    public void ReloadWeapon()
    {
        if (_currentWeapon is IRealoadable weapon)
        {
            UseAmmunition(weapon.AmmunitionName);
            weapon.Reload();

            _animator.SetBool(Reloaded, false);
            _animator.SetTrigger(Reloading);
        }
    }

    public void DropWeapon(WeaponIndex index)
    {
        Weapon targetWeapon;

        switch (index)
        {
            case WeaponIndex.MainWeapon:
                targetWeapon = _mainWeapon;
                _mainWeapon = null;
                break;
            case WeaponIndex.SecondWeapon:
                targetWeapon = _secondWeapon;
                _secondWeapon = null;
                break;
            default:
                targetWeapon = _meleeWeapon;
                _meleeWeapon = null;
                break;
        }

        targetWeapon.ToggleIsPicked();
        targetWeapon.Rigidbody.isKinematic = false;
        targetWeapon.Collider.isTrigger = false;
        targetWeapon.Collider.enabled = true;
        targetWeapon.gameObject.SetActive(true);
        targetWeapon.transform.SetParent(_weaponSpawner.Container);
        targetWeapon.transform.rotation = Quaternion.Euler(0, 0, 60);

        if (_currentWeapon && _currentWeapon.Index == targetWeapon.Index)
        {
            _currentWeapon = null;
            UnarmPlayer();
        }

        _audioSource.PlayOneShot(_dropSound);

        WeaponDropped?.Invoke(index);
        UpdatePickUpField?.Invoke(_itemsInPickUpField);
    }

    public void PickUpWeapon(Weapon weapon)
    {
        switch (weapon.Index)
        {
            case WeaponIndex.MainWeapon:
                if (_mainWeapon != null)
                    DropWeapon(_mainWeapon.Index);

                InitializeWeapon(ref _mainWeapon, weapon);
                break;
            case WeaponIndex.SecondWeapon:
                if (_secondWeapon != null)
                    DropWeapon(_secondWeapon.Index);
                
                InitializeWeapon(ref _secondWeapon, weapon);
                break;
            case WeaponIndex.MeleeWeapon:
                if (_meleeWeapon != null)
                    DropWeapon(_meleeWeapon.Index);

                InitializeWeapon(ref _meleeWeapon, weapon);
                break;
        }

        if (_currentWeapon == null)
            SelectWeapon(weapon.Index);

        _audioSource.PlayOneShot(_pickUpSound);
        OnItemOutPickUpField(weapon);
    }

    public void ToggleWeapon(Weapon weapon)
    {
        _targetWeapon = weapon;
        _animator.SetTrigger(NeedToggleWeapon);
    }

    public void UnarmPlayer()
    {
        WeaponChanged?.Invoke(null, 0);

        if(_currentWeapon && _currentWeapon.IsPicked)
            _currentWeapon.gameObject.SetActive(false);

        _currentWeapon = null;
        _player.IsHoldingWeapon = false;
        _playerInput.SetWeapon(null);
        _playerIK.SetLeftHandTarget(null);
        _animator.SetBool(IsHoldingWeapon, _player.IsHoldingWeapon);
        _animator.SetInteger(HoldingType, 0);
    }

    private void SelectWeapon(WeaponIndex index)
    {
        if (_currentWeapon)
            _currentWeapon.gameObject.SetActive(false);

        if (index == WeaponIndex.MainWeapon)
            SetUpWeapon(_mainWeapon);
        if (index == WeaponIndex.SecondWeapon)
            SetUpWeapon(_secondWeapon);
        if (index == WeaponIndex.MeleeWeapon)
            SetUpWeapon(_meleeWeapon);

        if (_currentWeapon is IRealoadable weapon)
        {
            if (weapon is FireArm fireArm)
            {
                if (_items.ContainsKey(fireArm.AmmunitionName))
                    WeaponChanged?.Invoke(fireArm, _items[fireArm.Properties.AmmunitionProperties.Name].ItemCount);
                else
                    WeaponChanged?.Invoke(fireArm, 0);
            }
        }

        if (_currentWeapon is MeleeWeapon meleeWeapon)
            WeaponChanged?.Invoke(meleeWeapon, 0);

        _audioSource.PlayOneShot(_selectWeaponSound);
    }

    private void InitializeWeapon(ref Weapon playerWeapon, Weapon weapon)
    {
        playerWeapon = weapon;
        playerWeapon.transform.parent = _rightHand;
        playerWeapon.Rigidbody.isKinematic = true;
        playerWeapon.Collider.enabled = false;
        playerWeapon.Collider.isTrigger = true;
        playerWeapon.ToggleIsPicked();
        playerWeapon.gameObject.SetActive(false);

        if (playerWeapon is FireArm fireArm)
            WeaponAdded?.Invoke(fireArm.Properties, fireArm.Index, ItemBelong.PlayerInventoty);
        if (playerWeapon is MeleeWeapon meleeWeapon)
            WeaponAdded?.Invoke(meleeWeapon.Properties, meleeWeapon.Index, ItemBelong.PlayerInventoty);
    }

    private void SetUpWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
        _currentWeapon.gameObject.SetActive(true);
        _player.IsHoldingWeapon = true;
        _animator.SetBool(IsHoldingWeapon, _player.IsHoldingWeapon);

        Vector3 rightHandPosition;
        Vector3 rightHandRotation;
        Vector3 position = new Vector3();
        Quaternion rotation = new Quaternion();

        if(_currentWeapon.LeftHandTarget)
            _playerIK.SetLeftHandTarget(_currentWeapon.LeftHandTarget);

        if(_currentWeapon is FireArm fireArm)
        {
            _playerProperties.GetFireArmState(fireArm, out rightHandPosition, out rightHandRotation, out position, out rotation);
            _playerIK.SetRightHandTransform(rightHandPosition, rightHandRotation);
            fireArm.Initialize(_mainCamera);
            _animator.SetInteger(HoldingType, (int)fireArm.Properties.HoldType);
        }

        if (_currentWeapon is MeleeWeapon meleeWeapon)
        {
            _playerProperties.GetMeleeWeaponState(meleeWeapon, out position, out rotation);
            meleeWeapon.Initialize(_animator);
            _animator.SetInteger(HoldingType, (int)meleeWeapon.Properties.HoldType);
        }

        _currentWeapon.transform.localPosition = position;
        _currentWeapon.transform.localRotation = rotation;

        _playerInput.SetWeapon(_currentWeapon);
    }

    private void OnWeaponDropped(WeaponIndex index) => DropWeapon(index);

    private bool IsFullMagazine(IRealoadable weapon) => 
        weapon.AmmunitionCountInMagazine == weapon.MagazineSize;

    private void OnAnimationSelectWeapon()
    {
        if (_targetWeapon != null)
            SelectWeapon(_targetWeapon.Index);
    }

    private void OnAnimationWeaponReloaded() => _animator.SetBool(Reloaded, true);

    private void OnAnimationCheckHit()
    {
        if (_currentWeapon is MeleeWeapon meleeWeapon)
            if (meleeWeapon.HitMarker.IsCollidedEnemy)
                meleeWeapon.EnemyHitted();
    }
}