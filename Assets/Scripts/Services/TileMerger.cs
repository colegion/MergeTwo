using Helpers;
using Pool;
using Tile;
using UnityEngine;

public class TileMerger
{
    private readonly ItemFactory _itemFactory;
    private readonly ParticleHelper _particleHelper;
    private readonly PoolController _poolController;

    public TileMerger(ItemFactory itemFactory, ParticleHelper particleHelper, PoolController poolController)
    {
        _itemFactory = itemFactory;
        _particleHelper = particleHelper;
        _poolController = poolController;
    }

    public BaseTile Merge(BaseTile selectedTile, BaseTile targetTile)
    {
        var nextStep = selectedTile.GetItemConfig().nextItem;
        var targetPos = targetTile.GetPosition();
        var objectType = Utilities.GetPoolableType(nextStep.itemType);

        _poolController.ReturnPooledObject(selectedTile);
        _poolController.ReturnPooledObject(targetTile);

        var spawned = _itemFactory.SpawnItemByConfig(nextStep, targetPos, objectType);

        _particleHelper.PlayParticleByType(ParticleType.TileMerge, new Vector2Int(spawned.X, spawned.Y));

        return spawned;
    }
}