using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using ScriptableObjects.Orders;
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
        
        //ReceiveNewOrder();
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

        bool orderAllComplete = true;
        
        foreach (var request in _currentOrder.requests)
        {
            bool requestFound = false;
            
            foreach (var item in _grid.GetAllTilesOnBoard())
            {
                if (item.GetItemConfig().step.ItemType == request.step.ItemType)
                {
                    requestFound = true;
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
            _currentOrder.hasCompleted = true;
            OnOrderCompleted?.Invoke(ItemType.Coin, _currentOrder.rewardAmount);
            orderUiHelper.OnOrderCompleted(ReceiveNewOrder);
            Debug.Log("Order Completed!");
        }
    }
}
