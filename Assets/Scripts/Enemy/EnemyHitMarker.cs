using System.Collections;
using UnityEngine;

public class EnemyHitMarker : MonoBehaviour
{
    [SerializeField] private float _collisionDelay = 1.3f;

    private bool _isCollidedPlayer;
    private IEnumerator _startCollisionDelay;

    public bool IsCollidedPlayer => _isCollidedPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Player player))
        {
            _isCollidedPlayer = true;

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
        _isCollidedPlayer = false;
    }
}