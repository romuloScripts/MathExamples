using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt2d : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_camera != null)
        {
            //Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            
           // Plane plane = new Plane(-transform.forward,transform.position);

            //if (plane.Raycast(ray, out float distance))
            //{
                Vector3 diff = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
                //Vector3 diff = Input.mousePosition - ray.GetPoint(distance) - transform.position;
                diff.Normalize();
                float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Quaternion.Euler(0f, 0f, angle);
            //}
        }
    }
}
