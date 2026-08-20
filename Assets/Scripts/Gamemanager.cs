using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private Player player;

    private void OnEnable()
    {
        TimeManager.Instance.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        int totalCrops = player.inventory.GetItemAmount(ItemType.TomatoCrop) +
                         player.inventory.GetItemAmount(ItemType.TurnipCrop);
        int totalMoney = player.Money;

        gameOverUI.Show(TimeManager.Instance.Day, totalCrops, totalMoney);
        Time.timeScale = 0f; // Freeze the game AFTER UI shows
    }
}
