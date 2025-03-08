using System.Collections;
using System.Collections.Generic;
using Helpers;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private SpriteRenderer tileRenderer;

    public void ConfigureSelf(BaseStepConfig config)
    {
        tileRenderer.sprite = config.itemSprite;
    }

    public void UpdateSprite(Sprite sprite)
    {
        tileRenderer.sprite = sprite;
    }

    public void ResetSelf()
    {
        tileRenderer.sprite = null;
    }

    public void ToggleVisuals(bool toggle)
    {
        visuals.SetActive(toggle);
    }
}
