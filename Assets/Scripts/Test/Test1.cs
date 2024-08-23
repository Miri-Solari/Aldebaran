using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    [SerializeField] Engine engine;
    [SerializeField] bool IsGiveDMG;
    [SerializeField] Transform centre;

    private void Start()
    {
        transform.RotateAround(centre.position ,Vector3.forward, 90);
    }
    // Update is called once per frame
    //void Update()
    //{
    //    if (IsGiveDMG)
    //    {
    //        engine.TakeDamage(1);
    //    }
    //}
}
