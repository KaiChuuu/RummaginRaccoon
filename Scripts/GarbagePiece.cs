using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbagePiece : MonoBehaviour, IResetable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            AudioManager.instance.PlaySound("Pickup");
            StageManager.instance.OnGarbageCollected();
        }
    }

    public void ResetObject()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
