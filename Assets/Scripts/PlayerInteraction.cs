using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Tilemap soilTilemap;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        // Seed-specific planting
        if (Input.GetKeyDown(KeyCode.Alpha1)) TryPlant(ItemType.TomatoSeed);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryPlant(ItemType.TurnipSeed);

        if (Input.GetKeyDown(KeyCode.R)) TryWater();
        if (Input.GetKeyDown(KeyCode.F)) TryFertilize();
        if (Input.GetKeyDown(KeyCode.H)) TryHarvest();
    }

    private void TryPlant(ItemType seedType)
    {
        Vector3Int pos = soilTilemap.WorldToCell(transform.position);
        if (!player.inventory.UseItem(seedType)) return;
        if (!soilTilemap.HasTile(pos)) return;
        
        SoilManager.Instance.PlantSeed(pos, seedType);
        GameSession.Instance.RecordSeedUsed(seedType);
    }

    private void TryWater()
    {
        if (!player.inventory.UseItem(ItemType.Water)) return;

        Vector3Int pos = soilTilemap.WorldToCell(transform.position);
        SoilManager.Instance.Water(pos);
    }

    private void TryFertilize()
    {
        if (!player.inventory.UseItem(ItemType.Fertilizer)) return;

        Vector3Int pos = soilTilemap.WorldToCell(transform.position);
        SoilManager.Instance.Fertilize(pos);
    }

    private void TryHarvest()
    {
        Vector3Int pos = soilTilemap.WorldToCell(transform.position);

        if (SoilManager.Instance.TryHarvest(pos, out ItemType cropType))
        {
            player.inventory.GainItem(cropType, 1);
        }
    }
}
