using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour 
{
	int oldLayer = 1;
	int voidLayer;

	void Start () 
	{
		voidLayer = 31;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name.Equals ("Cube 1")) 
		{
			other.gameObject.layer = oldLayer;
			GameObject plat = GameObject.Find("Platform");
			plat.layer = voidLayer;
			if (plat)
			{
				Physics.IgnoreLayerCollision(1,31,true);

			}

		} 	
		if (other.gameObject.name.Equals ("Bob")) 
		{
			other.gameObject.layer = oldLayer;
			GameObject plat2 = GameObject.Find("Platform");
			plat2.layer = voidLayer;
			if (plat2)
			{
				Physics.IgnoreLayerCollision(1,31,true);
				
			}
			
		} 	
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name.Equals ("Cube 1")) 
		{
			other.gameObject.layer = oldLayer + 1;
		} 
		if (other.gameObject.name.Equals ("bob")) 
		{
			other.gameObject.layer = oldLayer + 1;
		} 
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
