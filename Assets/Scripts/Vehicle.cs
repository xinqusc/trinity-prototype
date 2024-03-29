using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class Vehicle : MonoBehaviour
{
    // Start is called before the first frame update

    private static string prefabPath = "Prefabs/Vehicle";
    [SerializeField] private float speed;
    [SerializeField] private float contactDamage;
    [SerializeField] private bool hostility;

    public float GetSpeed() { return speed; }

    public void SetSpeed(float value) { speed = value; }

    public float GetContactDamage() { return contactDamage; }

    public void SetContactDamage(float value) { contactDamage = value; }

    public bool GetHostility() { return hostility; }

    public void SetHostility(bool value) { hostility = value; }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }

    public static IEnumerator Instantiate(Vector3 startPos, Vector3 targetPos, float traceDuration, float delay, float speed, float contactDamage, bool hostility)
    {
        Vector3 bias = Quaternion.Euler(0, 0, 90) * (targetPos - startPos).normalized * Resources.Load<GameObject>(prefabPath).transform.lossyScale.x / 2;
        Destroy(DrawLine("VehicleTrace", startPos + bias, targetPos + bias, Color.red), traceDuration);
        Destroy(DrawLine("VehicleTrace", startPos - bias, targetPos - bias, Color.red), traceDuration);
        yield return new WaitForSeconds(delay);
        GameObject vehicle = Instantiate(Resources.Load<GameObject>(prefabPath), startPos, Quaternion.LookRotation(Vector3.forward, targetPos - startPos));
        vehicle.tag = "Disposable";
        Vehicle script = vehicle.GetComponent<Vehicle>();
        script.SetSpeed(speed);
        script.SetContactDamage(contactDamage);
        script.SetHostility(hostility);
        vehicle.GetComponent<DestroyOutOfBounds>().SetOffset(-1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null && damageable.GetHostility() != hostility)
        {
            new Damage(gameObject, null, damageable, contactDamage, collision.transform.position - transform.position).Apply();
            GameplayManager.GetGoogleSender().SendMatrix2(speed);
        }
    }
}
