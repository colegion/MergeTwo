using ScriptableObjects;
using UnityEngine;

namespace UI
{
    public class TrailObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void ConfigureSelf(BaseItemConfig config)
        {
            spriteRenderer.sprite = config.step.itemSprite;
        }
    }
}
