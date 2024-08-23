using System.Collections;
using System;
using UnityEngine;

public class Engine : MonoBehaviour, IDamageable
{
    public float HP { get => hull; }
    public float Thrust { get => thrust; }
    public int PercentHP { get; private set; }
    public Vector3 ThrustVector { get; private set; }
    public Vector3 RotationVector { get; private set; }
    public event Action TakenDamage;
    [SerializeField] private float hull = 10;
    [SerializeField] float thrust = 10;
    [SerializeField] ShipSide shipSide; // side of ship
    [SerializeField] ModuleSide moduleSide; // side of module
    [SerializeField] Transform ship;
    private float _fullThrust;
    private float _fullHull;

    public void TakeDamage(float damage)
    {
        hull -= damage;
        TakenDamage();
    }

    internal Tuple<ShipSide, ModuleSide> GetSides()
    {
        return new Tuple<ShipSide, ModuleSide>(shipSide, moduleSide);
    }

    private void Start()
    {
        ThrustVectorQualifier();
        _fullHull = hull;
        _fullThrust = thrust;
        TakenDamage += RefreshThrust;
    }

    private void OnDestroy()
    {
        TakenDamage -= RefreshThrust;
    }

    private void RefreshThrust()
    {
        PercentHP = (int)((hull / _fullHull) * 100);
        thrust = _fullThrust * Mathf.Pow(hull/_fullHull, Settings.EngineDecreasingPowNumber); // exponent decrease
    }

    private void ThrustVectorQualifier()
    {
        var massCenterAngle = (gameObject.transform.position - ship.position).normalized;
        var thrustToRotation = new Vector3(0,0,0);
        ThrustVector = transform.up;
        thrustToRotation.x = ThrustVector.z * massCenterAngle.y - ThrustVector.y * massCenterAngle.z;
        thrustToRotation.y = ThrustVector.x * massCenterAngle.z - ThrustVector.z * massCenterAngle.x;
        thrustToRotation.z = ThrustVector.x * massCenterAngle.y - ThrustVector.y * massCenterAngle.x;
        RotationVector = thrustToRotation.normalized;
        ThrustVector = ThrustVector.normalized;    
    }
}
