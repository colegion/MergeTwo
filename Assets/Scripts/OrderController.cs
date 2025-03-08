using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using ScriptableObjects.Orders;
using UI;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [SerializeField] private OrderUIHelper orderUiHelper;
    [SerializeField] private List<OrderConfig> orders;

    private readonly Queue<OrderConfig> _orders = new Queue<OrderConfig>();
    private OrderConfig _currentOrder;
    private Grid _grid;

    public static event Action<ItemType, int> OnOrderCompleted;

    public void Initialize()
    {
        _grid = ServiceLocator.Get<Grid>();

        foreach (var order in orders)
        {
            _orders.Enqueue(order);
        }
        
        //ReceiveNewOrder();   
    }

    public void ReceiveNewOrder()
    {
        _currentOrder = _orders.Dequeue();
        orderUiHelper.ConfigureSelf(_currentOrder);
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
            ReceiveNewOrder();
            Debug.Log("Order Completed!");
        }
    }
}
