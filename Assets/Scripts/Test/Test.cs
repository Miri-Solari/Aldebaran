using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] ShipMovement oleg;
    private Vector3 olegVector;
    private LineRenderer _lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        olegVector = oleg.MoveDirection;
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.SetPosition(0, new Vector3(3.3f, 1.4f, 0.24f));
        _lineRenderer.SetPosition(1, olegVector*100);
    }

}
