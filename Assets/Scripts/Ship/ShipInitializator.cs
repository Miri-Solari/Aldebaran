using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInitializator : MonoBehaviour
{
    [SerializeField] ShipMovement movement;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initialization());
    }

    IEnumerator Initialization()
    {
        yield return new WaitForSeconds(0.1f);
        movement.Init();
    }
}
