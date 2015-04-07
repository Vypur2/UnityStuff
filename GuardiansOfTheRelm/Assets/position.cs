using UnityEngine;
using System.Collections;

public class position : MonoBehaviour {
	private GameObject center;

	private GameObject p1;
	float avgx;
	float avgy;
	float avgz;

	// Use this for initialization
	void Start () {
		center = GameObject.Find ("CenterStage");
		p1 = GameObject.Find ("Bob");
		if (center) {
			print(center.name);
		}
		if (p1)
		{
			print(p1.name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		avgx = (center.transform.position.x + p1.transform.position.x) / 2;
		avgy = (center.transform.position.y + p1.transform.position.y) / 2;
		avgz = (center.transform.position.z + p1.transform.position.z) / 2;

		this.transform.position.Set (avgx, avgy, avgz);
	}
}
