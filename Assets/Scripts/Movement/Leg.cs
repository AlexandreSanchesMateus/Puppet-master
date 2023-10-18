using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leg : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _impulseForce;

    public void AddImpulse(Vector3 direction, bool reverse)
    {
        if (reverse)
            _rb.AddForceAtPosition(-new Vector3(direction.x, 0, direction.z) * _impulseForce, transform.position, ForceMode.Impulse);
        else    
            _rb.AddForceAtPosition(new Vector3(direction.x, 0, direction.z) * _impulseForce, transform.position, ForceMode.Impulse);
    }
}
