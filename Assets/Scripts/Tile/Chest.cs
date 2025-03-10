using System;
using System.Collections;
using DG.Tweening;
using Helpers;
using ScriptableObjects;
using ScriptableObjects.Items;
using UnityEngine;

namespace Tile
{
    public class Chest : BaseTile
    {
        [SerializeField] private ParticleSystem readyToProduce;
        [SerializeField] private ParticleSystem chestCleared;
        private ChestItemConfig _config;
        private RewardHelper _rewardHelper;
        private ItemFactory _itemFactory;
        private Coroutine _unlockRoutine;
        private bool _hasUnlocked;
        private ProducableView _producableView;
        private CountdownTimer _timer;
        
        public override void ConfigureSelf(BaseItemConfig config, int x, int y)
        {
            base.ConfigureSelf(config, x, y);
            _config = (ChestItemConfig)config;
            if(_itemFactory == null) _itemFactory = ServiceLocator.Get<ItemFactory>();
            _rewardHelper = new RewardHelper(_config.chestRewards.capacityConfigs);

            _producableView = (ProducableView)tileView;
            _timer = new CountdownTimer(_config.durationToUnlock);
        }

        public override void OnTap()
        {
            if (_unlockRoutine != null)
            {
                _producableView.ShakeOnInvalid();
                GameController.Instance.TriggerWarning($"Time left for opening: {_timer.GetFormattedTime()}");
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
            if (itemToProduce == null) return;
            _itemFactory.SpawnItemByConfig(itemToProduce, type: Utilities.GetPoolableType(itemToProduce.itemType));
            _rewardHelper.DecreaseRemainingCount(itemToProduce);

            if (_rewardHelper.IsEmpty())
            {
                chestCleared.Play();
                DOVirtual.DelayedCall(0.5f, ()=>
                {
                    GameController.Instance.ReturnPoolableToPool(this);
                });
            }
        }

        private IEnumerator UnlockChest()
        {
            _producableView.ToggleClock(true);
            var time = _config.durationToUnlock;

            for (int i = time; i > 0; i--)
            {
                _timer.SetTime(i);
                yield return new WaitForSeconds(1f);
            }
            
            yield return new WaitForSeconds(_config.durationToUnlock);
            _hasUnlocked = true;
            _producableView.ToggleClock(false);
            tileView.UpdateSprite(_config.unlockedSprite);
            _unlockRoutine = null;
            readyToProduce.Play();
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
