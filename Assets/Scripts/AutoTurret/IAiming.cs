using UnityEngine;


internal interface IAiming
{
    public void Aim(GameObject target, Transform aimPoint)
    {
        aimPoint.LookAt(target.transform,new Vector3());
    }
}

