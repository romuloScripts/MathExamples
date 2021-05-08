using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Interpolation
{
    Lerp,
    SmoothStep,
    Repeat,
    PingPong,
    Eerp,
    Berp,
    Bounce,
    Sinerp,
    Coserp,
    Sigmoid,
    Sigmoid2
}

public class InterpolationFormulas : MonoBehaviour
{
    public Interpolation interpolation;
    [Range(-10f, 10f)] public float start = 0f;
    [Range(0f, 10f)] public float end = 1f;
    [Range(1f, 10f)] public float max = 1f;
    [Range(0.001f, 1f)] public float stepDrawGizmos = 0.01f;

    [Header("Play Mode")]
    public float speed = 1;
    public bool useTargetPoints;
    public Transform p1, p2;
    public bool moveTowards;
    public bool scale;

    private float _time;
    
    private void OnEnable()
    {
        _time = 0;
    }

    private void Update()
    {
        _time += speed * Time.deltaTime;

        _time = Mathf.Clamp(_time, 0, max);
        
        if (useTargetPoints)
        {
            if (moveTowards && p2)
            {
                var position = transform.position;
                var position2 = p2.position;
           
                Vector3 newPos = new Vector3(
                    MoveTowards(position.x,position2.x,speed* Time.deltaTime),
                    MoveTowards(position.y,position2.y,speed* Time.deltaTime),
                    MoveTowards(position.z,position2.z,speed* Time.deltaTime)
                );

                transform.position = newPos;
                return;
            }
            
            if (p1 && p2)
            {
                if(scale)
                    transform.localScale = GetPos(p1.localScale,p2.localScale,_time);
                else
                {
                    transform.position = GetPos(p1.position,p2.position,_time);
                }
            }
        }
        else
        {
            transform.position = GetPos(_time);
        }
    }

    private void OnDrawGizmos()
    {
        float x = 0;
        float y = GetValue(x);
        
        var pos = new Vector3(x,y);
        
        while (x < max)
        {
            x += stepDrawGizmos;
            x = Mathf.Clamp(x,0,max);
            y = GetValue(x);
            
            var newPos = new Vector3(x,y);

            Gizmos.DrawLine(pos,newPos);
            pos = newPos;
        }
    }

    private Vector3 GetPos(Vector3 po1, Vector3 pos2, float t)
    {
        return Lerp(po1,pos2,GetValue(t));
    }

    private Vector3 GetPos(float t)
    {
        float x = t;
        float y = GetValue(x);
        
        return new Vector3(x,y);
    }

    float GetValue(float t)
    {
        switch (interpolation)
        {
            case Interpolation.Lerp:
                t = Lerp(start, end, t); 
                break;
            case Interpolation.Berp:
                t = Berp(start, end, t); 
                break;
            case Interpolation.Bounce:
                t = Bounce(t); 
                break;
            case Interpolation.Eerp:
                t = Eerp(start, end, t); 
                break;
            case Interpolation.Repeat: 
                t = Repeat(t, end); 
                break;
            case Interpolation.PingPong: 
                t = PingPong(t, end); 
                break;
            case Interpolation.Sinerp: 
                t = Sinerp(start, end, t); 
                break;
            case Interpolation.SmoothStep: 
                t = SmoothStep(start, end, t); 
                break;
            case Interpolation.Coserp: 
                t = Coserp(start, end, t); 
                break;
            case Interpolation.Sigmoid: 
                t = Sigmoid(Remap(0,1,-10,10,t)); 
                break;
            case Interpolation.Sigmoid2: 
                t = Sigmoid(Lerp(start,end,t)); 
                break;
        }

        return t;
    }
    
    public static float Lerp( float a, float b, float t )
    {
        //a + (b - a) * t;
        return (1f - t) * a + t * b;
    }
    
    Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
    }

    public static float SmoothStep(float from, float to, float t)
    {
        //hermite
        //Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));
        t = -2.0f * t * t *  t + 3.0f *  t * t;
        return to *  t + from * (1.0f - t);
    }
    
    public static float Repeat(float value, float length)
    {
        return Mathf.Clamp(value - Mathf.Floor(value / length) * length, 0.0f, length);
    }

    public static float PingPong(float t, float length )
    {
        t = Repeat(t, length*2f);

        return length - Mathf.Abs(t - length);
    }

    public static float Eerp( float a, float b, float t )
    {
        return Mathf.Pow(a, 1 - t) * Mathf.Pow(b, t);
    }
    
    public static float Berp(float start, float end, float t)
    {
        t = Mathf.Clamp01(t);
        t = (Mathf.Sin(t * Mathf.PI * (0.2f + 2.5f * t * t * t)) * Mathf.Pow(1f - t, 2.2f) + t) * (1f + (1.2f * (1f - t)));
        return start + (end - start) * t;
    }
    
    public static float Bounce(float t)
    {
        t = Mathf.Clamp01(t);
        return Mathf.Abs(Mathf.Sin(6.28f * (t + 1f) * (t + 1f)) * (1f - t));
    }
    
    public static float Coserp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
    }
    
    public static float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }

    public float Sigmoid(float t)
    {
        const float e = 2.718281828459f;
        return  1.0f/(1.0f+Mathf.Pow(e,-t)); //Mathf.Exp(-t)
    }
    
    public static float InverseLerp( float a, float b, float value )
    {
        return (value - a) / (b - a);
    }
    
    public static float Remap( float iMin, float iMax, float oMin, float oMax, float value ) 
    {
        float t = InverseLerp( iMin, iMax, value );
        return Lerp( oMin, oMax, t );
    }
    
    public static float MoveTowards( float current, float target, float maxDelta ) 
    {
        if( Mathf.Abs( target - current ) <= maxDelta )
            return target;
        
        return current + Mathf.Sign( target - current ) * maxDelta;
    }
    
    // public static Vector3 MoveTowards(
    //     Vector3 current,
    //     Vector3 target,
    //     float maxDistanceDelta)
    // {
    //     float num1 = target.x - current.x;
    //     float num2 = target.y - current.y;
    //     float num3 = target.z - current.z;
    //     float num4 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
    //     if ((double) num4 == 0.0 || (double) maxDistanceDelta >= 0.0 && (double) num4 <= (double) maxDistanceDelta * (double) maxDistanceDelta)
    //         return target;
    //     float num5 = (float) Math.Sqrt((double) num4);
    //     return new Vector3(current.x + num1 / num5 * maxDistanceDelta, current.y + num2 / num5 * maxDistanceDelta, current.z + num3 / num5 * maxDistanceDelta);
    // }
}
