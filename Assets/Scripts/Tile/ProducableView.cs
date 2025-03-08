using UnityEngine;

namespace Tile
{
    public class ProducableView : TileView
    {
        [SerializeField] private SpriteRenderer clock;

        public void ToggleClock(bool value)
        {
            clock.enabled = value;
        }
    }
}
