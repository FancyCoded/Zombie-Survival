using UnityEngine;

[CreateAssetMenu(menuName = "Ammunition/properties", order = 51)]
public class AmmunitionProperties : ItemProperties
{
    [SerializeField] private AmmunitionType _type;

    public AmmunitionType Type => _type;
}