using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Helpers;
using JetBrains.Annotations;
using Pool;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using IPoolable = Interfaces.IPoolable;

public class GameController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Transform puzzleTransform;
    [SerializeField] private PoolController poolController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ItemFactory itemFactory;
    [SerializeField] private ItemConfigManager itemConfigManager;
    
    private LevelManager _levelManager;
    private Grid _grid;

    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

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
        _grid = new Grid(width, height);
        ServiceLocator.Register<Grid>(_grid);
        cameraController.SetGridSize(width, height);
        poolController.Initialize();
        ServiceLocator.Register(poolController);
        _levelManager = new LevelManager(puzzleTransform);
        ServiceLocator.Register(itemFactory);
        itemFactory.PopulateBoard();
    }

    public void OnTapPerformed(BaseTile tile = null)
    {
        OnUserTapped?.Invoke(tile);
    }
    
    public void OnSwipeReleased(BaseTile selectedTile, BaseTile targetTile)
    {
        if (selectedTile == null || targetTile == null) return;

        var originConfig = selectedTile.GetItemConfig();
        var targetConfig = targetTile.GetItemConfig();

        if (TileComparator.IsIdentical(originConfig, targetConfig))
        {
            MergeTiles(selectedTile, targetTile);
        }
        else
        {
            SwapTiles(selectedTile, targetTile);
        }
    }

    public void OnSwipeReleased(BaseTile selectedTile, Vector2Int targetPosition)
    {
        if (selectedTile == null) return;
        MoveTileToPosition(selectedTile, targetPosition);
    }

    private void MergeTiles(BaseTile selectedTile, BaseTile targetTile)
    {
        var nextStep = selectedTile.GetItemConfig().nextItem;
        var targetPos = targetTile.GetPosition();
        var tempTile = poolController.GetPooledObject(PoolableTypes.BaseTile);
        if (tempTile.GetGameObject().TryGetComponent(out BaseTile tile))
        {
            tile.ConfigureSelf(nextStep, targetPos.x, targetPos.y);
            ReturnPoolableToPool(selectedTile);
            ReturnPoolableToPool(targetTile);
        }
        else
        {
            Debug.LogWarning("There is an unexpected behaviour on merging tiles");
        }
        
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

    public BaseCell GetCell(int x, int y)
    {
        return _grid.GetCell(x, y);
    }

    public void ReturnPoolableToPool(IPoolable poolable)
    {
        poolController.ReturnPooledObject(poolable);
    } 

    private void OnDestroy()
    {
        List<TileData> tileData = new List<TileData>();
        var tiles = _grid.GetAllTilesOnBoard();

        foreach (var tile in tiles)
        {
            tileData.Add(tile.GetTileData());
        }

        var levelData = new LevelData()
        {
            boardWidth = _grid.Width,
            boardHeight = _grid.Height,
            tiles = tileData
        };
        
        _levelManager.SaveLevel(levelData);
    }
}

public abstract partial class TileComparator
{
    public static bool IsIdentical(BaseItemConfig first, BaseItemConfig second)
    {
        var firstStep = first.step;
        var secondStep = second.step;
        return firstStep.ItemType == secondStep.ItemType && firstStep.level == secondStep.level;
    }
}
