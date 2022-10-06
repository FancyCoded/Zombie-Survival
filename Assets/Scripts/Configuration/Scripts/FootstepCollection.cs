using UnityEngine;

[CreateAssetMenu(menuName = "FootstepCollection", order = 51)]
public class FootstepCollection : ScriptableObject
{
    [SerializeField] private AudioClip[] _footstepSounds;

    public AudioClip[] FootstepSounds => _footstepSounds;
}
