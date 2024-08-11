using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public bool IsFiring = false;
    [SerializeField] float reloadTime = 0.5f;
    [SerializeField, Range(0,1)] float fireTime = 0.1f;
    [SerializeField, Range(0.0001f, 0.25f)] float AimingDelay;
    [SerializeField] TurretAimer turretAimer;
    [SerializeField] FiringLaser firingLaser;
    private WaitForSeconds _reloadTime;
    private WaitForSeconds _fireTime;
    private WaitForSeconds _aimingDelay;

    private void Start()
    {
        _reloadTime = new WaitForSeconds(reloadTime);
        _fireTime = new WaitForSeconds(fireTime);
        _aimingDelay = new WaitForSeconds(AimingDelay);
    }

    private void Update()
    {
        if (!IsFiring)
        {
            OpenFire();
        }
    }

    void OpenFire()
    {
        if (turretAimer.IsTurretAim() != null)
        {
            StartCoroutine(Shoot());
        }
    }
    IEnumerator Shoot()
    {
        yield return AimingDelay;
        IsFiring = true;
        firingLaser.OpenFire(turretAimer.IsTurretAim());
        yield return _fireTime;
        firingLaser.StopFire();
        yield return _reloadTime;
        IsFiring = false;
        yield break;
    }

}
