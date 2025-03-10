using System;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Pool;
using Tile;
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

        public void IncreaseCurrency(ItemType type, int amount)
        {
            AnimateCurrencyClaim(type, amount);
        }

        private void AnimateCurrencyClaim(ItemType type, int amount)
        {
            if (_poolController == null) _poolController = ServiceLocator.Get<PoolController>();
            if (_itemConfigManager == null) _itemConfigManager = ServiceLocator.Get<ItemConfigManager>();

            var targetHUD = currencies.Find(c => c.GetCurrencyType() == type);
            if (targetHUD == null)
            {
                Debug.LogWarning($"No currency HUD found for type {type}");
                return;
            }

            var item = _itemConfigManager.GetItemConfig(type, SpritePlaceHolderConfigLevel);
            Vector3 targetWorldPos = GetWorldPositionFromRectTransform(targetHUD.GetTarget());

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
                    trail.MoveTowardsTarget(targetWorldPos, () =>
                    {
                        _poolController.ReturnPooledObject(trail);
                        targetHUD.IncreaseCurrency(1);
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

        private Vector3 GetWorldPositionFromRectTransform(Transform targetTransform)
        {
            if (targetTransform == null)
            {
                Debug.LogWarning("Target Transform is null");
                return Vector3.zero;
            }

            RectTransform rectTransform = targetTransform.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogWarning("Target does not have RectTransform component");
                return targetTransform.position;
            }

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning("No Canvas found in parent of RectTransform");
                return targetTransform.position;
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane + 5f));

                // Optional: debug line
                Debug.DrawLine(rectTransform.position, worldPos, Color.green, 2f);

                return worldPos;
            }

            // World Space canvas doesn't need conversion
            return rectTransform.position;
        }

        private void HandleOnCurrencyEarned(ItemType type, int amount)
        {
            IncreaseCurrency(type, amount);
        }

        private void AddListeners()
        {
            OrderController.OnOrderCompleted += HandleOnCurrencyEarned;
            SpecialTile.OnCurrencyGathered += HandleOnCurrencyEarned;
        }

        private void RemoveListeners()
        {
            OrderController.OnOrderCompleted -= HandleOnCurrencyEarned;
            SpecialTile.OnCurrencyGathered -= HandleOnCurrencyEarned;
        }
    }
}
