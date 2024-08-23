using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour // поиграть с множителями 
{
    public Vector3 MoveDirection;
    public ShipState ShipState = ShipState.ChangeTheCourse; 
    [Range(1, 100)] public float ThrustPower;
    [SerializeField] Vector3 frontDirection;
    [SerializeField] Engines engines;
    [SerializeField] Rigidbody hull;
    [SerializeField, Range(0f, 89.9f)] float rotationChangeAngle = 0.76f;
    private float _maxRotationSpeedPure;
    private float _minRotationSpeedPure;
    private bool _startRotation = true;
    private bool _rotationZ = true;
    private Quaternion _targetRotationZ;




    public void Init()
    {
        ChangeState();
    }
    private void FixedUpdate()
    {
        switch (ShipState)
        {
            case ShipState.AllAhead:
                AllAhead(); break;
            case ShipState.EvasiveManeuver:
                EvasiveManeuver(); break;
            case ShipState.ManeuverFight:
                ManeuverFight(); break;
            case ShipState.ChangeTheCourse:
                ChangeTheCourse(); break;
            case ShipState.Stay:
                Stay(); break;
            case ShipState.ChangeTheCourseAtFullAhead:
                ChangeTheCourseAtFullAhead(); break;
        }
    }

    private void AllAhead()
    {
        hull.AddRelativeForce(ThrustPower * engines.MaxThrust * frontDirection *
            engines.VelocityEfficiency * Settings.SpeedAndVelocityReduction);
    }

    private void ChangeTheCourse()
    {
        Quaternion targetRotation = Quaternion.LookRotation(MoveDirection * -1);

        if (_maxRotationSpeedPure == engines.PureManeuverability.x)
        {
            if (_startRotation)
            {
                _targetRotationZ = Quaternion.FromToRotation(Vector3.ProjectOnPlane(hull.transform.up,
                    Vector3.forward), Vector3.ProjectOnPlane(MoveDirection, Vector3.forward));
                CalculateZModifier();
                _startRotation = false;
            }
            if ((Mathf.Abs((_targetRotationZ.eulerAngles - hull.rotation.eulerAngles).z) > 90 - rotationChangeAngle + 2f) & _rotationZ)
            {
                var rotationZ = Quaternion.Lerp(hull.transform.rotation, _targetRotationZ,
                    Time.deltaTime * engines.PureManeuverability.z * Settings.TorqueToAnglePerSecond * ThrustPower / 4);
                targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, _targetRotationZ.eulerAngles.z);
                Quaternion finalRotationZ = hull.rotation;
                finalRotationZ.eulerAngles = new Vector3(finalRotationZ.eulerAngles.x, finalRotationZ.eulerAngles.y, rotationZ.eulerAngles.z);
                hull.transform.rotation = finalRotationZ;
            }
            else
            {
                _minRotationSpeedPure = _maxRotationSpeedPure;
                targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, hull.transform.rotation.eulerAngles.z);
                _rotationZ = false;
            }
        }
        else if (_maxRotationSpeedPure == engines.PureManeuverability.y)
        {
            if (_startRotation)
            {
                _targetRotationZ = Quaternion.FromToRotation(Vector3.ProjectOnPlane(hull.transform.right,
                    Vector3.forward), Vector3.ProjectOnPlane(MoveDirection, Vector3.forward));
                CalculateZModifier();
                _startRotation = false;
            }
            if ((Mathf.Abs((_targetRotationZ.eulerAngles - hull.rotation.eulerAngles).z) > 90 - rotationChangeAngle + 2f) & _rotationZ)
            {
                var rotationZ = Quaternion.Lerp(hull.transform.rotation, _targetRotationZ,
                    Time.deltaTime * engines.PureManeuverability.z * Settings.TorqueToAnglePerSecond * ThrustPower / 4);
                targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, _targetRotationZ.eulerAngles.z);
                Quaternion finalRotationZ = hull.rotation;
                finalRotationZ.eulerAngles = new Vector3(finalRotationZ.eulerAngles.x, finalRotationZ.eulerAngles.y, rotationZ.eulerAngles.z);
                hull.transform.rotation = finalRotationZ;
            }
            else
            {
                _minRotationSpeedPure = _maxRotationSpeedPure;
                targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, hull.transform.rotation.eulerAngles.z);
                _rotationZ = false;
            }
        }
        var rotation = Quaternion.Lerp(hull.transform.rotation, targetRotation,
            Time.deltaTime * _minRotationSpeedPure * Settings.TorqueToAnglePerSecond * ThrustPower / 100);
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, hull.transform.rotation.eulerAngles.z);
        hull.transform.rotation = rotation;

        if (Vector3.Angle(hull.transform.forward * -1, MoveDirection) < 90 - rotationChangeAngle) // убрать -1 и потом сделать вычисление по вектору форварда
        {
            hull.transform.rotation = targetRotation;
            ShipState = ShipState.Stay;
            _startRotation = true;
            _rotationZ = true;
            ChangeState();
        }
    }

    private void EvasiveManeuver()
    {
        
    }

    private void ManeuverFight()
    {
        
    }

    private void ChangeTheCourseAtFullAhead()
    {
        
    }
    private void Stay()
    {
        ShipState = ShipState.AllAhead;
        
    }

    private void ChangeState()
    {
        _targetRotationZ = Quaternion.FromToRotation(Vector3.ProjectOnPlane(hull.transform.up,
                    Vector3.forward), Vector3.ProjectOnPlane(MoveDirection, Vector3.forward));
        _maxRotationSpeedPure = Mathf.Max(engines.PureManeuverability.x, engines.PureManeuverability.y);
        _minRotationSpeedPure = Mathf.Min(engines.PureManeuverability.x, engines.PureManeuverability.y);
    }

    private void CalculateZModifier()
    {
        float modifierRotationZ = _targetRotationZ.eulerAngles.z;
        switch (_targetRotationZ.eulerAngles.z) 
        {
            case >= 270:
                modifierRotationZ *= 1;
                break;
            case >= 180:
                modifierRotationZ -= 180;
                break;
            case >= 90:
                modifierRotationZ += 180;
                break;
            case >= 0:
                modifierRotationZ *= 1;
                break;
        }
        Debug.Log(_targetRotationZ.eulerAngles.ToString() + modifierRotationZ.ToString());
        _targetRotationZ.eulerAngles = new Vector3(_targetRotationZ.eulerAngles.x, _targetRotationZ.eulerAngles.y, modifierRotationZ);
        Debug.Log(_targetRotationZ.eulerAngles);
    }
}
