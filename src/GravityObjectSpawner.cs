using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObjectSpawner : MonoBehaviour
{
    [SerializeField] private GravityObject spawn;
    [SerializeField] private GravityManager manager;
    [SerializeField] private float radius;
    [SerializeField] private int spawnNumber;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            for (int i = 0; i < spawnNumber; i++)
                SpawnObject();
        }
    }

    void SpawnObject()
    {
        Vector3 position = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * radius;
        if (position.sqrMagnitude > radius * radius)
        {
            SpawnObject();
            return;
        }

        manager.SpawnGravityObject(spawn, position, Quaternion.identity);
    }
}
