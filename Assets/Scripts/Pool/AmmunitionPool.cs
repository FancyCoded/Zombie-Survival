using System.Collections.Generic;
using UnityEngine;

public abstract class AmmunitionPool : MonoBehaviour 
{
    [SerializeField] private Transform _container;

    protected List<Ammunition> _pool = new List<Ammunition>();
    public Transform Container => _container;

    protected void Initialize(Ammunition[] templates, int ammoCount)
    {
        for (int i = 0; i < templates.Length; i++)
        {
            if (templates[i] is Ammunition ammunition)
            {
                if (ammunition.Properties.Type == AmmunitionType.Caliber45)
                    Create(templates[i], ammoCount);

                if (ammunition.Properties.Type == AmmunitionType.Caliber556)
                    Create(templates[i], ammoCount);

                if (ammunition.Properties.Type == AmmunitionType.Caliber762)
                    Create(templates[i], ammoCount);
            }
        }
    }

    private Ammunition Create(Ammunition template, int count)
    {
        Ammunition instance = null;

        for (int i = 0; i < count; i++)
        {
            instance = Instantiate(template, _container);
            instance.gameObject.SetActive(false);
            _pool.Add(instance);
        }

        return instance;
    }

    protected Ammunition GetAmmunition(Ammunition ammunition)
    {
        for (int i = 0; i < _pool.Count; i++)
            if (_pool[i].Properties.Type == ammunition.Properties.Type)
                if (_pool[i].gameObject.activeSelf == false)
                    return _pool[i];

        return Create(ammunition, 1);
    }
}