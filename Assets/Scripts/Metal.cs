using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : MonoBehaviour
{
	public Rigidbody RigidBody;
	public float Permeability = 0.05f;
    public float MaxForce = 10000.0f;

    // Start is called before the first frame update

	Vector3 CalculateGilbertF(Magnet magnet)
    {       
        var m1 = magnet.transform.position;
        var m2 = RigidBody.position;
        var r = m2 - m1;
        var dist = r.magnitude;
        var part0 = Permeability * magnet.MagnetForce * 150;
        var part1 = 4 * dist;

        var f = (part0 / part1);

        return f * r.normalized;
    }
    // Update is called once per frame
    void Update()
    {
        var magnets = FindObjectsOfType<Magnet>();
        var magCount = magnets.Length;

        var accF1 = Vector3.zero;
        
        for(int i = 0; i < magCount; i++)
        {
        	var m1 = magnets[i];
			var f = CalculateGilbertF(m1);
        	
        	if(accF1.magnitude > MaxForce)
        	{
        		accF1 = accF1.normalized * MaxForce;
        	}
        	accF1 += f * m1.MagnetForce;
        }
        RigidBody.AddForce(accF1 * (-1));
    }
}
