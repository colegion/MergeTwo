using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
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

    private ItemConfig _itemConfig;
    
    public virtual void ConfigureSelf(ItemConfig config, int x, int y)
    {
        _itemConfig = config;
        _x = x;
        _y = y;
        tileView.ConfigureSelf(_itemConfig);
        SetTransform();
    }

    public void OnFocus()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnTap()
    {
        throw new System.NotImplementedException();
    }

    public void SetLayer(int layer)
    {
        _layer = layer;
    }

    public void SetTransform()
    {
        transform.localPosition = new Vector3(_x, 1, _y);
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

    public ItemConfig GetItemConfig()
    {
        return _itemConfig;
    }

    private void ResetSelf()
    {
        _itemConfig = null;
        Grid.ClearTileOfParentCell(this);
        tileView.ResetSelf();
        tileView.ToggleVisuals(false);
    }

    public TileData GetTileData()
    {
        return new TileData()
        {
            itemLevel = _itemConfig.level,
            itemType = _itemConfig.itemType
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

    public GameObject GameObject()
    {
        return gameObject;
    }
}
