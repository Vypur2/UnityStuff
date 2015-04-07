using UnityEngine;
using System.Collections;

public class ActiveStates : MonoBehaviour {
	GameObject player1;

	// Use this for initialization
	void Start () {
		player1 = GameObject.Find ("Bob");
	}
	
	// Update is called once per frame
	void Update () {

		if (!player1.activeSelf) {
			print("errur");
		}
	}
}
