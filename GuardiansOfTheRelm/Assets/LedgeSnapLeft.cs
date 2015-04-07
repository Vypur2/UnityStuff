using UnityEngine;
using System.Collections;

public class LedgeSnapLeft : MonoBehaviour {
	CharacterController cntrl;
	private Vector3 moveDirection = Vector3.zero;
	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		Move c = other.gameObject.GetComponent<Move> ();
		if (c.FacingRight)
		{
			if (c.LedgeGrabCount == 0) 
			{
				c.TimeSinceLastGrab = 0;
				c.IsOnLeftLedge = true;
				other.transform.position = this.transform.position;
				Rigidbody body = other.GetComponent<Rigidbody> ();
				body.constraints = RigidbodyConstraints.FreezeAll;
				c.IsOnLeftLedge = true;
			} 
			else 
			{
				if (c.TimeSinceLastGrab < .75) 
				{
					;
				} 
				else 
				{
					other.transform.position = this.transform.position;
					Rigidbody body = other.GetComponent<Rigidbody> ();
					body.constraints = RigidbodyConstraints.FreezeAll;
					c.IsOnLeftLedge = true;
					c.TimeSinceLastGrab = 0;
				}
			}
		} 
	}

	void OnCollisionStay(Collision otherinfo)
	{
		Move c = otherinfo.gameObject.GetComponent<Move> ();
		if (c.FacingRight) 
		{
			if (c.TimeSinceLastGrab > .75)
			{
				otherinfo.transform.position = this.transform.position;
				Rigidbody body = otherinfo.gameObject.GetComponent<Rigidbody> ();
				body.constraints = RigidbodyConstraints.FreezeAll;
				c.IsOnLeftLedge = true;
				c.TimeSinceLastGrab = 0;
			}
		}

	}

	
	// Update is called once per frame
	void Update () {

	}
}
