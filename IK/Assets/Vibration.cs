using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics;

public class Vibration : MonoBehaviour {

    [Header ("Mass spring")]
    public Vector3 x;
    public float m;
    public float k;
    [Header("Simple Harmonic Motion")]
    public Vector3 pos;

    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(x, 0.2f);
    }

	// Use this for initialization
	void Start () {

        

        //Mathf.Exp();	
	}


    // Update is called once per frame
	void Update () {
        //float g = -9.8f;
        //float l = 2.0f;
        float w = Mathf.Sqrt(k / m);
        
        Debug.Log(w);
  
        x.y = 4* Mathf.Cos(w * Time.deltaTime + 0);


        //F = m * a;
        //F = -k * x.y;
        //ma+kx=0


        //x.x=0.5f*cos()
    }
}
