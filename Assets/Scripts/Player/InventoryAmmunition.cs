using UnityEngine;

public partial class PlayerInventory : MonoBehaviour
{
    private void UseAmmunition(string name)
    {
        _items[name].ReduceCount(1);
        WeaponAmmunitionsCountChanged?.Invoke(_items[name].ItemCount);
        ItemUsed?.Invoke(name, _items[name].ItemCount);
        InventoryWeightChanged?.Invoke(GetInventoryWeight());
        
        if (_items[name].ItemCount == 0)
            _items.Remove(name);
    }

    private void AddAmmunition(Ammunition ammunition) 
    {
        if (ammunition is Ammunition ammo)
        {
            string name = ammo.Properties.Name;

            if (_items.ContainsKey(name))
            {
                _items[name].IncreaseCount(1);
                ItemCountChanged?.Invoke(ammo, _items[name].ItemCount);
            }
            else
            {
                _items.Add(name, new ItemCell(ammo, 1));
                ItemAdded?.Invoke(ammo, _items[ammo.Properties.Name].ItemCount, ItemBelong.PlayerInventoty);
            }
            
            if(_currentWeapon && _currentWeapon is IRealoadable weapon && weapon.AmmunitionName == name)
                WeaponAmmunitionsCountChanged?.Invoke(_items[name].ItemCount);
        }
    }

    private void ReduceAmmunitionCount(Ammunition ammunition)
    {
        if (ammunition is Ammunition ammo)
        {
            string name = ammo.Properties.Name;

            if (IsMoreOne(name))
            {
                _items[name].ReduceCount(1);
                ItemCountChanged?.Invoke(ammo, _items[name].ItemCount);
            }
            else
            {
                _items.Remove(name);
                ItemDropped?.Invoke(ammo);
            }

            if (_currentWeapon && _currentWeapon is IRealoadable weapon)
                if (weapon.AmmunitionName == name && _items.ContainsKey(name))
                    WeaponAmmunitionsCountChanged?.Invoke(_items[name].ItemCount);
                else
                    WeaponAmmunitionsCountChanged?.Invoke(0);
        }
    }
}
