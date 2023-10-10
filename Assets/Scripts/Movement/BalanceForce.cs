using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BalanceForce : MonoBehaviour
{
    [SerializeField, Required, BoxGroup("Dependencie")] private Rigidbody _rb;

    [SerializeField, BoxGroup("Parameters")] private float _strandingForce;
    [SerializeField, BoxGroup("Parameters")] private Transform _forcePoint;
    
    private Vector2 _forceDirection = Vector2.zero;

    public bool EnableBalace { get; private set; } = true;

    private void FixedUpdate()
    {
        if (!EnableBalace) return;

        _rb.AddForceAtPosition(Vector3.up * _strandingForce + new Vector3(_forceDirection.x, 0, _forceDirection.y), _forcePoint.position, ForceMode.Force);
    }

    public void SetDirection(Vector2 direction) => _forceDirection = direction;

    public void EnableBalance(bool enable) => EnableBalace = enable;
}
