using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Helpers;
using JetBrains.Annotations;
using Pool;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Transform puzzleTransform;
    [SerializeField] private PoolController poolController;
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

    private void Start()
    {
        /*_grid = new Grid(width, height);
        ServiceLocator.Register<Grid>(_grid);
        poolController.Initialize();
        ServiceLocator.Register(poolController);
        ServiceLocator.Register(itemConfigManager);
        _levelManager = new LevelManager(_grid, puzzleTransform);*/
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

    public void OnSwipeReleased(BaseTile selectedTile, Vector2 targetPosition)
    {
        if (selectedTile == null) return;
        
        MoveTileToPosition(selectedTile, targetPosition);
    }

    private void MergeTiles(BaseTile selectedTile, BaseTile targetTile)
    {
        var currentStep = selectedTile.GetItemConfig();
        var nextStep = itemConfigManager.GetItemConfig(currentStep.ItemType, currentStep.level+1);
        var targetPos = targetTile.GetPosition();
        var tempTile = poolController.GetPooledObject(PoolableTypes.BaseTile);
        if (tempTile.GetGameObject().TryGetComponent(out BaseTile tile))
        {
            tile.ConfigureSelf(nextStep, targetPos.x, targetPos.y);
            poolController.ReturnPooledObject(selectedTile);
            poolController.ReturnPooledObject(targetTile);
        }
        else
        {
            Debug.LogWarning("There is an unexpected behaviour on merging tiles");
        }
        
    }

    private void SwapTiles(BaseTile selectedTile, BaseTile targetTile)
    {
    }

    private void MoveTileToPosition(BaseTile selectedTile, Vector2 targetPosition)
    {
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

public partial class TileComparator
{
    public static bool IsIdentical(BaseStepConfig first, BaseStepConfig second)
    {
        return first.ItemType == second.ItemType && first.level == second.level;
    }
}
