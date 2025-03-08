using System.Collections.Generic;
using Tile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GridSystem
{
    public class Grid
    {
        private BaseCell[,] _board;
        public int Width { get; private set; }
        public int Height { get; private set; }

        private List<BaseCell> _availableCells;

        private List<BaseTile> _tilesOnBoard;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            _board = new BaseCell[width, height];
            _availableCells = new List<BaseCell>();
            _tilesOnBoard = new List<BaseTile>();
        }

        public void PlaceCell(BaseCell cell)
        {
            _board[cell.X, cell.Y] = cell;
        }

        public BaseCell GetCell(int x, int y)
        {
            if (!IsCoordinateValid(x, y)) return null;
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
                _tilesOnBoard.Add(tile);
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
                _tilesOnBoard.Remove(tile);
            }
        }

        public List<BaseTile> GetAllTilesOnBoard()
        {
            return _tilesOnBoard;
        }
    

        public Transform GetCellTargetByCoordinate(int x, int y)
        {
            return _board[x, y].GetTarget();
        }

        public bool IsCoordinateValid(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

    }
}