using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using ScriptableObjects;
using ScriptableObjects;
using UnityEngine;

public class BaseTile : MonoBehaviour, ITappable, IPoolable
{
    [SerializeField] private TileView tileView;
    
    protected int _x;
    protected int _y;
    protected int _layer;
    
    public int X => _x;
    public int Y => _y;
    public int Layer => _layer;

    protected Grid Grid;

    protected BaseStepConfig _stepConfig;
    private Vector2Int _position;
    
    public virtual void ConfigureSelf(BaseStepConfig config, int x, int y)
    {
        _stepConfig = config;
        _x = x;
        _y = y;
        _position = new Vector2Int(x, y);
        tileView.ConfigureSelf(_stepConfig);
        SetTransform();

        Grid = ServiceLocator.Get<Grid>();
        Grid.PlaceTileToParentCell(this);
    }

    public void ConfigureSelf_2<T>(T config, int x, int y) where T : BaseStepConfig
    {
        _stepConfig = config;
        _x = x;
        _y = y;
        _position = new Vector2Int(x, y);
        tileView.ConfigureSelf(_stepConfig);
        SetTransform();

        Grid = ServiceLocator.Get<Grid>();
        Grid.PlaceTileToParentCell(this);
    }

    public void OnFocus()
    {
    }

    public virtual void OnTap()
    {
       
    }

    public void UpdatePosition(Vector2Int position)
    {
        SetPosition(position);
        SetTransform();
        //move

    }

    public void SetLayer(int layer)
    {
        _layer = layer;
    }

    public void SetTransform()
    {
        transform.localPosition = new Vector3(_x, 1, _y);
    }

    private void SetPosition(Vector2Int position)
    {
        _position = position;
        _x = _position.x;
        _y = _position.y;
    }

    public void SetLocalPosition(int x, int y)
    {
        transform.localPosition = new Vector3(x, 0, y);
    }

    public void SetXCoordinate(int x)
    {
        _x = x;
    }

    public void SetYCoordinate(int y)
    {
        _y = y;
    }

    public BaseStepConfig GetItemConfig()
    {
        return _stepConfig;
    }

    private void ResetSelf()
    {
        _stepConfig = null;
        Grid.ClearTileOfParentCell(this);
        tileView.ResetSelf();
        tileView.ToggleVisuals(false);
        _position = Vector2Int.zero;
    }

    public Vector2Int GetPosition()
    {
        return _position;
    }

    public TileData GetTileData()
    {
        return new TileData()
        {
            itemLevel = _stepConfig.level,
            itemType = _stepConfig.ItemType
        };
    }

    public void OnPooled()
    {
        tileView.ToggleVisuals(false);
    }

    public void OnFetchFromPool()
    {
        tileView.ToggleVisuals(true);
    }

    public void OnReturnPool()
    {
        ResetSelf();
    }

    public PoolableTypes GetPoolableType()
    {
        return PoolableTypes.BaseTile;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
