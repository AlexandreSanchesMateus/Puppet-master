using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leg : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _impulseForce;
    [SerializeField] private float _aroundRange;

    public void AddImpulse(Vector3 direction, bool reverse)
    {
        float angle = Vector2.SignedAngle(new Vector2(direction.x, direction.z), Vector2.up);
        float random = Random.Range(angle - _aroundRange, angle + _aroundRange);
        Vector2 result = new Vector2(Mathf.Cos(random), Mathf.Sin(random));

        if (reverse)
            _rb.AddForceAtPosition(result * _impulseForce, transform.position, ForceMode.Impulse);
        else    
            _rb.AddForceAtPosition(result * _impulseForce, transform.position, ForceMode.Impulse);
    }
}
