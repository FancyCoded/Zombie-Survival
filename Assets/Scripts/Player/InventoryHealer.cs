using UnityEngine;

public partial class PlayerInventory : MonoBehaviour
{
    private void OnHealerUsed(string name) => UseHealer(name);
    
    private void UseHealer(string name)
    {
        if (_items[name].Item is Healer healer)
        {
            _items[name].ReduceCount(1);
            healer.Heal(_player);
            ItemUsed?.Invoke(name, _items[name].ItemCount);
        }

        if (_items[name].ItemCount == 0)
            _items.Remove(name);

        _audioSource.PlayOneShot(_takeHealSound);
        InventoryWeightChanged?.Invoke(GetInventoryWeight());
    }

    private void AddHealer(Healer healer) 
    {
        string name = healer.Properties.Name;

        if (_items.ContainsKey(name))
        {
            _items[name].IncreaseCount(1);
            ItemCountChanged?.Invoke(healer, _items[name].ItemCount);
        }
        else
        {
            _items.Add(name, new ItemCell(healer, 1));
            ItemAdded?.Invoke(healer, _items[healer.Properties.Name].ItemCount, ItemBelong.PlayerInventoty);
        }
    }

    private void ReduceHealerCount(Healer healer)
    {
        string name = healer.Properties.Name;

        if (IsMoreOne(name))
        {
            _items[name].ReduceCount(1);
            ItemCountChanged?.Invoke(healer, _items[name].ItemCount);
        }
        else
        {
            _items.Remove(name);
            ItemDropped?.Invoke(healer);
        }
    }
}
