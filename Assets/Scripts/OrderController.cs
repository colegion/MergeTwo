using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using ScriptableObjects.Orders;
using Tile;
using UI;
using UnityEngine;
using Grid = GridSystem.Grid;

public class OrderController : MonoBehaviour, IInjectable
{
    [SerializeField] private OrderUIHelper orderUiHelper;
    [SerializeField] private List<OrderConfig> orders;

    private readonly Queue<OrderConfig> _orders = new Queue<OrderConfig>();
    private int _orderIndex;
    private OrderConfig _currentOrder;
    private Grid _grid;

    public static event Action<ItemType, int> OnOrderCompleted;
    
    public void InjectDependencies()
    {
        _grid = ServiceLocator.Get<Grid>();
    }

    public void Initialize()
    {
        foreach (var order in orders)
        {
            _orders.Enqueue(order);
        }
        
        ReceiveNewOrder();
    }

    public void ReceiveNewOrder()
    {
        if (_orders.Count > 0)
        {
            _currentOrder = _orders.Dequeue();
            _orderIndex++;
            orderUiHelper.ConfigureSelf(_currentOrder, _orderIndex);
        }
        else
        {
            orderUiHelper.DisableSelf();
        }
    }

    public void OnNewItemCreated()
    {
        if (_currentOrder == null) return;
        List<BaseTile> orderTiles = new List<BaseTile>();

        bool orderAllComplete = true;
        
        foreach (var request in _currentOrder.requests)
        {
            bool requestFound = false;
            
            foreach (var item in _grid.GetAllTilesOnBoard())
            {
                var config = item.GetItemConfig();
                if (config.step.ItemType == request.step.ItemType && config.step.level == request.step.level)
                {
                    requestFound = true;
                    orderTiles.Add(item);
                    break; 
                }
            }
            
            if (!requestFound)
            {
                orderAllComplete = false;
                break;
            }
        }
        
        if (orderAllComplete)
        {
            Sequence sequence = DOTween.Sequence();
            _currentOrder.hasCompleted = true;

            for (int i = 0; i < orderTiles.Count; i++)
            {
                var i1 = i;
                sequence.InsertCallback((i + 1) * 0.5f, () =>
                {
                    orderTiles[i1].transform.DOMove(orderUiHelper.transform.position, 0.45f).SetEase(Ease.InCubic);
                });
            }

            sequence.AppendCallback(() =>
            {
                orderUiHelper.OnOrderCompleted(ReceiveNewOrder);
                OnOrderCompleted?.Invoke(ItemType.Coin, _currentOrder.rewardAmount);
                foreach (var tile in orderTiles)
                {
                    GameController.Instance.ReturnPoolableToPool(tile);
                }
            });
            
            Debug.Log("Order Completed!");
        }
    }
}
