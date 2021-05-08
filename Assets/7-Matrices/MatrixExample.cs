using System.Numerics;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Vector3 = UnityEngine.Vector3;

public class MatrixExample : MonoBehaviour
{
    public bool debugMatrix;
    public bool debugLocal;
    public bool debugWorld;
    public Transform target;
    public Vector3 local;
    
    private void OnDrawGizmos()
    {
        if (debugLocal)
        {
            Debug.Log($"Local {transform.InverseTransformPoint(target.position)}"); 
            //matrix4X4.inverse.MultiplyPoint();  matrix4X4 * vector4 (10,5,4,1)
        }


        if (debugWorld)
        {
            Debug.Log($"World {transform.TransformPoint(local)}"); //matrix4X4.MultiplyPoint(local);
            Gizmos.DrawSphere(transform.TransformPoint(local),0.1f);
        }
        
        if (debugMatrix)
        {
            Debug.Log(transform.localToWorldMatrix);
            
            Matrix4x4 matrix4X4 = transform.localToWorldMatrix;// * target.localToWorldMatrix;
            matrix4X4.SetTRS(transform.position,transform.rotation,transform.lossyScale);
            Vector3 forward = matrix4X4.GetColumn(2);
            Vector3 pos = matrix4X4.GetColumn(3);

            Gizmos.DrawSphere(pos, 0.1f);
            Gizmos.DrawLine(pos, pos + forward);
        }
    }
}
