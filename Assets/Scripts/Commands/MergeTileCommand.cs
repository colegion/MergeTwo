using Interfaces;
using Tile;

namespace Commands
{
    public class MergeTileCommand : ITileCommand
    {
        private readonly TileMerger _tileMerger;
        private readonly BaseTile _selectedTile;
        private readonly BaseTile _targetTile;
        private BaseTile _mergedTile;

        public MergeTileCommand(TileMerger tileMerger, BaseTile selectedTile, BaseTile targetTile)
        {
            _tileMerger = tileMerger;
            _selectedTile = selectedTile;
            _targetTile = targetTile;
        }

        public void Execute()
        {
            _mergedTile = _tileMerger.Merge(_selectedTile, _targetTile);
            GameController.Instance.OnTapPerformed(_mergedTile);
            GameController.Instance.PlaceOutline(_mergedTile);
        }
        
    }
}