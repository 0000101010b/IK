using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public class splines : MonoBehaviour {

    public List<Vector2> p;

    Matrix<double> SM = DenseMatrix.OfArray(new double[,] {
                {1,-3,3,-1},
                {0,3,-6,3},
                {0,0,3,-3},
                {0,0,0,1}});
    Matrix<double> tM;
    Matrix<double> points;


    public Vector3 cp;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cp, 0.1f);

        Gizmos.color = Color.white;
        foreach (Vector3 v in p)
            Gizmos.DrawWireSphere(v, 0.1f);

    }

    // Use this for initialization
    void Start () {
       points = DenseMatrix.OfArray(new double[,] {
                {p[0].x,p[1].x,p[2].x,p[3].x},
                {p[0].y,p[1].y,p[2].y,p[3].y}});


    }

    // Update is called once per frame
    float t = 0;
    void Update () {
        t += 0.01f;
        t = t % 1;
        tM = DenseMatrix.OfArray(new double[,] {
                {1},
                {t},
                {Mathf.Pow(t,2)},
                {Mathf.Pow(t,3)}
        });

        Matrix<double> ANS = points * SM * tM;
        Debug.Log(ANS.ToString());
        cp.x=(float)ANS[0, 0];
        cp.y=(float)ANS[1, 0];

    }
}
