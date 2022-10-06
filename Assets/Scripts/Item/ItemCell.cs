public class ItemCell
{
    private Item _item;
    private int _itemCount;

    public int ItemCount => _itemCount;
    public Item Item => _item;

    public ItemCell(Item item, int itemCount)
    {
        _item = item;
        _itemCount = itemCount;
    }

    public void IncreaseCount(int count) => _itemCount += count;

    public void ReduceCount(int count) => _itemCount -= count;
}