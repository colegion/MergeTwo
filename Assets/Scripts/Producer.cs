using System.Collections;
using System.Collections.Generic;
using Helpers;
using Pool;
using ScriptableObjects;
using UnityEngine;

public class Producer : BaseTile
{
    private ProducerItemConfig _config;
    private List<ProducerCapacityConfig> _producerCapacity;
    private readonly Dictionary<BaseItemConfig, int> _remainingCounts = new Dictionary<BaseItemConfig, int>();
    private PoolController _poolController;
    private Grid _grid;

    private Coroutine _cooldownRoutine;
    private bool _cooldownActive;
    public override void ConfigureSelf(BaseItemConfig config, int x, int y)
    {
        base.ConfigureSelf(config, x, y);
        _poolController = ServiceLocator.Get<PoolController>();
        _grid = ServiceLocator.Get<Grid>();
        _config = (ProducerItemConfig)config;
        _producerCapacity = ((ProducerItemConfig)config).producerCapacity.capacityConfigs;
        PopulateCapacities();
    }
    
    public override void OnTap()
    {
        if (_cooldownActive)
        {
            
        }
        else
        {
            ProduceRandomItem();
        }
    }

    private void ProduceRandomItem()
    {
        bool cooldownActive = true;
        foreach (var config in _remainingCounts)
        {
            if (config.Value > 0) 
            {
                cooldownActive = false;
                break;  
            }
        }

        if (cooldownActive && _cooldownRoutine == null)
        {
            _cooldownRoutine = StartCoroutine(EnterCoolDown());
            return;
        } 

        var randomIndex = Random.Range(0, _producerCapacity.Count);
        var produceItem = _producerCapacity[randomIndex].itemToProduce;

        while (_remainingCounts[produceItem] == 0)
        {
            randomIndex = Random.Range(0, _producerCapacity.Count);
            produceItem = _producerCapacity[randomIndex].itemToProduce;
        }

        var tile = _poolController.GetPooledObject(PoolableTypes.BaseTile);
        var randomCell = _grid.GetAvailableRandomCell();
        tile.GetGameObject().GetComponent<BaseTile>().ConfigureSelf(produceItem, randomCell.X, randomCell.Y);
        _remainingCounts[produceItem]--;
    }

    private IEnumerator EnterCoolDown()
    {
        ToggleInteractable(false);
        _cooldownActive = true;
        yield return new WaitForSeconds(_config.durationForRecharge);
        ToggleInteractable(true);
        _cooldownActive = false;
        PopulateCapacities();
    }

    private void PopulateCapacities()
    {
        _remainingCounts.Clear();
        foreach (var temp in _producerCapacity)
        {
            _remainingCounts.TryAdd(temp.itemToProduce, temp.produceCount);
        }
    }
}
