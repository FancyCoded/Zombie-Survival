using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class FireArm : Weapon, IFireArm, IRealoadable
{
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private Transform _trailStartPoint;
    [SerializeField] protected FireArmProperties _properties;
    [SerializeField] private AudioClip _dryFireSound;

    private int _ammunitionCountInMagazine = 0;
    private float _lastShootTime;
    private ShootType _shootType;

    public FireArmProperties Properties => _properties;
    public ShootType ShootType => _shootType;
    public int MagazineSize => _properties.MagazineSize;
    public int AmmunitionCountInMagazine => _ammunitionCountInMagazine;
    public string AmmunitionName => _properties.AmmunitionProperties.Name;

    public UnityAction<int> AmmunitionCountInMagazineChanged { get; set; }

    private void Start() => _shootType = _properties.ShootType;

    public void Initialize(Camera camera) => MainCamera = camera;

    public void Reload()
    {
        AudioSource.PlayOneShot(_properties.ReloadSound);
        _ammunitionCountInMagazine = _properties.MagazineSize;
        AmmunitionCountInMagazineChanged?.Invoke(_ammunitionCountInMagazine);
    }

    public void PlayDryFire() => AudioSource.PlayOneShot(_dryFireSound);

    public void ToggleShootType()
    {
        if (_shootType == ShootType.Auto)
            _shootType = ShootType.Single;
        else
            _shootType = ShootType.Auto;
    }

    public void Fire()
    {
        if (_shootType == ShootType.Auto && _lastShootTime + _properties.ShootDelay < Time.time)
        {
            FireAction();
            _lastShootTime = Time.time;
        }

        if(_shootType == ShootType.Single)
            FireAction();
    }

    public void FireAction()
    {
        AudioSource.PlayOneShot(_properties.ShootSound);
        _muzzleFlash.Play();
        _ammunitionCountInMagazine--;
        AmmunitionCountInMagazineChanged?.Invoke(_ammunitionCountInMagazine);

        RaycastHit hit;
        Vector3 rayOrigin = MainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        if (Physics.Raycast(rayOrigin, MainCamera.transform.forward, out hit))
        {
            if (hit.collider.sharedMaterial != null)
                switch (hit.collider.sharedMaterial.name)
                {
                    case Wood:
                        SetDecal(hit, _properties.WoodHitEffect);
                        break;
                    case Stone:
                        SetDecal(hit, _properties.StoneHitEffect);
                        break;
                    case Sand:
                        SetDecal(hit, _properties.SandHitEffect);
                        break;
                    case Metal:
                        SetDecal(hit, _properties.MetalHitEffect);
                        break;
                    case WaterFilled:
                        SetDecal(hit, _properties.BarrelHitEffect);
                        break;
                    case Meat:
                        Instantiate(_properties.MeatHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                        break;
                    default:
                        break;
                }

        }
        
        TrailRenderer trail = Instantiate(_trail, _trailStartPoint.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, hit.point));
        
        if (hit.collider && hit.collider.TryGetComponent(out Zombie zombie))
            zombie.TakeDamage(_properties.Damage);
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 endPoint)
    {
        float time = 0;
        Vector3 startPoint = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = endPoint;
        Destroy(trail.gameObject, trail.time);
    }

    private void SetDecal(RaycastHit hit, GameObject decalTemplate)
    {
        GameObject decal = Instantiate(decalTemplate, hit.point, Quaternion.LookRotation(hit.normal));
        decal.transform.parent = hit.transform;
    }
}
