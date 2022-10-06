using UnityEngine;

public abstract class ItemProperties : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Item _itemTemplate;
    [SerializeField] private int _weight;
    [SerializeField] private int _cost;

    public string Name => _name;
    public Sprite Icon => _icon;
    public Item ItemTemplate => _itemTemplate;
    public int Weight => _weight;
    public int Cost => _cost;
}
