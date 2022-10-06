using UnityEngine;

public class SurfaceType : MonoBehaviour
{
    [SerializeField] private FootstepCollection _footstepCollection;

    public FootstepCollection FootstepCollection => _footstepCollection;
}
