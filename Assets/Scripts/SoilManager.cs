using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SoilManager : MonoBehaviour
{
    public static SoilManager Instance;

    [Header("Tilemaps")]
    public Tilemap soilTilemap;
    public Tilemap cropTilemap;

    [Header("Tomato Tiles")]
    public Tile tomatoPlanted;
    public Tile tomatoGrowing;
    public Tile tomatoGrown;

    [Header("Turnip Tiles")]
    public Tile turnipPlanted;
    public Tile turnipGrowing;
    public Tile turnipGrown;

    private class SoilTile
    {
        public ItemType seedType;
        public ItemType cropType;
        public float timer;
        public float growTime = 8f;
        public bool watered;
        public bool fertilized;
        public bool grown;
        public bool growing;
    }

    private readonly Dictionary<Vector3Int, SoilTile> tiles = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        foreach (var entry in tiles)
        {
            SoilTile tile = entry.Value;

            if (tile.grown || !tile.watered || !tile.fertilized)
                continue;

            tile.timer += Time.deltaTime;

            if (!tile.growing && tile.timer >= tile.growTime * 0.5f)
            {
                tile.growing = true;
                cropTilemap.SetTile(entry.Key, GetGrowingTile(tile.cropType));
            }

            if (tile.timer >= tile.growTime)
            {
                tile.grown = true;
                cropTilemap.SetTile(entry.Key, GetGrownTile(tile.cropType));
            }
        }
    }

    public bool CanPlant(Vector3Int pos)
    {
        if (!soilTilemap.HasTile(pos)) return false;
        if (tiles.ContainsKey(pos)) return false;
        return true;
    }

    public void PlantSeed(Vector3Int pos, ItemType seedType)
    {
        if (!CanPlant(pos)) return;

        ItemType cropType = SeedToCrop(seedType);

        SoilTile tile = new SoilTile
        {
            seedType = seedType,
            cropType = cropType
        };

        tiles[pos] = tile;
        cropTilemap.SetTile(pos, GetPlantedTile(cropType));
    }

    public void Water(Vector3Int pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
            tile.watered = true;
    }

    public void Fertilize(Vector3Int pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
            tile.fertilized = true;
    }

    public bool TryHarvest(Vector3Int pos, out ItemType cropType)
    {
        cropType = default;

        if (!tiles.TryGetValue(pos, out var tile)) return false;
        if (!tile.grown) return false;

        cropType = tile.cropType;
        tiles.Remove(pos);
        cropTilemap.SetTile(pos, null);
        GameSession.Instance.RecordCropHarvested(cropType);
        return true;
    }

    private ItemType SeedToCrop(ItemType seed)
    {
        return seed switch
        {
            ItemType.TomatoSeed => ItemType.TomatoCrop,
            ItemType.TurnipSeed => ItemType.TurnipCrop,
            _ => throw new System.Exception("Invalid seed type: " + seed)
        };
    }

    private Tile GetPlantedTile(ItemType crop)
    {
        return crop switch
        {
            ItemType.TomatoCrop => tomatoPlanted,
            ItemType.TurnipCrop => turnipPlanted,
            _ => null
        };
    }

    private Tile GetGrowingTile(ItemType crop)
    {
        return crop switch
        {
            ItemType.TomatoCrop => tomatoGrowing,
            ItemType.TurnipCrop => turnipGrowing,
            _ => null
        };
    }

    private Tile GetGrownTile(ItemType crop)
    {
        return crop switch
        {
            ItemType.TomatoCrop => tomatoGrown,
            ItemType.TurnipCrop => turnipGrown,
            _ => null
        };
    }
}
