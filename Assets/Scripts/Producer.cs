using System.Collections;
using System.Collections.Generic;
using Helpers;
using Pool;
using ScriptableObjects;
using UnityEngine;

public class Producer : BaseTile
{
    private List<ProducerCapacityConfig> producerCapacity;
    private readonly Dictionary<BaseItemConfig, int> remainingCounts = new Dictionary<BaseItemConfig, int>();
    private PoolController _poolController;
    private Grid _grid;
    public override void ConfigureSelf(BaseItemConfig config, int x, int y)
    {
        base.ConfigureSelf(config, x, y);
        _poolController = ServiceLocator.Get<PoolController>();
        _grid = ServiceLocator.Get<Grid>();
        producerCapacity = ((ProducerItemConfig)config).producerCapacity.capacityConfigs;
        foreach (var temp in producerCapacity)
        {
            remainingCounts.TryAdd(temp.itemToProduce, temp.produceCount);
        }
    }
    
    public override void OnTap()
    {
        ProduceRandomItem();
    }

    private void ProduceRandomItem()
    {
        bool cooldownActive = true;
        foreach (var config in remainingCounts)
        {
            if (config.Value > 0) 
            {
                cooldownActive = false;
                break;  
            }
        }

        if (cooldownActive) return; 

        var randomIndex = Random.Range(0, producerCapacity.Count);
        var produceItem = producerCapacity[randomIndex].itemToProduce;

        while (remainingCounts[produceItem] == 0)
        {
            randomIndex = Random.Range(0, producerCapacity.Count);
            produceItem = producerCapacity[randomIndex].itemToProduce;
        }

        var tile = _poolController.GetPooledObject(PoolableTypes.BaseTile);
        var randomCell = _grid.GetAvailableRandomCell();
        tile.GetGameObject().GetComponent<BaseTile>().ConfigureSelf(produceItem, randomCell.X, randomCell.Y);
        remainingCounts[produceItem]--;
    }

}
