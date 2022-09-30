using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
A data structure that stores all the Gravity Objects in an Octree so
that calculating the force of all Gravity Objects in each frame is
O(NlogN) instead of O(N^2) like the brute force method.
*/
public class Octree
{
    private Cube c;
    private Octree[] nodes;
    private float mass;
    private Vector3 com;
    private bool hasChildren;
    private static float threshold = 0.5f;

    private Octree(Cube c)
    {
        this.c = c;

        mass = 0;
        com = Vector3.zero;
    }

    public Octree(GravityObject[] g)
    {
        // Finds the object furthest away from origin in the octree
        float maxDistanceSqrd = 0;
        for (int i = 0; i < g.Length; i++)
            maxDistanceSqrd = Mathf.Max(maxDistanceSqrd, g[i].Position.sqrMagnitude);

        // That constant is slightly above 4/sqrt(3) to ensure that all objects are within
        // the cube of the root Octree.
        float length = Mathf.Sqrt(maxDistanceSqrd) * 2.31f;
        c = new Cube(Vector3.one * (-0.5f) * length, length);

        mass = 0;
        com = Vector3.zero;

        for (int i = 0; i < g.Length; i++)
            Insert(g[i]);
    }

    public void Insert(GravityObject g)
    {
        Insert(g.Mass, g.Position);
    }

    public void Insert(float mass, Vector3 pos)
    {
        // If this node has no mass, then store the details of the Gravity Object in the node
        if (this.mass == 0)
        {
            this.mass = mass;
            this.com = pos;
            return;
        }

        // If the node has no children then insert the Gravity Object stored in this node and
        // insert the Gravity Object passed through function into the children
        if (!hasChildren)
        {
            CreateChildren();
            
            InsertInChild(this.mass, this.com);
        }

        InsertInChild(mass, pos);

        // Recalculate the total mass and centre of mass of the node
        float totalMass = this.mass + mass;
        Vector3 newCom = Vector3.zero;

        newCom.x = centreOfMass(this.mass, this.com.x, mass, pos.x, totalMass);
        newCom.y = centreOfMass(this.mass, this.com.y, mass, pos.y, totalMass);
        newCom.z = centreOfMass(this.mass, this.com.z, mass, pos.z, totalMass);

        this.mass = totalMass;
        this.com = newCom;
    }

    // Inserts a Gravity object in the child node
    private void InsertInChild(float mass, Vector3 pos)
    {
        int octant = c.Octant(pos);
        if (nodes[octant] == null)
            nodes[octant] = new Octree(new Cube(c, octant));

        nodes[octant].Insert(mass, pos);
    }

    // Creates the 8 sub Octree nodes
    private void CreateChildren()
    {
        this.nodes = new Octree[8];
        hasChildren = true;
    }

    // Calculates a new centre of mass when new Gravity Object is added
    private float centreOfMass(float mass1, float pos1, float mass2, float pos2, float totalMass)
    {
        return (mass1 * pos1 + mass2 * pos2) / totalMass;
    }

    // Calculates the force exerted on a gravity object by all other objects O(NlogN)
    public Vector3 Force(GravityObject g)
    {
        // Gravity object cannot have force applied by itself
        if (g.Position == this.com && g.Mass == this.mass)
            return Vector3.zero;

        float distance = (g.Position - this.com).magnitude;
        float ratio = c.Length / distance;

        // If the centre of mass is sufficiently far away of a small enough node
        // then we just do an approximation by applying the force to centre of
        // mass instead of calculating exact force with each subnode
        if (!hasChildren || ratio < threshold)
        {
            return g.CalculateForce(this.mass, this.com);
        }

        // Otherwise we calculate the force applied by each node
        Vector3 totalForce = Vector3.zero;

        for (int i = 0; i < nodes.Length; i++)
            if (nodes[i] != null)
                totalForce += nodes[i].Force(g);

        return totalForce;
    }
}
