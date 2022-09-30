using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube
{
    private Vector3 bottomCorner;
    private float length;

    public float Length { get { return length; } }

    public Cube(Vector3 bottomCorner, float length)
    {
        this.bottomCorner = bottomCorner;
        this.length = length;
    }

    // Creates a cube that is an octant (corner with half length) of a bigger cube 
    public Cube(Cube prev, int octant)
    {
        this.length = prev.length / 2;
        this.bottomCorner = prev.bottomCorner;
        
        if (octant / 4 == 0) this.bottomCorner.z += this.length;
        if ((octant / 2) % 2 == 0) this.bottomCorner.y += this.length;
        if (octant % 2 == 0) this.bottomCorner.x += this.length;
    }

    // Returns -1 if not in cube. Otherwise returns the octant the Vector is in
    public int Octant(Vector3 vec)
    {
        Vector3 diff = vec - bottomCorner;

        if (diff.x < 0 || diff.x > length || diff.y < 0 || diff.y > length || diff.z < 0 || diff.z > length)
            return -1;

        double halfLength = length/2;
        int octant = 0;

        octant += diff.z < halfLength ? 4 : 0;
        octant += diff.y < halfLength ? 2 : 0;
        octant += diff.x < halfLength ? 1 : 0;

        return octant;
    }

    public override string ToString()
    {
        return bottomCorner.ToString() + " " + length;
    }
}
