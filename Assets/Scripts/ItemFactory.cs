using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private int itemSpawnCount;
    
    private Grid _grid;
    private PoolController _poolController;
    private OrderController _orderController;

    public void InjectDependencies()
    {
        _grid = ServiceLocator.Get<Grid>();
        _poolController = ServiceLocator.Get<PoolController>();
        _orderController = ServiceLocator.Get<OrderController>();
    }

    public void PopulateInitialBoard()
    {
        var configManager = ServiceLocator.Get<ItemConfigManager>();

        var producer = configManager.GetItemConfig(ItemType.VegetableProducer, 4);
        var tempTile = _poolController.GetPooledObject(PoolableTypes.Producer);
        var tile = tempTile.GetGameObject().GetComponent<BaseTile>();
        var randomCell = _grid.GetAvailableRandomCell();
        tile.ConfigureSelf(producer, randomCell.X, randomCell.Y);
        
        for (int i = 0; i < itemSpawnCount; i++)
        {
            var randomConfig = configManager.GetRandomConfig();
            tempTile = _poolController.GetPooledObject(Utilities.GetPoolableType(randomConfig.itemType));
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
    
}
