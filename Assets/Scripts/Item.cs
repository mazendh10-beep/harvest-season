using UnityEngine;

public class Item
{
 


    public ItemType itemType;
    public int amount;

   public Sprite GetSprite()
{
    return itemType switch
    {
        ItemType.TomatoSeed => ItemAssets.Instance.TomatoSeedSprite,
        ItemType.TomatoCrop => ItemAssets.Instance.TomatoCropSprite,
        ItemType.TurnipSeed => ItemAssets.Instance.TurnipSeedSprite,
        ItemType.TurnipCrop => ItemAssets.Instance.TurnipCropSprite,
        ItemType.Water => ItemAssets.Instance.WateringCanSprite,
        ItemType.Fertilizer => ItemAssets.Instance.FertilizerSprite,
        ItemType.Hoe => ItemAssets.Instance.HoeSprite,
        _ => null
    };
}


    public bool IsSeed()
    {
        return itemType == ItemType.TurnipSeed || itemType == ItemType.TomatoSeed;
    }

    public bool IsCrop()
    {
        return itemType == ItemType.TurnipCrop || itemType == ItemType.TomatoCrop;
    }

    public bool IsTool()
    {
        return itemType == ItemType.Hoe || itemType == ItemType.WateringCan;
    }
}
