using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] private float mass;
    private static float baseMass = 1;
    private Rigidbody rb;

    void Awake()
    {
        // Prevents problems of 0/Negative mass
        if (mass <= 0)
            mass = baseMass;

        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Force);
    }

    // Calculates the force of gravity between this gravity object and another mass
    public Vector3 CalculateForce(float mass, Vector3 pos)
    {
        Vector3 direction = pos - this.Position;
        float len = direction.magnitude;

        return ((this.mass * mass * GravityManager.G) / (len * len * len)) * direction;
    }

    public float Mass { get { return mass; } }
    public Vector3 Position { get { return this.transform.position; } }
}
