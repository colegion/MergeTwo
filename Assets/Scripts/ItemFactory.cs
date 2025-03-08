using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Pool;
using ScriptableObjects;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private int itemSpawnCount;

    private Grid _grid;
    private PoolController _poolController;
    private OrderController _orderController;

    private void InjectFields()
    {
        _grid = ServiceLocator.Get<Grid>();
        _poolController = ServiceLocator.Get<PoolController>();
        _orderController = ServiceLocator.Get<OrderController>();
    }

    public void PopulateInitialBoard()
    {
        InjectFields();
        
        var configManager = ServiceLocator.Get<ItemConfigManager>();

        var producer = configManager.GetItemConfig(ItemType.VegetableProducer, 4);
        var tempTile = _poolController.GetPooledObject(PoolableTypes.Producer);
        var tile = tempTile.GetGameObject().GetComponent<BaseTile>();
        var randomCell = _grid.GetAvailableRandomCell();
        tile.ConfigureSelf(producer, randomCell.X, randomCell.Y);
        
        for (int i = 0; i < itemSpawnCount; i++)
        {
            var randomConfig = configManager.GetRandomConfig();
            tempTile = _poolController.GetPooledObject(GetPoolableType(randomConfig.itemType));
            tile = tempTile.GetGameObject().GetComponent<BaseTile>();
            randomCell = _grid.GetAvailableRandomCell();
            tile.ConfigureSelf(randomConfig, randomCell.X, randomCell.Y);
        }
    }

    public void SpawnItemByConfig(BaseItemConfig itemConfig, Vector2Int? specificPosition = null, PoolableTypes type = PoolableTypes.BaseTile)
    {
        var tile = _poolController.GetPooledObject(type);
        var position = specificPosition ?? _grid.GetAvailableRandomCell().GetPosition();
        tile.GetGameObject().GetComponent<BaseTile>().ConfigureSelf(itemConfig, position.x, position.y);
        _orderController.OnNewItemCreated();
    }

    private PoolableTypes GetPoolableType(ItemType type)
    {
        switch (type)
        {
            case ItemType.Coin : case ItemType.Energy:
                return PoolableTypes.SpecialTile;
            case ItemType.VegetableProducer:
                return PoolableTypes.Producer;
            default:
                return PoolableTypes.BaseTile;
        }
    }
}
