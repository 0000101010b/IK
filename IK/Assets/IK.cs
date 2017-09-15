using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Globalization;

public class IK : MonoBehaviour {

    public Vector3 g;

    public Joint[] arm;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //arm
        Gizmos.DrawLine(Vector3.zero,arm[1].s);
        //forarm
        Gizmos.DrawLine(arm[1].s, arm[1].s + arm[2].s);

        //joints
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Vector3.zero,new Vector3(0.5f,0.5f, 0.5f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(arm[1].s, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(arm[1].s +arm[2].s, 0.5f);

        //target
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(g, new Vector3(0.5f, 0.5f, 0.5f));

    }

    public Vector3[] init_pos;

    public float angle1;
    public float angle2;

    public bool reachTarget=true;
    // Use thisfor initialization
	void Start () {

        arm[1].s= Quaternion.AngleAxis(-45, Vector3.forward) * arm[1].s;
        arm[2].s = Quaternion.AngleAxis(-90, Vector3.forward) * arm[2].s;

        init_pos = new Vector3[2];
        init_pos[0] = arm[1].s;
        init_pos[1] = arm[2].s;
    }




    IEnumerator Jacobian_1DOF()
    {
        Vector3 axis = Vector3.forward;
        Vector3  e = arm[2].s + arm[1].s;

        while (Vector3.Distance(g, e) > 0.1f)
        {
            yield return new WaitForSeconds(0.05f);

            e = arm[2].s + arm[1].s;//root
            Vector<double> Z = JacobianPsuedoInverse(new Vector3(e.x, e.y, 0),new Vector3(g.x,g.y,0), Vector3.forward);
        }
    }


    public Vector<double> JacobianPsuedoInverse(Vector3 e,Vector3 _g,Vector3 axis)
    {
        e = arm[2].s + arm[1].s;//root

        Vector3 j1 = Vector3.Cross(axis, e.normalized);
        Vector3 j2 = Vector3.Cross(axis, (e - arm[1].s).normalized);


        Matrix<double> J = DenseMatrix.OfArray(new double[,] {
                {j1.x,j2.x},
                {j1.y,j2.y},
                {j1.z,j2.z} });

        Matrix<double> Jp = J.PseudoInverse();

        Vector3 de = _g - e;
        de.Normalize();

        Vector<double> V = DenseVector.OfArray(new double[] { de.x, de.y, de.z });
        V = Jp * V * 1f;

        /*
                float v1 = float.Parse(V[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                float v2 = float.Parse(V[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                Vector3 R = new Vector3(v1, v2);
                */

        for (int i = 1; i < arm.Length; i++)
        {
            if (axis == Vector3.up)
            {
                arm[i].angle.y += (float)V[i - 1];
                arm[i].s = Quaternion.AngleAxis(arm[i].angle.y % 360, axis) * new Vector3(init_pos[i - 1].x,0, init_pos[i - 1].x);
            }
            else if (axis == Vector3.right)
            {
                arm[i].angle.x += (float)V[i - 1];
                arm[i].s = Quaternion.AngleAxis(arm[i].angle.x % 360, axis) * new Vector3(0,init_pos[i - 1].x, init_pos[i - 1].x);
            }
            else
            {
                arm[i].angle.z += (float)V[i - 1];
                arm[i].s = Quaternion.AngleAxis(arm[i].angle.z % 360, axis) * new Vector3(init_pos[i - 1].x, init_pos[i - 1].x,0);
            }

            //arm[i].s = Quaternion.AngleAxis(arm[i].angle.x % 360,Vector3.right ) * init_pos[i-1];
            //arm[i].s = Quaternion.AngleAxis(arm[i].angle.y % 360, Vector3.up) * arm[i].s;
            //arm[i].s = Quaternion.AngleAxis(arm[i].angle.z % 360, Vector3.forward) * init_pos[i - 1];

        }
        return V;
        //angle1 += R.x;
        //angle2 += R.y;
    }


	
	// Update is called once per frame
	void Update () {
		if(reachTarget)
        {
            StartCoroutine(Jacobian_1DOF());
            reachTarget = false;
        }
	}
}
[System.Serializable]
public class Joint
{
    public Vector3 s;//pos
    public Vector3 v;//dir
    public Vector3 angle;

}
