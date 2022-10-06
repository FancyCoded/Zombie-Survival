using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trader/Storage", order = 51)]
public class TraderStorage : ScriptableObject
{
    [SerializeField] private ItemProperties[] _items;

    private Dictionary<string, ItemProperties> _itemsDictionary;

    public ItemProperties[] Items => _items;

    private void Start()
    {
        for(int i = 0; i < _items.Length; i++)
            _itemsDictionary.Add(_items[i].Name, _items[i]);
    }
}