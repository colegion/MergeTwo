using System;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private List<CurrencyHUD> currencies;
        [SerializeField] private float offset;
        [SerializeField] private float baseDelay;
        private PoolController _poolController;
        private ItemConfigManager _itemConfigManager;
        private const int SpritePlaceHolderConfigLevel = 0;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        [ContextMenu("Test animation")]
        public void Test()
        {
            IncreaseCurrency(ItemType.Coin, 10);
        }
        
        public void IncreaseCurrency(ItemType type, int amount)
        {
            AnimateCurrencyClaim(type, amount);
        }

        private void AnimateCurrencyClaim(ItemType type, int amount)
        {
            if (_poolController == null) _poolController = ServiceLocator.Get<PoolController>();
            if (_itemConfigManager == null) _itemConfigManager = ServiceLocator.Get<ItemConfigManager>();
            var target = currencies.Find(c => c.GetCurrencyType() == type);
            var item = _itemConfigManager.GetItemConfig(type, SpritePlaceHolderConfigLevel);
            Sequence sequence = DOTween.Sequence();
            
            for (int i = 0; i < amount; i++)
            {
                var tempTrail = _poolController.GetPooledObject(PoolableTypes.TrailObject);
                var trailGo = tempTrail.GetGameObject();
                trailGo.transform.position = GetRandomPositionAroundCenter();
                var trail = trailGo.GetComponent<TrailObject>();
                trail.ConfigureSelf(item);
                sequence.InsertCallback(baseDelay * (i + 1), () =>
                {
                    trail.MoveTowardsTarget(target.GetTarget(), () =>
                    {
                        _poolController.ReturnPooledObject(trail);
                        target.IncreaseCurrency(1);
                    });
                });
            }
        }

        private Vector3 GetRandomPositionAroundCenter()
        {
            Vector3 worldCenter = Vector3.zero;
            
            float randomX = Random.Range(-offset, offset);
            float randomZ = Random.Range(-offset, offset);
            
            return worldCenter + new Vector3(randomX, 2, randomZ);
        }

        private void HandleOnOrderCompleted(ItemType type, int amount)
        {
            IncreaseCurrency(type, amount);
        }

        private void AddListeners()
        {
            OrderController.OnOrderCompleted += HandleOnOrderCompleted;
        }

        private void RemoveListeners()
        {
            OrderController.OnOrderCompleted -= HandleOnOrderCompleted;
        }


    }
}
