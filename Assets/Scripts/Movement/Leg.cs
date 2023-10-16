using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leg : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _impulseForce;
    [SerializeField, Dropdown("GetVectorValues")] private Vector3 _direction;

    private DropdownList<Vector3> GetVectorValues()
    {
        return new DropdownList<Vector3>()
        {
            { "Right",   Vector3.right },
            { "Left",    Vector3.left },
            { "Up",      Vector3.up },
            { "Down",    Vector3.down },
            { "Forward", Vector3.forward },
            { "Back",    Vector3.back }
        };
    }

    public void AddImpulseForward()
    {
        _rb.AddForceAtPosition(_direction * _impulseForce, transform.position, ForceMode.Impulse);
    }
}
