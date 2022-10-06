using UnityEngine;
using UnityEngine.EventSystems;

public class PointerSounder : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _selectSound;
    
    public void OnPointerClick(PointerEventData eventData) => _audioSource.PlayOneShot(_clickSound);

    public void OnPointerEnter(PointerEventData eventData) => _audioSource.PlayOneShot(_selectSound);
}
