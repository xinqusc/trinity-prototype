using UnityEngine;

public abstract class PassiveItem : MonoBehaviour
{
}

public abstract class ActiveItem : MonoBehaviour
{
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract bool IsUsable();
    public abstract float GetChargeProgress();
    public abstract Sprite GetUISprite();
    public abstract void ResetCharge();
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

public interface IOnDeathEffect
{
    void OnDeath();
}

public interface IDisposable { }

public class Damage
{
    private GameObject source;
    private GameObject medium;
    private IDamageable target;
    private float value;
    private Vector3 diretcion;

    public Damage(GameObject source, GameObject medium, IDamageable target, float value, Vector3 diretcion)
    {
        this.source = source;
        this.medium = medium;
        this.target = target;
        this.value = value;
        this.diretcion = diretcion.normalized;
    }

    public GameObject GetSource() { return source; }


    public GameObject GetMedium() { return medium; }

    public IDamageable GetTarget() { return target; }

    public float GetValue() { return value; }

    public Vector3 GetDiretcion() { return diretcion; }

    public void Apply()
    {
        target.ReceiveDamage(this);
    }

    public static void Apply(Damage damage) { damage.Apply(); }
}