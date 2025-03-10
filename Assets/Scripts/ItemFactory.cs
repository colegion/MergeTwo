using System;
using Helpers;
using Interfaces;
using Pool;
using ScriptableObjects;
using ScriptableObjects.Items;
using Tile;
using UnityEngine;
using Grid = GridSystem.Grid;

public class ItemFactory : MonoBehaviour, IInjectable
{
    [SerializeField] private ParticleHelper particleHelper;
    [SerializeField] private int itemSpawnCount;

    private Grid _grid;
    private PoolController _poolController;
    private OrderController _orderController;
    private ItemConfigManager _configManager;

    public void InjectDependencies()
    {
        _grid = ServiceLocator.Get<Grid>();
        _poolController = ServiceLocator.Get<PoolController>();
        _orderController = ServiceLocator.Get<OrderController>();
        _configManager = ServiceLocator.Get<ItemConfigManager>();
    }

    public void PopulateInitialBoard()
    {
        if (_configManager == null || _grid == null || _poolController == null) return;
        
        SpawnProducer(ItemType.VegetableProducer, level: 4);
        SpawnProducer(ItemType.DinnerProducer, level: 4);
        
        for (int i = 0; i < itemSpawnCount; i++)
        {
            var randomConfig = _configManager.GetRandomConfig();
            SpawnTile(randomConfig);
        }
    }

    public BaseTile SpawnItemByConfig(BaseItemConfig itemConfig, Vector2Int? specificPosition = null, PoolableTypes type = PoolableTypes.BaseTile)
    {
        if (itemConfig == null) return null;

        var position = specificPosition ?? _grid.GetAvailableRandomCell().GetPosition();
        return CreateTile(itemConfig, position, type);
    }

    private void SpawnProducer(ItemType itemType, int level)
    {
        var producerConfig = _configManager.GetItemConfig(itemType, level);
        if (producerConfig == null)
        {
            Debug.LogWarning($"Producer config not found for {itemType} at level {level}");
            return;
        }

        SpawnTile(producerConfig, PoolableTypes.Producer);
    }

    private void SpawnTile(BaseItemConfig config, PoolableTypes? typeOverride = null)
    {
        if (config == null) return;

        var poolableType = typeOverride ?? Utilities.GetPoolableType(config.itemType);
        var randomCell = _grid.GetAvailableRandomCell();
        CreateTile(config, randomCell.GetPosition(), poolableType);
    }

    private BaseTile CreateTile(BaseItemConfig config, Vector2Int position, PoolableTypes poolableType)
    {
        var pooledTile = _poolController.GetPooledObject(poolableType);
        var tile = pooledTile.GetGameObject().GetComponent<BaseTile>();

        if (tile == null)
        {
            Debug.LogError($"No BaseTile component found on pooled object of type {poolableType}");
            return null;
        }

        tile.ConfigureSelf(config, position.x, position.y);
        _orderController?.OnNewItemCreated();
        return tile;
    }
}
