using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Snake : MonoBehaviour
{

    public List<Transform> segments = new List<Transform>();
    public float dist=1;

    private void Update()
    {

        for (int i = 0; i < segments.Count; i++)
        {
            var segment = segments[i];
            Vector3 positionS = segments[i].transform.position;
            Vector3 targetS = i == 0 ? transform.position : segments[i - 1].transform.position;
            Vector3 diff = positionS - targetS; 
            diff.Normalize();
            segment.transform.position = targetS + dist * diff;
        }
    }

    // private void OnDrawGizmosSelected()
    // {
    //     for (int i = 0; i < segments.Count; i++)
    //     {
    //         Gizmos.DrawWireSphere(segments[i].position,dist/2f);
    //     }
    // }
}
