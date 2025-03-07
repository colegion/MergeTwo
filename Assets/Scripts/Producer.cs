using System.Collections;
using System.Collections.Generic;
using Helpers;
using Pool;
using ScriptableObjects;
using UnityEngine;

public class Producer : BaseTile
{
    private ProducerItemConfig _config;
    private RewardHelper _rewardHelper;
    private ItemFactory _itemFactory;

    private Coroutine _cooldownRoutine;
    private bool _cooldownActive;
    public override void ConfigureSelf(BaseItemConfig config, int x, int y)
    {
        base.ConfigureSelf(config, x, y);
        _itemFactory = ServiceLocator.Get<ItemFactory>();
        _config = (ProducerItemConfig)config;
        _rewardHelper = new RewardHelper(_config.producerCapacity.capacityConfigs);
    }
    
    public override void OnTap()
    {
        if (_cooldownActive)
        { 
            
        }
        else
        {
            ProduceItem();
        }
    }

    private void ProduceItem()
    {
        if (_rewardHelper.IsEmpty() && _cooldownRoutine == null)
        {
            _cooldownRoutine = StartCoroutine(EnterCoolDown());
            return;
        }

        var itemToProduce = _rewardHelper.GetRandomItemToProduce();
        _itemFactory.SpawnItemByConfig(itemToProduce);
        _rewardHelper.DecreaseRemainingCount(itemToProduce);
    }

    private IEnumerator EnterCoolDown()
    {
        ToggleInteractable(false);
        _cooldownActive = true;
        yield return new WaitForSeconds(_config.durationForRecharge);
        ToggleInteractable(true);
        _cooldownActive = false;
        _rewardHelper.PopulateCapacities();
    }
}
