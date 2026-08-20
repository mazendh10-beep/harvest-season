using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text cropsText;
    [SerializeField] private TMP_Text moneyText;

    public void Show(int day, int totalCrops, int totalMoney)
    {
        if (panel == null) return;

        panel.SetActive(true); // Activate panel first
        dayText.text = $"Days Played: {day}";
        cropsText.text = $"Crops Harvested: {totalCrops}";
        moneyText.text = $"Money Earned: ${totalMoney}";
    }
}
