using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public int moneyEarned;

    private Dictionary<ItemType, int> cropsHarvested = new();
    private Dictionary<ItemType, int> seedsUsed = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ===== RECORDING =====

    public void RecordMoney(int amount)
    {
        moneyEarned += amount;
    }

    public void RecordSeedUsed(ItemType seedType)
    {
        if (!seedsUsed.ContainsKey(seedType))
            seedsUsed[seedType] = 0;

        seedsUsed[seedType]++;
    }

    public void RecordCropHarvested(ItemType cropType)
    {
        if (!cropsHarvested.ContainsKey(cropType))
            cropsHarvested[cropType] = 0;

        cropsHarvested[cropType]++;
    }

    // ===== READING =====

    public IReadOnlyDictionary<ItemType, int> CropsHarvested => cropsHarvested;
    public IReadOnlyDictionary<ItemType, int> SeedsUsed => seedsUsed;
}
