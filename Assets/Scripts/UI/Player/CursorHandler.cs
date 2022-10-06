using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private Pause pause;
    [SerializeField] private GameEnd _gameEnd;

    private void Update()
    {
        if (inventoryView.IsInteractable() || pause.IsPauseActive() || _gameEnd.IsGameEnd)
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }
}
