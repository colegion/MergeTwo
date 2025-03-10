using Tile;

namespace Services
{
    public class TileSwapper
    {
        public void Swap(BaseTile selectedTile, BaseTile targetTile)
        {
            var firstCoordinate = selectedTile.GetPosition();
            var secondCoordinate = targetTile.GetPosition();

            selectedTile.UpdatePosition(secondCoordinate);
            targetTile.UpdatePosition(firstCoordinate);
        }
    }
}