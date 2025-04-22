using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour, IResetable
{
    private bool _isTouchingPlayer = false ;

    private void Awake()
    {
        if (TryGetComponent(out BoxCollider2D collider))
        {
            collider.isTrigger = true;
        }
        else
        {
            BoxCollider2D _collider = gameObject.AddComponent<BoxCollider2D>();
            _collider.isTrigger = true;
        }

        gameObject.layer = LayerMask.NameToLayer("Tiles");
    }

    public bool IsTouchingPlayer()
    {
        return _isTouchingPlayer;
    }

    protected virtual void OnPlayerEnter() { }
    protected virtual void OnPlayerExit() { }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isTouchingPlayer = true;
            OnPlayerEnter();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _isTouchingPlayer = false;
            OnPlayerExit();
        }
    }

    public abstract void ResetObject();
}
