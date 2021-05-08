 using UnityEngine;

public class DrawVector : MonoBehaviour
{
    public Transform p1;
    public Transform p2;
    public bool difference;
    public bool normalized;
    public bool length;
    public bool scalarProductDot;
    public bool vectorProductCross;
    public bool debugDistance;

    private void OnDrawGizmos()
    {
        if(!p1 || !p2) return;
        
        Gizmos.DrawLine(Vector3.zero, p1.position);
        Gizmos.DrawLine(Vector3.zero, p2.position);

        if (difference)
        {
            Gizmos.color = Color.red;
        
            Gizmos.DrawLine(Vector3.zero, (p1.position - p2.position).normalized);
        
            Gizmos.color = Color.green;
        
            Gizmos.DrawLine(Vector3.zero, (p2.position - p1.position).normalized);
        }

        if (normalized)
        {
            Gizmos.color = Color.blue;
            //normalized = vector / magnitude

            Vector3 pos = p1.position;
            Vector3 normalized = pos / Mathf.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);

            Gizmos.DrawLine(Vector3.zero, normalized);
            Gizmos.DrawLine(Vector3.zero, p2.position.normalized);
        }

        if (length)
        {
            Vector3 pos = p1.position;
            
            float magnitude = Mathf.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
            float sqrtMagnitude = pos.x * pos.x + pos.y * pos.y + pos.z * pos.z;
            
            Debug.Log($"Magnitude {magnitude}");
            Debug.Log($"sqrt Magnitude {sqrtMagnitude}");
        }

        if (scalarProductDot)
        {
            Vector3 pos1 = p1.position.normalized;
            Vector3 pos2 = p2.position.normalized;

            float dot = pos1.x * pos2.x + pos1.y * pos2.y + pos1.z * pos2.z;
            Debug.Log($"Dot {dot}");
        }
        
        if (vectorProductCross)
        {
            Vector3 pos1 = p1.position;
            Vector3 pos2 = p2.position;
            
            //Vector3.Cross()s()

            Vector3 cross = new Vector3(
                pos1.y *  pos2.z - pos1.z * pos2.y, 
                pos1.z * pos2.x - pos1.x * pos2.z, 
                pos1.x *  pos2.y - pos1.y * pos2.x);
            
            Gizmos.color = Color.yellow;
            
            Gizmos.DrawLine(Vector3.zero,cross);
            
        }

        if (debugDistance)
        {
            Vector3 diff = p1.position - p2.position;
            float magnitude = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z);
            
            Debug.Log($"Distance {magnitude}");
        }
    }
}
