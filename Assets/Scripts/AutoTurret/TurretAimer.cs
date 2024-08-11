using UnityEngine;

public class TurretAimer : MonoBehaviour, IAiming
{
    [SerializeField] TurretTriger turretTriger;
    private GameObject _target;

    private void OnEnable()
    {
        Subscription();
    }

    private void OnDisable()
    {
        UnSubscription();
    }

    private void Update()
    {
        if (_target != null)
        ((IAiming)this).Aim(_target, gameObject.transform);
    }

    public Transform IsTurretAim()
    {
        return _target?.transform;
    }

    void Subscription()
    {
        turretTriger.EnemyDetect += GetTarget;
    }

    void UnSubscription()
    {
        turretTriger.EnemyDetect -= GetTarget;
    }

    void GetTarget(GameObject target)
    {
        _target = target;
    }
}
