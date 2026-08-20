using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private Player player;

    public bool Buy(ItemType type, int amount)
{
    StoreItemDatabase.GetData(
        type,
        TimeManager.Instance.CurrentSeason,
        out int buyPrice,
        out _,
        out bool canBuy,
        out _
    );

    if (!canBuy) return false;   // ← FAILS HERE

    int total = buyPrice * amount;
    return player.TryBuy(total, type, amount);
}

    public bool Sell(ItemType type, int amount)
    {
        if (!StoreItemDatabase.TryGet(type, out StoreItemData data))
            return false;

        if (!data.canSell)
            return false;

        int totalGain = data.sellPrice * amount;
        return player.TrySell(totalGain, type, amount);
    }
}
