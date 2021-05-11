using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrizPositions : MonoBehaviour
{
    public Transform[] targets;

    private Vector3[] iniPositions;
    
    private void Start()
    {
        iniPositions = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            iniPositions[i] = transform.InverseTransformPoint(targets[i].position);
        }
    }

    private void Update()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 iniPos = iniPositions[i];
            Vector4 pos = new Vector4(iniPos.x,iniPos.y,iniPos.z,1f);
            targets[i].position = transform.localToWorldMatrix * pos;
        }
    }
}
