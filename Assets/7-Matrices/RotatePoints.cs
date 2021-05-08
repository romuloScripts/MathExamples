using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePoints : MonoBehaviour
{
    public float followPercentage =1;
    public Transform[] transforms;

    private Vector3[] _localPos;

    private void Start()
    {
        _localPos = new Vector3[transforms.Length];
        for (var i = 0; i < transforms.Length; i++)
        {
            _localPos[i] = transform.InverseTransformPoint(transforms[i].position);
        }
    }

    private void Update()
    {
        for (var i = 0; i < transforms.Length; i++)
        {
            Vector3 pos = _localPos[i];
            
            var item = transforms[i];
            item.transform.position = transform.localToWorldMatrix * new Vector4(pos.x, pos.y, pos.z, followPercentage);
        }
    }
}
