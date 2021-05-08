using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform target;
    public Rigidbody rigidbody;
    public float speed;
    public float rotateSpeed;
    
    void FixedUpdate()
    {
        Vector3 direction = (transform.position - target.position).normalized;
        var dir = Vector3.Cross(direction, transform.forward);
        
        Debug.DrawLine(transform.position, transform.position + dir);
        
        rigidbody.angularVelocity += dir * (rotateSpeed * Time.deltaTime);

        rigidbody.velocity = transform.forward * (speed * Time.deltaTime);
    }
}
