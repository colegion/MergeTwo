using Interfaces;
using Services;
using Tile;

namespace Commands
{
    public class SwapTileCommand : ITileCommand
    {
        private readonly TileSwapper _tileSwapper;
        private readonly BaseTile _selectedTile;
        private readonly BaseTile _targetTile;

        public SwapTileCommand(TileSwapper tileSwapper, BaseTile selectedTile, BaseTile targetTile)
        {
            _tileSwapper = tileSwapper;
            _selectedTile = selectedTile;
            _targetTile = targetTile;
        }

        public void Execute()
        {
            _tileSwapper.Swap(_selectedTile, _targetTile);
            GameController.Instance.PlaceOutline(_targetTile);
        }

        // Optional: Undo()
    }
}