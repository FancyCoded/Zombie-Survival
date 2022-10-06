using UnityEngine;
using System.Collections.Generic;

public abstract class WeaponPool : MonoBehaviour
{
    [SerializeField] private Transform _container;

    private List<Weapon> _pool = new List<Weapon>();
     
    public Transform Container => _container;

    protected void Initialize(Weapon[] templates, int initialWeaponCount)
    {
        for (int i = 0; i < templates.Length; i++)
        {
            if(templates[i] is FireArm fireArm)
            {
                if (fireArm.Properties.Type == FireArmType.M4A4)
                    Create(fireArm, initialWeaponCount);

                if (fireArm.Properties.Type == FireArmType.AKM)
                    Create(fireArm, initialWeaponCount);

                if (fireArm.Properties.Type == FireArmType.M4A1S)
                    Create(fireArm, initialWeaponCount);

                if (fireArm.Properties.Type == FireArmType.Colt)
                    Create(fireArm, initialWeaponCount);
            }

            if(templates[i] is MeleeWeapon meleeWeapon)
            {
                if (meleeWeapon.Properties.Type == MeleeWeaponType.Knife)
                    Create(meleeWeapon, initialWeaponCount);
            }
        }
    }

    protected Weapon GetWeapon(Weapon weapon)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if(weapon is FireArm fireArm && _pool[i] is FireArm poolFireArm)
                if(fireArm.Properties.Type == poolFireArm.Properties.Type && poolFireArm.gameObject.activeSelf == false)
                    return poolFireArm;
            if (weapon is MeleeWeapon meleeWeapon && _pool[i] is MeleeWeapon poolMeleeWeapon)
                if (meleeWeapon.Properties.Type == poolMeleeWeapon.Properties.Type && poolMeleeWeapon.gameObject.activeSelf == false)
                    return poolMeleeWeapon;

        }
            
        return Create(weapon, 1);
    }

    private Weapon Create(Weapon template, int count)
    {
        Weapon instance = null;

        for (int i = 0; i < count; i++)
        {
            instance = Instantiate(template, _container);
            instance.gameObject.SetActive(false);
            _pool.Add(instance);
        }

        return instance;
    }
}