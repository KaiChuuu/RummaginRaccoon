using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BackgroundObject
{
    public int NumberToSpawn;
    public GameObject ObjectToSpawn;
}

public class BackgroundElementSpawner : MonoBehaviour
{
    public float MinX, MaxX;
    public float MinY, MaxY;

    public List<BackgroundObject> backgroundObjects = new();

    private void Start()
    {
        SpawnBackgroundObjects();
    }

    private void SpawnBackgroundObjects()
    {
        foreach (BackgroundObject obj in backgroundObjects)
        {
            for (int i = 0; i < obj.NumberToSpawn; i++)
            {
                GameObject g = Instantiate(obj.ObjectToSpawn, new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY)), Quaternion.identity);
                g.transform.localScale *= Random.Range(1, 1.25f);
                g.transform.SetParent(transform, true);
            }
        }
    }
}
