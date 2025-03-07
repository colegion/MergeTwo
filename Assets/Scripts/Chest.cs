using System.Collections;
using System.Collections.Generic;
using Helpers;
using ScriptableObjects;
using UnityEngine;

public class Chest : BaseTile
{
    private ChestItemConfig _config;
    private RewardHelper _rewardHelper;

    private ItemFactory _itemFactory;
    
    private Coroutine _unlockRoutine;
    private bool _hasUnlocked;
    public override void ConfigureSelf(BaseItemConfig config, int x, int y)
    {
        base.ConfigureSelf(config, x, y);
        _config = (ChestItemConfig)config;
        if(_itemFactory == null) _itemFactory = ServiceLocator.Get<ItemFactory>();
        _rewardHelper = new RewardHelper(_config.chestRewards.capacityConfigs);
    }

    public override void OnTap()
    {
        if (!_hasUnlocked && _unlockRoutine == null)
        {
            _unlockRoutine = StartCoroutine(UnlockChest());
        }
        else
        {
            ProduceItem();
        }
    }

    private void ProduceItem()
    {
        var itemToProduce = _rewardHelper.GetRandomItemToProduce();
        _itemFactory.SpawnItemByConfig(itemToProduce);
        _rewardHelper.DecreaseRemainingCount(itemToProduce);

        if (_rewardHelper.IsEmpty())
        {
            GameController.Instance.ReturnPoolableToPool(this);
        }
    }

    private IEnumerator UnlockChest()
    {
        ToggleInteractable(false);
        yield return new WaitForSeconds(_config.durationToUnlock);
        ToggleInteractable(true);
        _hasUnlocked = true;
        tileView.UpdateSprite(_config.unlockedSprite);
    }

    protected override void ResetSelf()
    {
        base.ResetSelf();
        _config = null;
        _rewardHelper = null;
        _hasUnlocked = false;
    }
}
