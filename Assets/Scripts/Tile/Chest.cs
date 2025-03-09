using System.Collections;
using Helpers;
using ScriptableObjects;
using ScriptableObjects.Items;
using UnityEngine;

namespace Tile
{
    public class Chest : BaseTile
    {
        private ChestItemConfig _config;
        private RewardHelper _rewardHelper;

        private ItemFactory _itemFactory;
    
        private Coroutine _unlockRoutine;
        private bool _hasUnlocked;

        private ProducableView _producableView;
        public override void ConfigureSelf(BaseItemConfig config, int x, int y)
        {
            base.ConfigureSelf(config, x, y);
            _config = (ChestItemConfig)config;
            if(_itemFactory == null) _itemFactory = ServiceLocator.Get<ItemFactory>();
            _rewardHelper = new RewardHelper(_config.chestRewards.capacityConfigs);

            _producableView = (ProducableView)tileView;
        }

        public override void OnTap()
        {
            if (_unlockRoutine != null)
            {
                _producableView.ShakeOnInvalid();
            }
            else
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
            _producableView.ToggleClock(true);
            yield return new WaitForSeconds(_config.durationToUnlock);
            _hasUnlocked = true;
            _producableView.ToggleClock(false);
            tileView.UpdateSprite(_config.unlockedSprite);
        }

        protected override void ResetSelf()
        {
            base.ResetSelf();
            _config = null;
            _rewardHelper = null;
            _hasUnlocked = false;
            _producableView.ToggleClock(false);
        }
    }
}
