using UnityEngine;

public class Ammunition : Item
{
    [SerializeField] private AmmunitionProperties _properties;

    public AmmunitionProperties Properties => _properties;
}