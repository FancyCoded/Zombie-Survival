using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class PlayerWeaponHitMarker : MonoBehaviour
{
    [SerializeField] private float _collisionDelay;

    private bool _isCollidedEnemy;
    private IEnumerator _startCollisionDelay;
    private Zombie _zombie;

    public Zombie Zombie => _zombie;
    public bool IsCollidedEnemy => _isCollidedEnemy;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent(out Zombie zombie))
        {
            _isCollidedEnemy = true;
            _zombie = zombie;

            if (_startCollisionDelay != null)
                StopCoroutine(_startCollisionDelay);

            _startCollisionDelay = StartCollisionDealy();
            StartCoroutine(_startCollisionDelay);
        }

    }

    private IEnumerator StartCollisionDealy()
    {
        WaitForSeconds seconds = new WaitForSeconds(_collisionDelay);

        yield return seconds;
        _isCollidedEnemy = false;
        _zombie = null;
    }
}
