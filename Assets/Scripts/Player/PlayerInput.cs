using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInventory))]
public class PlayerInput : MonoBehaviour
{
    private const string Aiming = "Aiming";
    private const string PutBackWeapon = "PutBackWeapon";
    private const string Reloaded = "Reloaded";
    private const string HoldingWeapon = "HoldingWeapon";
    
    [SerializeField] private Transform _targetLook;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private Pause _resume;
    [SerializeField] private bool _isDebugAiming;

    private Player _player;
    private PlayerInventory _playerInventory;
    private Animator _animator;
    private Weapon _currentWeapon;

    private bool _isAiming;

    public UnityAction TraderFound;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        Resume();

        if(_resume.IsActive == false)
        {
            ToggleInventoryActivity();

            if (_inventoryView.IsInteractable())
                return;

            Hit();
            Aim();
            Fire();
            Sprint();
            ToggleWeapon();
            TryPickUpItem();
            ChangeShootType();
            ReloadWeapon();
            StopAtTrader();

            if(_player.IsAiming == false)
                DropWeapon();
        }
    }

    public void SetWeapon(Weapon weapon) => _currentWeapon = weapon;
    
    private void Resume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if(_resume.IsSettingActive() == false)
                _resume.ToggleActivityPauseMenu();
    }

    private void ToggleInventoryActivity()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            _inventoryView.ToggleActivity();
    }
    
    private void TryPickUpItem()
    {
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

        if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            if (hit.collider.TryGetComponent(out Weapon weapon))
                if(_animator.GetBool(Reloaded) && weapon.IsPicked == false) 
                    _playerInventory.PickUpWeapon(weapon);

            if(hit.collider.TryGetComponent(out Item item))
                _playerInventory.TryAddItem(item, hit);
        }
    }

    private void DropWeapon()
    {
        if(Input.GetKeyDown(KeyCode.G) && _currentWeapon && _animator.GetBool(Reloaded))
            _playerInventory.DropWeapon(_currentWeapon.Index);
    }

    private void ToggleWeapon()
    {
        if (_player.IsAiming == false && _animator.GetBool(Reloaded))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                if (_playerInventory.MainWeapon != null && _playerInventory.CurrentWeapon != _playerInventory.MainWeapon)
                    _playerInventory.ToggleWeapon(_playerInventory.MainWeapon);
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
                if (_playerInventory.SecondWeapon != null && _playerInventory.CurrentWeapon != _playerInventory.SecondWeapon)
                    _playerInventory.ToggleWeapon(_playerInventory.SecondWeapon);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                if (_playerInventory.MeleeWeapon != null && _playerInventory.CurrentWeapon != _playerInventory.MeleeWeapon)
                    _playerInventory.ToggleWeapon(_playerInventory.MeleeWeapon);

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if(_player.IsHoldingWeapon == true)
                {
                    _animator.SetTrigger(PutBackWeapon);
                    _playerInventory.UnarmPlayer();
                }
            }
        }
    }

    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _player.IsSprinting = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            _player.IsSprinting = false;
    }

    private void Aim()
    {
        if (_animator.GetBool(HoldingWeapon) && (_currentWeapon is  MeleeWeapon) == false)
        {
            if (_isDebugAiming)
                _isAiming = true;
            else
            {
                if (Input.GetMouseButton(1) && _animator.GetBool(Reloaded))
                    _isAiming = true;
                else
                    _isAiming = false;
            }

            _player.IsAiming = _isAiming;
            _animator.SetBool(Aiming, _player.IsAiming);
        }
    }

    private void Fire()
    {
        if(_currentWeapon && _currentWeapon is FireArm fireArm && _isAiming)
        {
            if (fireArm.AmmunitionCountInMagazine > 0)
            {
                switch (fireArm.ShootType)
                {
                    case ShootType.Single:
                        if (Input.GetMouseButtonDown(0))
                            fireArm.Fire();
                        break;
                    case ShootType.Auto:
                        if (Input.GetMouseButton(0))
                            fireArm.Fire();
                        break;
                }
            }
            else if(Input.GetMouseButtonDown(0))
                fireArm.PlayDryFire();
        }
    }

    private void Hit()
    {
        if (_currentWeapon is MeleeWeapon weapon && weapon.Properties.Type == MeleeWeaponType.Knife)
            if (Input.GetMouseButtonDown(0))
                weapon.Hit();

    }

    private void ReloadWeapon()
    {
        if (Input.GetKey(KeyCode.R) && _currentWeapon && _player.IsAiming == false)
            if(_playerInventory.CanReload())
                _playerInventory.ReloadWeapon();
    }

    private void ChangeShootType()
    {
        if(_currentWeapon is FireArm fireArm)
            if (_currentWeapon && Input.GetKeyDown(KeyCode.T))
                fireArm.ToggleShootType();
    }

    private void StopAtTrader()
    {
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

        if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            if(hit.collider.TryGetComponent(out Trader trader) && _inventoryView.IsInteractable() == false)
            {
                if (trader.IsOpen == false)
                    return;

                trader.Open(_player);
                _inventoryView.TradingViewUpdate();
                TraderFound?.Invoke();
            }
        }
    }
}