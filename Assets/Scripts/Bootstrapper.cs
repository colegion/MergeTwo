using System.Collections;
using System.Linq;
using Helpers;
using Interfaces;
using Pool;
using UnityEngine;
using Grid = GridSystem.Grid;

public class Bootstrapper : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(InitializeDependencies());
    }

    private IEnumerator InitializeDependencies()
    {
        var itemConfigManager = FindObjectOfType<ItemConfigManager>();
        while (!itemConfigManager.IsReady) 
        {
            yield return null;
        }

        Debug.Log("Bootstrapper: Registering dependencies...");

        var grid = new Grid(GameController.Instance.GridWidth, GameController.Instance.GridHeight);
        ServiceLocator.Register(grid);

        var poolController = FindObjectOfType<PoolController>();
        var orderController = FindObjectOfType<OrderController>();
        var itemFactory = FindObjectOfType<ItemFactory>();

        ServiceLocator.Register(poolController);
        ServiceLocator.Register(orderController);
        ServiceLocator.Register(itemFactory);
        
        foreach (var injectable in FindObjectsOfType<MonoBehaviour>().OfType<IInjectable>())
        {
            injectable.InjectDependencies();
        }

        Debug.Log("Bootstrapper: Dependencies injected, loading level...");
        GameController.Instance.LoadLevel();
    }
}