using UnityEngine;

namespace Tile
{
    public class ChestView : TileView
    {
        [SerializeField] private SpriteRenderer clock;

        public void ToggleClock(bool value)
        {
            clock.enabled = value;
        }
    }
}
