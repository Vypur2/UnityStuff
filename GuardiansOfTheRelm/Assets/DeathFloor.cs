using UnityEngine;
using System.Collections;

public class DeathFloor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		Move c = other.gameObject.GetComponent<Move> ();
		GameObject spawnloc = GameObject.Find ("Spawn");
		other.transform.position = spawnloc.transform.position;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
