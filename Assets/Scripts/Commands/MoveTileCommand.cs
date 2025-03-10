using Interfaces;
using Tile;
using UnityEngine;

namespace Commands
{
    public class MoveTileCommand : ITileCommand
    {
        private readonly BaseTile _selectedTile;
        private readonly Vector2Int _targetPosition;

        public MoveTileCommand(BaseTile selectedTile, Vector2Int targetPosition)
        {
            _selectedTile = selectedTile;
            _targetPosition = targetPosition;
        }

        public void Execute()
        {
            _selectedTile.UpdatePosition(_targetPosition);
            GameController.Instance.PlaceOutline(_selectedTile);
        }

        // Optional: Undo()
    }
}