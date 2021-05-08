using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float delay;
    public Missile missile;
    public Transform target;
    [Range(-1, 1)] 
    public float dotThreshold;

    private float _time;

    private void Awake()
    {
        _time = delay;
    }

    private void Update()
    {
        _time -= Time.deltaTime;
        float dot = Vector3.Dot((target.position - transform.position).normalized, transform.forward);
        Debug.Log(dot);

        if (_time <= 0 && dot >= dotThreshold)
        {
            _time = delay;
            Missile instance = Instantiate(missile, transform.position, transform.rotation);
            instance.target = target;
        }
    }

    private void OnDrawGizmos()
    {
        float dot = Vector3.Dot((target.position - transform.position).normalized, transform.forward);
        if (dot < dotThreshold)
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(transform.position, target.position);
    }
}
