using System.Collections;
using System.Collections.Generic;
using Helpers;
using Pool;
using ScriptableObjects;
using UnityEngine;

public class Producer : BaseTile
{
    private ProducerStepConfig producerStepConfig;
    private PoolController _poolController;
    private Grid _grid;
    public override void ConfigureSelf(BaseItemConfig config, int x, int y)
    {
        base.ConfigureSelf(config, x, y);
        _poolController = ServiceLocator.Get<PoolController>();
        _grid = ServiceLocator.Get<Grid>();
        producerStepConfig = ((ProducerItemConfig)config).producerCapacity;
    }
    
    
    public override void OnTap()
    {
        Debug.LogWarning("produce");
        ProduceRandomItem();
    }

    private void ProduceRandomItem()
    {
        bool cooldownActive = true;
        foreach (var config in producerStepConfig.capacityConfigs)
        {
            cooldownActive = !(config.produceCount > 0);
        }

        //if (cooldownActive) return;
        
        var randomIndex = Random.Range(0, producerStepConfig.capacityConfigs.Count);
        var produceItem = producerStepConfig.capacityConfigs[randomIndex].itemToProduce;
        var tile = _poolController.GetPooledObject(PoolableTypes.BaseTile);
        var randomCell = _grid.GetAvailableRandomCell();
        tile.GetGameObject().GetComponent<BaseTile>().ConfigureSelf(produceItem, randomCell.X, randomCell.Y);
        Debug.LogWarning("produced item" , tile.GetGameObject());
        producerStepConfig.capacityConfigs[randomIndex].produceCount--;
    }
}
