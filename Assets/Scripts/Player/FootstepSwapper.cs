using UnityEngine;

[RequireComponent(typeof(Movement))]
public class FootstepSwapper : MonoBehaviour
{
    [SerializeField] private FootstepCollection[] _footstepCollections;
    [SerializeField] private float _groundCheckDistance;

    private TerrainChecker _terrainChecker;
    private Movement _movement;
    private string _currentLayerName;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _terrainChecker = new TerrainChecker();
    }

    public void CheckLayers()
    {
        RaycastHit hit;
        float rayCastMotionDistance = _movement.CapsuleCollider.bounds.extents.y + _groundCheckDistance;

        if(Physics.Raycast(_movement.TransformCenter.position, Vector3.down, out hit, rayCastMotionDistance))
        {
            if(hit.transform.TryGetComponent(out Terrain terrain))
            {
                if (_currentLayerName != _terrainChecker.GetLayerName(transform.position, terrain))
                {
                    _currentLayerName = _terrainChecker.GetLayerName(transform.position, terrain);

                    for (int i = 0; i < _footstepCollections.Length; i++)
                        if (_currentLayerName == _footstepCollections[i].name)
                            _movement.SwapFootsteps(_footstepCollections[i]);
                }
            }

            if(hit.transform.TryGetComponent(out SurfaceType surfaceType))
            {
                _currentLayerName = surfaceType.FootstepCollection.name;
                _movement.SwapFootsteps(surfaceType.FootstepCollection);
            }
        }
    }
}
