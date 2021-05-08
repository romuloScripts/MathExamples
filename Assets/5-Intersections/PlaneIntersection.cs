using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneIntersection : MonoBehaviour
{
    public enum TestType
    {
        Ray,
        Point,
    }

    public TestType testType;
    public Transform rayTransform;
    public Transform point;
    public bool reflect;

    [Header("Editor")]
    public Vector3 normal;
    public float planeDistance;

    private void OnDrawGizmos()
    {
        normal = transform.forward;
        planeDistance = Vector3.Dot(normal, transform.position);

        switch (testType)
        {
            case TestType.Ray:
                RaycastOnPlane();
                break;

            case TestType.Point:
                PointOnPlane();
                break;
        }
    }
    
    private void RaycastOnPlane()
    {
        if (!rayTransform) return;
        Ray ray = new Ray(rayTransform.position,rayTransform.forward);
        Gizmos.DrawLine(ray.origin,ray.origin+ ray.direction * 1000);
        var t = transform;
        
        Plane plane = new Plane(t.forward, t.position);

        float distance = plane.Raycast(ray);
        
        if (!float.IsNaN(distance))
        {
            Vector3 point = ray.GetPoint(distance);

            Gizmos.DrawSphere(point,1f);

            if (reflect)
            {
                Vector3 reflected = plane.Reflect(ray.direction);
                Gizmos.DrawLine(point,point + reflected * 100);
            }
        }
    }

    private void PointOnPlane()
    {
        if (!point) return;
        
        var t = transform;
        Plane plane = new Plane(t.forward, t.position);

        float distance = Mathf.Abs(plane.GetDistanceToPoint(point.position));

        if (distance < 0.3f)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(point.position,1f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point.position,1f);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(plane.ClosestPointOnPlane(point.position),1);
        }
    }
    
    public class Ray
    {
        public Vector3 origin;
        public Vector3 direction;
        
        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }
        
        
        public Vector3 GetPoint(float distance)
        {
            return origin + direction * distance;
        }
    }
    
    public class Plane
    {
        private Vector3 normal;
        private float distance;
    
        public Plane(Vector3 normal, Vector3 point)
        {
            this.normal = Vector3.Normalize(normal);
            distance = Vector3.Dot(this.normal, point);
        }
        
        public float Raycast(Ray ray)
        {
            //t = D -N.S / N.V
            
            float nv = Vector3.Dot(ray.direction, normal);
            
            if (nv>= 0.0f)
            {
                return float.NaN;
            }
            
            float nsd = distance -Vector3.Dot(ray.origin, normal);
            
            float t = nsd / nv;
            if(t >= 0.0)
                return t;
            return float.NaN;
        }
    
        public float GetDistanceToPoint(Vector3 point)
        {
            return Vector3.Dot(normal, point) - distance;
        }
        
        public Vector3 ClosestPointOnPlane(Vector3 point)
        {
            //dot * vetor -> projeção ortografica
            return point - normal * GetDistanceToPoint(point);
        }
        
        public Vector3 Reflect(Vector3 dir)
        {
            return dir -2f * normal * Vector3.Dot(normal, dir);
        }
    }
}




