using UnityEngine;
using System.Collections.Generic;

public abstract class HealerPool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    
    private List<Healer> _pool = new List<Healer>();

    public Transform Container => _container;

    protected void Initialize(Healer[] templates, int healerCount)
    {
        for (int i = 0; i < templates.Length; i++)
        {
            if (templates[i] is AidKit)
                Create(templates[i], healerCount);
            if (templates[i] is Painkiller)
                Create(templates[i], healerCount);
            if (templates[i] is EnergyDrink)
                Create(templates[i], healerCount);
            if (templates[i] is Bandage)
                Create(templates[i], healerCount);
        }
    }

    protected Healer GetHealer(Healer healer)
    {
        for (int i = 0; i < _pool.Count; i++)
            if (_pool[i].Type == healer.Type && _pool[i].gameObject.activeSelf == false)
                return _pool[i];

        return Create(healer, 1);
    }

    private Healer Create(Healer template, int count)
    {
        Healer instance = null;

        for (int i = 0; i < count; i++)
        {
            instance = Instantiate(template, _container);
            instance.gameObject.SetActive(false);
            _pool.Add(instance);
        }

        return instance;
    }
}