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

    public void PopulateBoard()
    {
        _grid = ServiceLocator.Get<Grid>();
        _poolController = ServiceLocator.Get<PoolController>();
        
        var configManager = ServiceLocator.Get<ItemConfigManager>();

        var producer = configManager.GetItemConfig(ItemType.VegetableProducer, 4);
        var tempTile = _poolController.GetPooledObject(PoolableTypes.Producer);
        var tile = tempTile.GetGameObject().GetComponent<BaseTile>();
        var randomCell = _grid.GetAvailableRandomCell();
        tile.ConfigureSelf(producer, randomCell.X, randomCell.Y);
        
        for (int i = 0; i < itemSpawnCount; i++)
        {
            
        }
    }

    public void SpawnItemByConfig(BaseItemConfig itemConfig)
    {
        var tile = _poolController.GetPooledObject(PoolableTypes.BaseTile);
        var randomCell = _grid.GetAvailableRandomCell();
        tile.GetGameObject().GetComponent<BaseTile>().ConfigureSelf(itemConfig, randomCell.X, randomCell.Y);
    }
    
}
