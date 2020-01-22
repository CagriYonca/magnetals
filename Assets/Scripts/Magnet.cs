using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // Metallerin mıknatısları etkilememesi eklenecek

    public enum Polarization {
        Positive,
        Negative
    }

    public float Permeability = 0.05f;
    public float MaxForce = 10000.0f;

    public float MagnetForce;
    public Polarization MagneticPole;
    public Rigidbody RigidBody;

    void Start()
    {

    }

    void FixedUpdate() 
    {
        var magnets = FindObjectsOfType<Magnet>();
        var magCount = magnets.Length;

        var metals = FindObjectsOfType<Metal>();
        var metCount = metals.Length;

        var accF1 = Vector3.zero;
        
        // calc magnetic force for all metals
        for(int i = 0; i < metCount; i++)
        {
            var m1 = metals[i];
            var rb1 = m1.RigidBody;

            var f = CalculateGilbertF2(m1);
            
            if(accF1.magnitude > MaxForce)
            {
                accF1 = accF1.normalized * MaxForce;
            }

            accF1 -= f * MagnetForce;
        }

        // calc magnetic force for all other magnets
        for(int i = 0; i < magCount; i++)
        {
            var m1 = magnets[i];
            var rb1 = m1.RigidBody;
            
            if(rb1.position == RigidBody.position)
            {
                continue;
            }

            var f = CalculateGilbertF(m1);
            var magnetForce = m1.MagnetForce * MagnetForce;
            if(accF1.magnitude > MaxForce)
            {
                accF1 = accF1.normalized * MaxForce;
            }

            accF1 += f * magnetForce;
        }

        RigidBody.AddForceAtPosition(accF1 * (-1) , RigidBody.position);
    }

    Vector3 CalculateGilbertF(Magnet magnet1)
    {       
        var m1 = magnet1.transform.position;
        var m2 = RigidBody.position;
        var r = m2 - m1;
        var dist = r.magnitude;
        var part0 = Permeability * magnet1.MagnetForce * MagnetForce;
        var part1 = 4 * Mathf.PI * dist;

        var f = (part0 / part1);

        if(magnet1.MagneticPole == MagneticPole)
        {
            f = -f;
        }

        return f * r.normalized;
    }

    Vector3 CalculateGilbertF2(Metal metal)
    {       
        var m1 = RigidBody.position;
        var m2 = metal.transform.position;
        var r = m2 - m1;
        var dist = r.magnitude;
        var part0 = Permeability * MagnetForce * 1;
        var part1 = dist;

        var f = (part0 / part1);

        return f * r.normalized;
    }

}
