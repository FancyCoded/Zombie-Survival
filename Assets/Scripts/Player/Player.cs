using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(Wallet))]
public class Player : MonoBehaviour
{
    public bool IsSprinting;
    public bool IsHoldingWeapon;
    public bool IsAiming;

    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";

    [SerializeField] CameraHandler _cameraHandler;

    private Health _health;
    private PlayerInput _playerInput;
    private Movement _movement;
    private Animator _animator;
    private Wallet _wallet;
    private PlayerInventory _invenotory;

    public Health Health => _health;
    public Wallet Wallet => _wallet;
    public PlayerInventory Inventory => _invenotory;
    public float HealthAmount => _health.Amount;
    public float MaxHealth => _health.Amount;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _wallet = GetComponent<Wallet>();
        _invenotory = GetComponent<PlayerInventory>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
        _animator = GetComponent<Animator>();
    }

    public void GameEnd()
    {
        _playerInput.enabled = false;
        _movement.enabled = false;
        _cameraHandler.enabled = false;
        _animator.SetFloat(Vertical, 0);
        _animator.SetFloat(Horizontal, 0);
    }

    public void TakeDamage(float damage) => _health.TakeDamage(damage);

    public void TakeHeal(float healthAmount, float healStep = 0, float healDelay = 0) =>
        _health.TakeHeal(healthAmount, healStep, healDelay);

    public void Buy(Item item) => _invenotory.AddBoughtItem(item);
}