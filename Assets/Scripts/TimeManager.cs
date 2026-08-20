using UnityEngine;
using System;

public enum Season
{
    Spring,
    Summer
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public event Action<int> OnDayChanged;
    public event Action<Season> OnSeasonChanged;
    public event Action OnGameOver;


    public int Day { get; private set; } = 1;
    public Season CurrentSeason { get; private set; } = Season.Spring;

    [SerializeField] private float dayLength = 10f;
    public float timer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= dayLength)
        {
            timer = 0f;
            AdvanceDay();
        }
    }

    private void AdvanceDay()
    {
        Day++;
        Debug.Log($"Day advanced to {Day}");
        OnDayChanged?.Invoke(Day);

        if (Day == 7 && CurrentSeason == Season.Spring)
        {
            CurrentSeason = Season.Summer;
            Debug.Log("Season changed to Summer");
            OnSeasonChanged?.Invoke(CurrentSeason);
        }

        if (Day == 14 && CurrentSeason == Season.Summer)
        {
            Debug.Log("End of Summer — Game Over");
            OnGameOver?.Invoke();
            Time.timeScale = 0f;
        }
    }
}

