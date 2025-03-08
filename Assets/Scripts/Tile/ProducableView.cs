using DG.Tweening;
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

        public void ShakeOnInvalid()
        {
            transform.DOShakeScale(.25f, 1.7f).SetEase(Ease.OutBounce);
        }
    }
}
