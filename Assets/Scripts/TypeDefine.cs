using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveItem : MonoBehaviour
{
}

public abstract class ActiveItem : MonoBehaviour
{
    public abstract void Activate();
}

public interface IProjectileModifier
{
    public void Modify(GameObject projectile);
}

public interface IDamageable
{
    bool GetHostility();

    void ReceiveDamage(Damage damage);
}

public class Damage
{
    private GameObject source;
    private GameObject medium;
    private IDamageable target;
    private float value;

    public GameObject GetSource() { return source; }

    public void SetSource(GameObject value) { source = value; }

    public GameObject GetMedium() { return medium; }

    public void SetMedium(GameObject value) { medium = value; }

    public IDamageable GetTarget() { return target; }

    public void SetTarget(IDamageable value) { target = value; }

    public float GetValue() { return value; }

    public void SetValue(float value) { this.value = value; }

    public void Apply() { target.ReceiveDamage(this); }

    public static void Apply(Damage damage) { damage.Apply(); }
}