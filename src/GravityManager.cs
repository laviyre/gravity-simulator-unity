using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    GravityObject[] g;
    public static float G;
    [SerializeField] private float gravitationalConstant = 1f;
    private bool gravityObjectsLoaded = false;

    void Awake()
    {
        G = gravitationalConstant;
    }

    void FixedUpdate()
    {
        if (!gravityObjectsLoaded)
            LoadGravityObjects();

        //BarnesHut();
        BruteForce();
    }

    // Calculates the force of each object using Barnes Hut Algorithm O(NLogN)
    void BarnesHut()
    {
        Octree octree = new Octree(g);

        for (int i = 0; i < g.Length; i++)
            g[i].AddForce(octree.Force(g[i]));
    }

    // Calculates the force of each object using Brute Force O(N^2)
    void BruteForce()
    {
        for (int i = 0; i < g.Length; i++)
        {
            Vector3 force = Vector3.zero;

            for (int j = 0; j < g.Length; j++)
            {
                if (i != j)
                    force += g[i].CalculateForce(g[j].Mass, g[j].Position);
            }

            g[i].AddForce(force);
        }
    }

    public void SpawnGravityObject(GravityObject gravityObject, Vector3 position, Quaternion rotation)
    {
        Instantiate(gravityObject, position, rotation, this.transform);
        gravityObjectsLoaded = false;
    }

    void LoadGravityObjects()
    {
        g = GetComponentsInChildren<GravityObject>();
        gravityObjectsLoaded = true;
    }
}
