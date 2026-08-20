using UnityEngine;
public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Seeds")]
    public Sprite TurnipSeedSprite;
    public Sprite TomatoSeedSprite;

    [Header("Crops")]
    public Sprite TurnipCropSprite;
    public Sprite TomatoCropSprite;

    [Header("Tools")]
    public Sprite HoeSprite;
    public Sprite WateringCanSprite;

    [Header("Misc")]
    public Sprite FertilizerSprite;
    public Sprite WaterSprite;
}
