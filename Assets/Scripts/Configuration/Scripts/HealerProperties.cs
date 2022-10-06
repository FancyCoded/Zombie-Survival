using UnityEngine;

[CreateAssetMenu(menuName = "Healer/properties", order = 51)]
public class HealerProperties : ItemProperties
{
    [SerializeField] private float _treatHealth;
    [SerializeField] private float _healDelay = 0;
    [SerializeField] private float _healStep = 0;

    public float HealthAmount => _treatHealth;
    public float HealDelay => _healDelay;
    public float HealStep => _healStep;
}