using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Commands;
using GridSystem;
using Helpers;
using JetBrains.Annotations;
using Pool;
using ScriptableObjects;
using ScriptableObjects.Items;
using Services;
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
    [SerializeField] private ParticleHelper particleHelper;
    [SerializeField] private GameObject selectionOutline;
    
    private LevelManager _levelManager;
    private Grid _grid;
    private TileMerger _tileMerger;
    private TileSwapper _tileSwapper;
    private CommandInvoker _commandInvoker;
    
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
    public static event Action<string> OnWarningNeeded;

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
        
        _tileMerger = new TileMerger(itemFactory, particleHelper, poolController);
        _tileSwapper = new TileSwapper();
        _commandInvoker = new CommandInvoker();
        
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

        if (TileComparator.IsConfigsIdentical(originConfig, targetConfig) &&
            !TileComparator.IsTilesIdentical(selectedTile, targetTile))
        {
            if (originConfig.step.isMaxLevel && targetConfig.step.isMaxLevel)
            {
                TriggerWarning("Item already reached max level!");
                return;
            }

            var mergeCommand = new MergeTileCommand(_tileMerger, selectedTile, targetTile);
            _commandInvoker.ExecuteCommand(mergeCommand);
        }
        else
        {
            var swapCommand = new SwapTileCommand(_tileSwapper, selectedTile, targetTile);
            _commandInvoker.ExecuteCommand(swapCommand);
        }
    }

    public void OnSwipeReleased(BaseTile selectedTile, Vector2Int targetPosition)
    {
        if (selectedTile == null) return;

        var moveCommand = new MoveTileCommand(selectedTile, targetPosition);
        _commandInvoker.ExecuteCommand(moveCommand);
    }

    public void PlaceOutline(BaseTile tile)
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

    public void TriggerWarning(string warning)
    {
        OnWarningNeeded?.Invoke(warning);
    }

    public void AppendLevelTiles(TileData data)
    {
        _levelTiles.Add(data);
    }

    public void RemoveDataFromLevelTiles(TileData data)
    {

        for (int i = _levelTiles.Count - 1; i >= 0; i--)
        {
            if (_levelTiles[i].IsIdentical(data))
            {
                _levelTiles.Remove(_levelTiles[i]);
            }    
        }
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
