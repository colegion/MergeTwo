using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using GridSystem;
using Helpers;
using JetBrains.Annotations;
using Pool;
using ScriptableObjects;
using ScriptableObjects.Items;
using Tile;
using Unity.VisualScripting;
using UnityEngine;
using Grid = GridSystem.Grid;
using IPoolable = Interfaces.IPoolable;

public class GameController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Transform puzzleTransform;
    [SerializeField] private PoolController poolController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ItemFactory itemFactory;
    [SerializeField] private OrderController orderController;
    [SerializeField] private GameObject selectionOutline;
    
    private LevelManager _levelManager;
    private Grid _grid;
    private List<TileData> _levelTiles = new List<TileData>();

    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    public int GridWidth => width;
    public int GridHeight => height;

    public static event Action<BaseTile> OnUserTapped;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel()
    {
        Debug.Log("GameController: Loading Level...");
        _grid = ServiceLocator.Get<Grid>();
        cameraController.SetGridSize(width, height);
        poolController.Initialize();
        _levelManager = new LevelManager(puzzleTransform);
        
        orderController.Initialize();
    }

    public void OnTapPerformed(BaseTile tile = null)
    {
        OnUserTapped?.Invoke(tile);
        if (tile != null)
        {
            PlaceOutline(tile);
        }
    }
    
    public void OnSwipeReleased(BaseTile selectedTile, BaseTile targetTile)
    {
        if (selectedTile == null || targetTile == null) return;

        var originConfig = selectedTile.GetItemConfig();
        var targetConfig = targetTile.GetItemConfig();

        if (TileComparator.IsConfigsIdentical(originConfig, targetConfig) && !TileComparator.IsTilesIdentical(selectedTile, targetTile))
        {
            MergeTiles(selectedTile, targetTile);
            PlaceOutline(targetTile);
        }
        else
        {
            SwapTiles(selectedTile, targetTile);
            PlaceOutline(targetTile);
        }
    }

    public void OnSwipeReleased(BaseTile selectedTile, Vector2Int targetPosition)
    {
        if (selectedTile == null) return;
        MoveTileToPosition(selectedTile, targetPosition);
        PlaceOutline(selectedTile);
    }

    private void MergeTiles(BaseTile selectedTile, BaseTile targetTile)
    {
        var nextStep = selectedTile.GetItemConfig().nextItem;
        var targetPos = targetTile.GetPosition();
        var objectType = nextStep.itemType == ItemType.VegetableProducer
            ? PoolableTypes.Producer
            : PoolableTypes.BaseTile;
        ReturnPoolableToPool(selectedTile);
        ReturnPoolableToPool(targetTile);
        itemFactory.SpawnItemByConfig(nextStep, targetPos, objectType);
    }

    private void SwapTiles(BaseTile selectedTile, BaseTile targetTile)
    {
        var firstCoordinate = selectedTile.GetPosition();
        var secondCoordinate = targetTile.GetPosition();
        selectedTile.UpdatePosition(secondCoordinate);
        targetTile.UpdatePosition(firstCoordinate);
    }

    private void MoveTileToPosition(BaseTile selectedTile, Vector2Int targetPosition)
    {
        selectedTile.UpdatePosition(targetPosition);
    }

    private void PlaceOutline(BaseTile tile)
    {
        var cell = _grid.GetCell(tile.X, tile.Y);
        selectionOutline.transform.position = cell.GetWorldPosition();
    }
    
    public BaseCell GetCell(int x, int y)
    {
        return _grid.GetCell(x, y);
    }

    public void ReturnPoolableToPool(IPoolable poolable)
    {
        poolController.ReturnPooledObject(poolable);
    }

    public void AppendLevelTiles(TileData data)
    {
        _levelTiles.Add(data);
    }

    public void RemoveDataFromLevelTiles(TileData data)
    {
        _levelTiles.Remove(data);
    }

    private void OnDestroy()
    {
        var levelData = new LevelData()
        {
            boardWidth = _grid.Width,
            boardHeight = _grid.Height,
            tiles = _levelTiles
        };
        
        _levelManager.SaveLevel(levelData);
    }
}

public abstract partial class TileComparator
{
    public static bool IsConfigsIdentical(BaseItemConfig first, BaseItemConfig second)
    {
        var firstStep = first.step;
        var secondStep = second.step;
        return firstStep.ItemType == secondStep.ItemType && firstStep.level == secondStep.level && !firstStep.isMaxLevel;
    }

    public static bool IsTilesIdentical(BaseTile first, BaseTile second)
    {
        return first == second;
    }
}
