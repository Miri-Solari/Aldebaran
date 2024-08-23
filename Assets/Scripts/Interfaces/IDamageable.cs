
using System;
using UnityEditor;

internal interface IDamageable
{
    public float HP { get; }
    public event Action TakenDamage;

    public void TakeDamage(float damage);
}

