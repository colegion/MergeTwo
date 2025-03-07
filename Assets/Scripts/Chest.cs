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
        _itemFactory = ServiceLocator.Get<ItemFactory>();
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
        if (_rewardHelper.IsEmpty() && _unlockRoutine == null)
        {
            _unlockRoutine = StartCoroutine(UnlockChest());
            return;
        }

        var itemToProduce = _rewardHelper.GetRandomItemToProduce();
        _itemFactory.SpawnItemByConfig(itemToProduce);
        _rewardHelper.DecreaseRemainingCount(itemToProduce);
    }

    private IEnumerator UnlockChest()
    {
        ToggleInteractable(false);
        yield return new WaitForSeconds(_config.durationToUnlock);
        ToggleInteractable(true);
        _hasUnlocked = true;
        tileView.UpdateSprite(_config.unlockedSprite);
    }
}
