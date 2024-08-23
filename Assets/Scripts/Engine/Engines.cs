using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class responsible to calculate and update all engines params
/// </summary>
public class Engines : MonoBehaviour
{
    /// <summary>
    /// params that responsible to control efficiency of Main Thrust depends on percent of HP
    /// </summary>
    public float VelocityEfficiency {  get; private set; }
    /// <summary>
    /// params that responsible to control efficiency of Minor Engine Thrust depends on percent of HP
    /// </summary>
    public float ManeuverEfficiency {  get; private set; }
    /// <summary>
    /// params that responsible to understand max torque to maneuvering if ship don't accelerate
    /// </summary>
    public Vector3 PureManeuverability {  get; private set; } // Max torque if ship don't accelerate
    /// <summary>
    /// params that responsible to understand max torque to maneuvering if ship don't accelerate
    /// </summary>
    public Vector3 PartialManeuverability { get; private set; } // Max torque if ship accelerate
    /// <summary>
    /// Maximum HitPoint of all Engines
    /// </summary>
    public float FullHP {  get; private set; }
    /// <summary>
    /// Current HitPoint of all Engines
    /// </summary>
    public float CurrHp { get; private set; }
    /// <summary>
    /// Maximum Thrust for accelerate ship
    /// </summary>
    public float MaxThrust {  get => _maxSustainEngineSum; }
    [SerializeField] Engine[] engines;
    private Dictionary<ShipSide, Dictionary<ModuleSide, Engine>> _engineList = new();
    private float _sustainEngineSum = 0f;
    private float _maxSustainEngineSum = 1f;
    private int _sustainEngineCount = 0;




    private void Start()
    {
        Tuple<ShipSide, ModuleSide> sides;
        foreach (var engine in engines)
        {
            engine.TakenDamage += RefreshData;
            sides = engine.GetSides();
            if (sides.Item1 == ShipSide.Back)
            {
                _sustainEngineCount++;
            }
            if (!_engineList.ContainsKey(sides.Item1))
            {
                _engineList.Add(sides.Item1, new Dictionary<ModuleSide, Engine>());
                _engineList[sides.Item1].Add(sides.Item2, engine);
            }
            else
            {
                if (!_engineList[sides.Item1].ContainsKey(sides.Item2))
                {
                    _engineList[sides.Item1].Add(sides.Item2, engine);
                }
            }
        }
        //engines = null;
        VelocityEfficiencyRefresh();
        CalculatePartialManeuverability();
        CalculatePureManeuverability();
        _maxSustainEngineSum = _sustainEngineSum;

        //Debugs();
    }

    private void OnDestroy()
    {
        foreach (var engine in engines)
        {
            engine.TakenDamage -= RefreshData;
        }
    }
    private void VelocityEfficiencyRefresh()
    {
        _sustainEngineSum = 0f;
        foreach (var keyEngine in _engineList[ShipSide.Back])
        {
            _sustainEngineSum += keyEngine.Value.Thrust * keyEngine.Value.ThrustVector.z;
        }
        VelocityEfficiency = _maxSustainEngineSum * Mathf.Pow( _sustainEngineSum / 
            _maxSustainEngineSum,Settings.EngineDecreasingPowNumber);
    }

    private void ManeuverEfficiencyRefresh()
    {
        ManeuverEfficiency = FullHP * Mathf.Pow(CurrHp/FullHP, Settings.EngineDecreasingPowNumber);
    }

    private void RefreshData()
    {
        RefreshHp();
        Debug.Log("refresh");
        if (CurrHp != FullHP)
        {
            ManeuverEfficiencyRefresh();
            VelocityEfficiencyRefresh();
        }
    }

    private void RefreshHp()
    {
        float hp = 0;
        foreach (var engine in engines)
        {
            hp += engine.HP;
        }
        CurrHp = hp;
    }

    private void CalculatePureManeuverability()
    {
        var PureManeuverabilityTemp = Vector3.zero;
        foreach (var engine in engines)
        {
            if (engine.RotationVector.x * PureManeuverabilityTemp.x >= 0)
                PureManeuverabilityTemp.x += engine.RotationVector.x;
            if (engine.RotationVector.y * PureManeuverabilityTemp.y >= 0)
                PureManeuverabilityTemp.y += engine.RotationVector.y;
            if (engine.RotationVector.z * PureManeuverabilityTemp.z >= 0)
                PureManeuverabilityTemp.z += engine.RotationVector.z;
        }
        PureManeuverabilityTemp.x = Mathf.Abs(PureManeuverabilityTemp.x);
        PureManeuverabilityTemp.y = Mathf.Abs(PureManeuverabilityTemp.y);
        PureManeuverabilityTemp.z = Mathf.Abs(PureManeuverabilityTemp.z);
        PureManeuverability = PureManeuverabilityTemp;
    }

    private void CalculatePartialManeuverability()
    {
        var PartialManeuverabilityTemp = Vector3.zero;
        foreach (var engine in engines)
        {
            if (engine.GetSides().Item1 != ShipSide.Back)
            {
                if (engine.RotationVector.x * PartialManeuverabilityTemp.x >= 0)
                    PartialManeuverabilityTemp.x += engine.RotationVector.x;
                if (engine.RotationVector.y * PartialManeuverabilityTemp.y >= 0)
                    PartialManeuverabilityTemp.y += engine.RotationVector.y;
                if (engine.RotationVector.z * PartialManeuverabilityTemp.z >= 0)
                    PartialManeuverabilityTemp.z += engine.RotationVector.z;
            }

        }
        PartialManeuverabilityTemp.x = Mathf.Abs(PartialManeuverabilityTemp.x);
        PartialManeuverabilityTemp.y = Mathf.Abs(PartialManeuverabilityTemp.y);
        PartialManeuverabilityTemp.z = Mathf.Abs(PartialManeuverabilityTemp.z);
        PartialManeuverability = PartialManeuverabilityTemp;
    }


    //private void Debugs() // Debug Engines
    //{
    //    string s = "";
    //    foreach (var item in _engineList)
    //    {
    //        s += "\n" + item.Key.ToString() + "\n";
    //        foreach (var item1 in _engineList[item.Key])
    //        {
    //            s += item1.Key.ToString() + item1.Value.ToString() + "   " + item1.Value.RotationVector +"\n";
    //        }
    //    }
    //    Debug.Log(s);
    //    Debug.Log(PureManeuverability);
    //    Debug.Log(PartialManeuverability);
    //}
}
