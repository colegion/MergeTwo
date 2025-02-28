using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid
{
    private BaseCell[,] _world;
    public int Width { get; private set; }
    public int Height { get; private set; }

    private List<BaseCell> _availableCells;

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        _world = new BaseCell[width, height];
        _availableCells = new List<BaseCell>();
    }

    public void AppendAvailableCells(BaseCell cell)
    {
        if (_availableCells.Contains(cell)) return;
        _availableCells.Add(cell);
    }

    public void RemoveCellFromAvailableCells(BaseCell cell)
    {
        if (_availableCells.Contains(cell))
        {
            _availableCells.Remove(cell);
        }
    }

    public BaseCell GetAvailableRandomCell()
    {
        var index = Random.Range(0, _availableCells.Count);
        return _availableCells[index];
    }

    public void SetCell(BaseCell cell)
    {
        if (_world[cell.X, cell.Y] != null)
        {
            Debug.LogError($"Specified coordinate already holds for another cell! Coordinate: {cell.X} {cell.Y}");
        }
        else
        {
            _world[cell.X, cell.Y] = cell;
        }
    }

    public void PlaceTileToParentCell(BaseTile tile)
    {
        var cell = _world[tile.X, tile.Y];
        if (cell == null)
        {
            Debug.LogWarning($"Given tile has no valid coordinate X: {tile.X} Y: {tile.Y}");
        }
        else
        {
            cell.SetTile(tile);
        }
    }

    public void ClearTileOfParentCell(BaseTile tile)
    {
        var cell = _world[tile.X, tile.Y];
        if (cell == null)
        {
            Debug.LogWarning($"Given tile has no valid coordinate X: {tile.X} Y: {tile.Y}");
        }
        else
        {
            cell.SetTileNull(tile.Layer);
        }
    }

    public Transform GetCellTargetByCoordinate(int x, int y)
    {
        return _world[x, y].GetTarget();
    }
    
}