using System.Collections;
using Helpers;
using ScriptableObjects;
using ScriptableObjects.Items;
using UnityEngine;

namespace Tile
{
    public class Producer : BaseTile
    {
        [SerializeField] private Particle readyToProduce;
        private ProducerItemConfig _config;
        private RewardHelper _rewardHelper;
        private ItemFactory _itemFactory;

        private Coroutine _cooldownRoutine;
        private bool _cooldownActive;

        private ProducableView _producableView;
        private ParticleHelper _particleHelper;
        private Particle _activeParticle;
        public override void ConfigureSelf(BaseItemConfig config, int x, int y)
        {
            base.ConfigureSelf(config, x, y);
            if(_itemFactory == null) _itemFactory = ServiceLocator.Get<ItemFactory>();
            _config = (ProducerItemConfig)config;
            _rewardHelper = new RewardHelper(_config.producerCapacity.capacityConfigs);
            if (_particleHelper == null) _particleHelper = ServiceLocator.Get<ParticleHelper>();

            _producableView = (ProducableView)tileView;

            if (_config.canProduce)
                _producableView.ToggleEnergyBottle(true);
            
            readyToProduce.Play();
        }
    
        public override void OnTap()
        {
            if (!_config.canProduce) return;
        
            if (_cooldownActive)
            {
                _producableView.ShakeOnInvalid();
            }
            else
            {
                ProduceItem();
            }
        }

        private void ProduceItem()
        {
            if (!PlayerInventory.Instance.HasEnoughEnergy(_config.produceCost))
            {
                Debug.LogWarning("Insufficient energy");
                return;
            }
        
            if (_rewardHelper.IsEmpty() && _cooldownRoutine == null)
            {
                _cooldownRoutine = StartCoroutine(EnterCoolDown());
                return;
            }

            var itemToProduce = _rewardHelper.GetRandomItemToProduce();
            if (itemToProduce == null) return;
            _itemFactory.SpawnItemByConfig(itemToProduce);
            _rewardHelper.DecreaseRemainingCount(itemToProduce);
            PlayerInventory.Instance.SpendEnergy(_config.produceCost);
        }

        private IEnumerator EnterCoolDown()
        {
            _producableView.ToggleClock(true);
            readyToProduce.Stop();
            _cooldownActive = true;
            yield return new WaitForSeconds(_config.durationForRecharge);
            _cooldownActive = false;
            _producableView.ToggleClock(false);
            _rewardHelper.PopulateCapacities();
            _cooldownRoutine = null;
            readyToProduce.Play();
        }
    
        protected override void ResetSelf()
        {
            base.ResetSelf();
            _config = null;
            _rewardHelper = null;
            _cooldownActive = false;
            _producableView.ToggleClock(false);
        }
    }
}
