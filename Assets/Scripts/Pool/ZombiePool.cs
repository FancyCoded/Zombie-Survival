using UnityEngine;
using System.Collections.Generic;

public abstract class ZombiePool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] protected Player Player;

    private List<Zombie> _pool = new List<Zombie>();

    public Transform Container => _container;

    protected void Initialize(Zombie[] templates, int romeroCount, int girlCount, int policemanCount, int maynardCount, int skeletonBossCount)
    {
        for (int i = 0; i < templates.Length; i++)
        {
            if (templates[i] is Romero)
                Create(templates[i], romeroCount);

            if (templates[i] is Girl)
                Create(templates[i], girlCount);

            if (templates[i] is Policeman)
                Create(templates[i], policemanCount);

            if (templates[i] is Maynard)
                Create(templates[i], maynardCount);

            if (templates[i] is SkeletonBoss)
                Create(templates[i], skeletonBossCount);
        }
    }

    protected Zombie GetZombie(Zombie zombie)
    {
        for (int i = 0; i < _pool.Count; i++)
            if (_pool[i].Properties.Type == zombie.Properties.Type && _pool[i].gameObject.activeSelf == false)
                return _pool[i];

        return Create(zombie, 1);
    }

    private Zombie Create(Zombie template, int count) 
    {
        Zombie instance = null; 

        for (int i = 0; i < count; i++)
        {
            instance = Instantiate(template, _container);
            instance.gameObject.SetActive(false);
            _pool.Add(instance);
        }

        return instance;
    }
}