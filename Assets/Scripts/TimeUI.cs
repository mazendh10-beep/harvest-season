using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text seasonText;

    private void Start()
    {
        TimeManager.Instance.OnDayChanged += UpdateDay;
        TimeManager.Instance.OnSeasonChanged += UpdateSeason;

        // Initial sync
        UpdateDay(TimeManager.Instance.Day);
        UpdateSeason(TimeManager.Instance.CurrentSeason);
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance == null) return;

        TimeManager.Instance.OnDayChanged -= UpdateDay;
        TimeManager.Instance.OnSeasonChanged -= UpdateSeason;
    }

    private void UpdateDay(int day)
    {
        dayText.text = $"Day {day}";
    }

    private void UpdateSeason(Season season)
    {
        seasonText.text = season.ToString();
    }
}
