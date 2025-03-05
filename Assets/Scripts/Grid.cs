using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid
{
    private BaseCell[,] _board;
    public int Width { get; private set; }
    public int Height { get; private set; }

    private List<BaseCell> _availableCells;

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
        _board = new BaseCell[width, height];
        _availableCells = new List<BaseCell>();
    }

    public BaseCell GetCell(int x, int y)
    {
        return _board[x, y];
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
        if (_board[cell.X, cell.Y] != null)
        {
            Debug.LogError($"Specified coordinate already holds for another cell! Coordinate: {cell.X} {cell.Y}");
        }
        else
        {
            _board[cell.X, cell.Y] = cell;
        }
    }

    public void PlaceTileToParentCell(BaseTile tile)
    {
        var cell = _board[tile.X, tile.Y];
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
        var cell = _board[tile.X, tile.Y];
        if (cell == null)
        {
            Debug.LogWarning($"Given tile has no valid coordinate X: {tile.X} Y: {tile.Y}");
        }
        else
        {
            cell.SetTileNull(tile.Layer);
        }
    }

    public List<BaseTile> GetAllTilesOnBoard()
    {
        List<BaseTile> allTiles = new List<BaseTile>();
        foreach (var cell in _board)
        {
            var cellTiles = cell.GetAssignedTiles();
            allTiles.AddRange(cellTiles);
        }

        return allTiles;
    }

    public Transform GetCellTargetByCoordinate(int x, int y)
    {
        return _board[x, y].GetTarget();
    }
    
}