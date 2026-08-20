using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class StoreUI : MonoBehaviour
{
    [Header("References")]
    public Store store;
    public Player player;

    [Header("UI Elements")]
    public TMP_Dropdown modeDropdown;   // 0 = Buy, 1 = Sell
    public TMP_Dropdown itemDropdown;
    public TMP_InputField amountInput;
    public TMP_Text moneyText;

private readonly List<ItemType> buyItems = new()
{
    ItemType.TurnipSeed,
    ItemType.TomatoSeed,
    ItemType.Water,
    ItemType.Fertilizer
};

private readonly List<ItemType> sellItems = new()
{
    ItemType.TurnipCrop,
    ItemType.TomatoCrop
};

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnSeasonChanged += RefreshItemDropdown;
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnSeasonChanged -= RefreshItemDropdown;
    }

    private void Start()
    {
        modeDropdown.ClearOptions();
        modeDropdown.AddOptions(new List<string> { "Buy", "Sell" });
        modeDropdown.onValueChanged.AddListener(_ => RefreshItemDropdown());

        amountInput.text = "1";
        RefreshItemDropdown();
    }

    private void Update()
    {
        moneyText.text = $"Money: {player.Money}";
    }

    public void RefreshItemDropdown(Season _ = default)
    {
        itemDropdown.ClearOptions();
        bool isBuy = modeDropdown.value == 0;
        List<ItemType> items = isBuy ? buyItems : sellItems;

        List<string> options = new();
        foreach (var item in items)
        {
            StoreItemDatabase.GetData(item, TimeManager.Instance.CurrentSeason, out int buyPrice, out int sellPrice, out bool canBuy, out bool canSell);
            int price = isBuy ? buyPrice : sellPrice;
            options.Add($"{item} - {price}");
        }

        itemDropdown.AddOptions(options);
    }

    public void OnConfirm()
    {
        if (!int.TryParse(amountInput.text, out int amount)) amount = 1;
        amount = Mathf.Max(1, amount);

        bool isBuy = modeDropdown.value == 0;
        List<ItemType> items = isBuy ? buyItems : sellItems;
        ItemType selectedItem = items[itemDropdown.value];

        bool success = isBuy ? store.Buy(selectedItem, amount) : store.Sell(selectedItem, amount);

        if (!success) Debug.LogWarning($"Transaction failed: {selectedItem}");
    }
}
