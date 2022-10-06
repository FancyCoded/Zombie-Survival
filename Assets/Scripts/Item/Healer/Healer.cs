using UnityEngine;

public abstract class Healer : Item
{
    [SerializeField] private HealerType _healerType;
    [SerializeField] private HealerProperties _properties;
    
    public HealerType Type => _healerType;
    public HealerProperties Properties => _properties;

    public abstract void Heal(Player player);
}