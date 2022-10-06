using UnityEngine;

public abstract class Item : MonoBehaviour
{ 
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _rigidbody;

    public Collider Collider => _collider;
    public Rigidbody Rigidbody => _rigidbody;
}