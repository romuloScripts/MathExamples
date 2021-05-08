using UnityEngine;

public static class ExtendVectorClass
{
    /// <summary>
    /// Rotates the vector by the angles on the axis specified.
    /// </summary>
    /// <param name="direction"> The vector to be rotated </param>
    /// <param name="xAngle"> The angle of rotation on the X axis </param>
    /// <param name="yAngle"> The angle of rotation on the Y axis </param>
    /// <returns></returns>
    public static Vector3 RotateDirectionAngleAxis(this Vector3 direction, float xAngle, float yAngle) {
        direction.Normalize();

        Matrix4x4 trs = new Matrix4x4();
        trs.SetTRS(Vector3.zero, Quaternion.LookRotation(direction), Vector3.one);

        return trs * RotationMatrix(trs, xAngle, true) * RotationMatrix(trs, yAngle, false) * Vector3.forward;

        Matrix4x4 RotationMatrix(Matrix4x4 trsOfDir, float angle, bool usingXAxis) {
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);
            Matrix4x4 rotationMatrix = Matrix4x4.identity;

            rotationMatrix.SetRow(0, usingXAxis ? new Vector3(1, 0, 0) : new Vector3(cos, 0, sin));
            rotationMatrix.SetRow(1, usingXAxis ? new Vector3(0, cos, -sin) : new Vector3(0, 1, 0));
            rotationMatrix.SetRow(2, usingXAxis ? new Vector3(0, sin, cos) : new Vector3(-sin, 0, cos));

            return rotationMatrix;
        }
    }
}
