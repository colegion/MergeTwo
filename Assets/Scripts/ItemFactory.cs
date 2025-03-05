using System.Collections;
using System.Collections.Generic;
using Helpers;
using Pool;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    [SerializeField] private int itemSpawnCount;

    public void PopulateBoard()
    {
        var grid = ServiceLocator.Get<Grid>();
        var configManager = ServiceLocator.Get<ItemConfigManager>();
        var poolController = ServiceLocator.Get<PoolController>();

        var producer = configManager.GetItemConfig(ItemType.VegetableProducer, 4);
        var tempTile = poolController.GetPooledObject(PoolableTypes.Producer);
        var tile = tempTile.GetGameObject().GetComponent<BaseTile>();
        var randomCell = grid.GetAvailableRandomCell();
        tile.ConfigureSelf(producer, randomCell.X, randomCell.Y);
        
        for (int i = 0; i < itemSpawnCount; i++)
        {
            
        }
    }
    
}
