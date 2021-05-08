using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esphira : MonoBehaviour
{
    public float miniSphereRadius=0.1f;
    public float radius;
    public int total = 200;
    public float min;
    public float max;
    [Range(0,360)]
    public float angleStartDeg;
    [Range(0,360)]
    public float angleRangeDeg;
    
    private void OnDrawGizmos()
    {
        for (int i = 0; i < total; i++)
        {
            Gizmos.DrawSphere(transform.position + (Vector3)(transform.localToWorldMatrix * Point(radius, i,total, min, max, angleStartDeg, angleRangeDeg)),miniSphereRadius);
        }
    }

    /// golden angle in radians
    static float Phi = Mathf.PI * ( 3f - Mathf.Sqrt( 5f ) );
    static float TAU = Mathf.PI * 2;

    public static Vector3 Point( float radius , int index , int total , float min = 0f, float max = 1f , float angleStartDeg = 0f, float angleRangeDeg = 360 )
    {
        // y goes from min (-) to max (+)
        var y = ( ( index / ( total - 1f ) ) * ( max - min ) + min ) * 2f - 1f;

        // golden angle increment
        var theta = Phi * index ; 
        
        if( angleStartDeg != 0 || angleRangeDeg != 360 )
        {
            theta = ( theta % ( TAU ) ) ;
            theta = theta < 0 ? theta + TAU : theta ;
            
            var a1 = angleStartDeg * Mathf.Deg2Rad;
            var a2 = angleRangeDeg * Mathf.Deg2Rad;
            
            theta = theta * a2 / TAU + a1;
        }

        // https://stackoverflow.com/a/26127012/2496170
    
        // radius at y
        var rY = Mathf.Sqrt( 1 - y * y ); 
    
        var x = Mathf.Cos( theta ) * rY;
        var z = Mathf.Sin( theta ) * rY;

        return  new Vector3( x, y, z ) * radius;
    }
}
