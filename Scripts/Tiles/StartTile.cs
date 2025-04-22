using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTile : Tile
{
    public Vector2 GetStartPosition()
    {
        return (Vector2)transform.position + new Vector2(0, 2);
    }

    public override void ResetObject()
    {
        
    }
}
