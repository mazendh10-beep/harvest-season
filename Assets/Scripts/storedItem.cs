using System;
using System.Collections.Generic;

public static class StoreItemDatabase
{
    private static Dictionary<ItemType, StoreItemData> data =
        new Dictionary<ItemType, StoreItemData>()
    {
        { ItemType.TurnipSeed,  new StoreItemData(5,  2, true,  true) },
        { ItemType.TurnipCrop,  new StoreItemData(0, 10, false, true) },

        { ItemType.TomatoSeed, new StoreItemData(8,  3, true,  true) },
        { ItemType.TomatoCrop, new StoreItemData(0, 15, false, true) },

        { ItemType.Fertilizer, new StoreItemData(20, 5, true,  true) },
    };

    public static bool TryGet(
        ItemType type,
        out StoreItemData itemData
    )
    {
        return data.TryGetValue(type, out itemData);
    }
public static void GetData(
        ItemType type,
        Season season,
        out int buyPrice,
        out int sellPrice,
        out bool canBuy,
        out bool canSell
    )
    {
        buyPrice = 0;
        sellPrice = 0;
        canBuy = false;
        canSell = false;

        switch (type)
        {
            // ================= SEEDS =================
            case ItemType.TomatoSeed:
                buyPrice = season == Season.Spring ? 5 : 8;
                canBuy = true;
                break;

            case ItemType.TurnipSeed:
                buyPrice = season == Season.Spring ? 4 : 6;
                canBuy = true;
                break;

            // ================= CROPS =================
            case ItemType.TomatoCrop:
                sellPrice = 15;
                canSell = true;
                break;

            case ItemType.TurnipCrop:
                sellPrice = 10;
                canSell = true;
                break;

            // ================= SUPPLIES =================
            case ItemType.Water:
                buyPrice = 3;
                canBuy = true;
                break;

            case ItemType.Fertilizer:
                buyPrice = 6;   // ← THIS IS NOW GUARANTEED
                canBuy = true;
                break;

            // ================= TOOLS =================
            case ItemType.Hoe:
                canBuy = false;
                canSell = false;
                break;
        }
    }

}

public struct StoreItemData
{
    public int buyPrice;
    public int sellPrice;
    public bool canBuy;
    public bool canSell;

    public StoreItemData(int buy, int sell, bool canBuy, bool canSell)
    {
        buyPrice = buy;
        sellPrice = sell;
        this.canBuy = canBuy;
        this.canSell = canSell;
    }


}
