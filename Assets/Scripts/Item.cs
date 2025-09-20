[System.Serializable]
public class Item
{
    public string itemName;
    public int price;
    public int quantity;

    public Item(string name, int price, int quantity)
    {
        this.itemName = name;
        this.price = price;
        this.quantity = quantity;
    }
}