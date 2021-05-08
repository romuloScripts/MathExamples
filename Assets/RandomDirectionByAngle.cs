using UnityEngine;

public class RandomDirectionByAngle : MonoBehaviour
{
    [Header("Use this field to see a new random direction (in magenta)")]
    public bool CALCULATE_NEW_RANDOM_DIRECTION;
    public bool debugMaxAngles = true;

    [Header("Transform to simulate a direction")]
    public Transform refTrans;
    Vector3 refDirection => refTrans.position - transform.position;
    Vector3 calculatedDir;
    Vector3 lol;

    float x, y;

    [Header("Max angle of rotation in all axes")]
    public float maxAngle;

    [System.Serializable]
    public struct TestDir
    {
        public float xAngle, yAngle;
        public Color color;
    }

    public TestDir[] testDirections;

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, refDirection);

        Vector3 maxVerticalDir = refDirection.RotateDirectionAngleAxis(0, maxAngle);
        Vector3 minVerticalDir = refDirection.RotateDirectionAngleAxis(0, -maxAngle);
        Vector3 maxHorizontalDir = refDirection.RotateDirectionAngleAxis(maxAngle, 0);
        Vector3 minHorizontalDir = refDirection.RotateDirectionAngleAxis(-maxAngle, 0);

        if (debugMaxAngles) {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, maxVerticalDir);
            Gizmos.DrawRay(transform.position, minVerticalDir);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, maxHorizontalDir);
            Gizmos.DrawRay(transform.position, minHorizontalDir);
        }

        if (CALCULATE_NEW_RANDOM_DIRECTION) {
            CALCULATE_NEW_RANDOM_DIRECTION = false;
            float randomAngleX = Random.Range(-maxAngle, maxAngle);
            float randomAngleY = Random.Range(-maxAngle, maxAngle);

            calculatedDir = refDirection.RotateDirectionAngleAxis(randomAngleX, randomAngleY);
        }

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, lol);


        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, calculatedDir);
        
        foreach (var d in testDirections) {
            Gizmos.color = d.color;
            Gizmos.DrawRay(transform.position, refDirection.RotateDirectionAngleAxis(d.xAngle, d.yAngle));
        }

        //Vector3 v = refDirection.RotateDirectionAngleAxis(Random.Range(-maxAngle, maxAngle), Random.Range(-maxAngle, maxAngle));
        for (float _x = -maxAngle; _x <= maxAngle; _x+=4) {
            for (float _y = -maxAngle; _y <= maxAngle; _y+=4) {
                Vector3 v = refDirection.RotateDirectionAngleAxis(_x, _y);
                Debug.DrawRay(transform.position + v, v * 0.05f, Color.magenta);
            }

        }
    }
}
