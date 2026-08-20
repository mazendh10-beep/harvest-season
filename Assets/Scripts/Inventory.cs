using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> items;
    public event Action OnInventoryChanged;

    public Inventory()
    {
        items = new List<Item>();

  
        AddItem(ItemType.Hoe, 1);
    }

     public void AddItem(ItemType type, int amount)
    {
        Item existing = items.Find(i => i.itemType == type);
        if (existing != null)
            existing.amount += amount;
        else
            items.Add(new Item { itemType = type, amount = amount });

        OnInventoryChanged?.Invoke();
    }

  public bool RemoveItem(ItemType type, int amount)
    {
        Item item = items.Find(i => i.itemType == type);
        if (item == null || item.amount < amount) return false;

        item.amount -= amount;
        if (item.amount <= 0)
            items.Remove(item);

        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(ItemType type)
    {
        Item item = items.Find(i => i.itemType == type);
        return item != null && item.amount > 0;
    }

    public int GetItemAmount(ItemType type)
    {
        Item item = items.Find(i => i.itemType == type);
        return item != null ? item.amount : 0;
    }

    public List<Item> GetItems() => items;

      public bool UseItem(ItemType type)
    {
        Item item = items.Find(i => i.itemType == type && i.amount > 0);
        if (item == null) return false;

        if (!item.IsTool())
            item.amount--;

        OnInventoryChanged?.Invoke();
        return true;
    }

    public void GainItem(ItemType type, int amount)
    {
        AddItem(type, amount);
    }
}
