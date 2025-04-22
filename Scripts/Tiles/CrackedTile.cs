using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedTile : Tile
{
    private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _underSpriteRenderer;
    [SerializeField] private SpriteRenderer _borderSpriteRenderer;
    [SerializeField] private SpriteRenderer _crackSpriteRenderer;
    [SerializeField] private Sprite _crackSprite;
    [SerializeField] private Sprite _holeSprite;

    private float _defaultUnderAlpha = 1f;

    private void Start()
    {
        _defaultUnderAlpha = _underSpriteRenderer.color.a;
        _collider = GetComponent<BoxCollider2D>();
    }

    protected override void OnPlayerExit()
    {
        BreakTile();
    }

    private void BreakTile()
    {
        AudioManager.instance.PlaySound("Crumble");
        _collider.enabled = false;
        _borderSpriteRenderer.color = new Color(_borderSpriteRenderer.color.r, _borderSpriteRenderer.color.g, _borderSpriteRenderer.color.b, 0);
        _underSpriteRenderer.color = new Color(_underSpriteRenderer.color.r, _underSpriteRenderer.color.g, _underSpriteRenderer.color.b, 0);
        _crackSpriteRenderer.sprite = _holeSprite;
    }

    public override void ResetObject()
    {
        _borderSpriteRenderer.color = new Color(_borderSpriteRenderer.color.r, _borderSpriteRenderer.color.g, _borderSpriteRenderer.color.b, 1);
        _underSpriteRenderer.color = new Color(_underSpriteRenderer.color.r, _underSpriteRenderer.color.g, _underSpriteRenderer.color.b, _defaultUnderAlpha);
        _collider.enabled = true;
        _crackSpriteRenderer.sprite = _crackSprite;
    }
}
