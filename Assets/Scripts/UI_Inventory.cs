using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory playerInventory;

    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;

    public void SetInventory(Inventory inventory)
    {
        playerInventory = inventory;
        playerInventory.OnInventoryChanged += RefreshInventoryItems;
        RefreshInventoryItems();
    }

    public void RefreshInventoryItems()
    {
        if (playerInventory == null || itemSlotTemplate == null || itemSlotContainer == null) return;

        // Clear old slots
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 35f;

        foreach (Item item in playerInventory.GetItems())
        {
            // Skip zero amount consumables
            if (item.amount <= 0 && item.itemType != ItemType.Hoe) continue;

            RectTransform slotRect = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            slotRect.gameObject.SetActive(true);
            slotRect.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);

            // Icon
            Image iconImage = slotRect.Find("Icon")?.GetComponent<Image>();
            if (iconImage != null)
                iconImage.sprite = item.GetSprite();

            // Amount
            TextMeshProUGUI amountText = slotRect.Find("AmountText")?.GetComponent<TextMeshProUGUI>();
            if (amountText != null)
                amountText.text = item.itemType == ItemType.Hoe ? "" : item.amount.ToString();

            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    }
}
