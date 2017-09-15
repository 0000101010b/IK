using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interplation : MonoBehaviour {


    public Vector3 p1;
    public Vector3 p2;
    public Vector3 pI;

    public float t1;
    public float t2;
    public float tI;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(p1, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(p2, 0.2f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(p1, p2);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pI, 0.1f);

    }

	// Use this for initialization
	void Start () {
        float alpha = (tI - t1) / (t2 - t1);
        pI = (1 - alpha) * p1 + alpha * p2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
