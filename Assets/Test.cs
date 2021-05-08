using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Debug.Log(Quaternion.identity * transform.forward + " " + transform.forward);
    }
}
