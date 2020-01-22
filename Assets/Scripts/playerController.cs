using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
	// Raycast edilen nesneye velocity setlemesi yapılacak
	private GameObject item;
	public int forceStrength = 200;

    void FixedUpdate()
    {
    	if(Input.GetMouseButton(0))
    	{
    		RaycastHit firstHit, secHit;
    		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
			if(Physics.Raycast(ray, out firstHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("DragTargets"))) {
    			if(firstHit.collider.tag == "Draggable")
    			{
    				item = firstHit.collider.gameObject;
    				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if(Physics.Raycast(ray, out secHit))
					{
						Vector3 direction = secHit.point - item.transform.position;
						Vector3 newPos = new Vector3(direction.x, 0, direction.z);
						item.GetComponent<Rigidbody>().AddForce(newPos * 1000);
					}
    			}
    		}
    	}    
    }
}
