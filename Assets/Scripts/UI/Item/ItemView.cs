using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _itemCount;
    [SerializeField] private Drag _drag;

    public Drag Drag => _drag;
    public Image Image => _image;
    public TMP_Text Title => _title;
    public TMP_Text ItemCount => _itemCount;

    private void OnEnable() => _drag.ItemCountChanged += OnItemCountChanged;

    private void OnDisable() => _drag.ItemCountChanged -= OnItemCountChanged;

    private void OnItemCountChanged(int count) => _itemCount.text = count.ToString();
}
