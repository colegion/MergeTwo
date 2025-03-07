using System;
using DG.Tweening;
using Helpers;
using Interfaces;
using Pool;
using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class TrailObject : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject visuals;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void ConfigureSelf(BaseItemConfig config)
        {
            spriteRenderer.sprite = config.step.itemSprite;
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }

        public void MoveTowardsTarget(Transform target, Action onComplete)
        {
            transform.DOJump(target.position, .3f, 1, .3f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                
                onComplete?.Invoke();
            });
        }

        public void OnPooled()
        {
            visuals.SetActive(false);
        }

        public void OnFetchFromPool()
        {
            visuals.SetActive(true);
        }

        public void OnReturnPool()
        {
            visuals.SetActive(false);
            spriteRenderer.sprite = null;
            transform.position = Vector3.zero;
        }

        public PoolableTypes GetPoolableType()
        {
            return PoolableTypes.TrailObject;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
