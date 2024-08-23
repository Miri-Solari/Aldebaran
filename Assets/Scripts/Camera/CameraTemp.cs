using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTemp : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform center;
    private Vector3 shift;

    private void Start()
    {
        shift = cam.transform.position - center.position;
    }
    // Update is called once per frame
    void Update()
    {
        cam.transform.position = shift + center.position;
        cam.transform.LookAt(center);
    }
}
