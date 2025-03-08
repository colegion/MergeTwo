using GridSystem;
using Helpers;
using Interfaces;
using ScriptableObjects;
using UnityEngine;
using Grid = GridSystem.Grid;

namespace Tile
{
    public class BaseTile : MonoBehaviour, ITappable, IPoolable
    {
        [SerializeField] private Collider collider;
        [SerializeField] protected TileView tileView;
    
        protected int _x;
        protected int _y;
        protected int _layer;
    
        public int X => _x;
        public int Y => _y;
        public int Layer => _layer;

        protected Grid Grid;

        protected BaseItemConfig _stepConfig;
        private Vector2Int _position;
    
        public virtual void ConfigureSelf(BaseItemConfig config, int x, int y)
        {
            _stepConfig = config;
            _x = x;
            _y = y;
            _position = new Vector2Int(x, y);
            tileView.ConfigureSelf(_stepConfig.step);
            SetTransform();

            if(Grid == null) Grid = ServiceLocator.Get<Grid>();
            Grid.PlaceTileToParentCell(this);
        }
    
        public virtual void OnTap()
        {
       
        }

        public void UpdatePosition(Vector2Int position)
        {
            SetPosition(position);
            SetTransform();
            //move

        }

        public void SetLayer(int layer)
        {
            _layer = layer;
        }

        public void SetTransform()
        {
            if (Grid == null) Grid = ServiceLocator.Get<Grid>();

            BaseCell cell = Grid.GetCell(_x, _y);
            if (cell != null)
            {
                transform.position = cell.GetWorldPosition();
            }
            else
            {
                Debug.LogWarning($"Cell at {_x}, {_y} not found! Using fallback position.");
                transform.position = new Vector3(_x, 1, _y);
            }
        }

        private void SetPosition(Vector2Int position)
        {
            _position = position;
            _x = _position.x;
            _y = _position.y;
        }

        public void SetLocalPosition(int x, int y)
        {
            transform.localPosition = new Vector3(x, 0, y);
        }

        public void SetXCoordinate(int x)
        {
            _x = x;
        }

        public void SetYCoordinate(int y)
        {
            _y = y;
        }

        public BaseItemConfig GetItemConfig()
        {
            return _stepConfig;
        }

        protected virtual void ResetSelf()
        {
            _stepConfig = null;
            Grid.ClearTileOfParentCell(this);
            tileView.ResetSelf();
            tileView.ToggleVisuals(false);
            _position = Vector2Int.zero;
        }

        public Vector2Int GetPosition()
        {
            return _position;
        }
    
        public void ToggleInteractable(bool toggle)
        {
            collider.enabled = toggle;
        }

        public TileData GetTileData()
        {
            return new TileData()
            {
                itemLevel = _stepConfig.step.level,
                itemType = _stepConfig.step.ItemType
            };
        }

        public void OnPooled()
        {
            tileView.ToggleVisuals(false);
        }

        public void OnFetchFromPool()
        {
            tileView.ToggleVisuals(true);
        }

        public void OnReturnPool()
        {
            ResetSelf();
        }

        public PoolableTypes GetPoolableType()
        {
            return PoolableTypes.BaseTile;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
