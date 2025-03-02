using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileRenderer;

    public void ConfigureSelf(ItemConfig config)
    {
        tileRenderer.sprite = config.itemSprite;
    }

    public void ResetSelf()
    {
        tileRenderer.sprite = null;
    }
}
