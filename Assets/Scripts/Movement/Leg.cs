using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Leg : MonoBehaviour
{
    [SerializeField, Required] private Rigidbody _rb;

    [SerializeField, BoxGroup("Suspension")] private float _restLength;
    [SerializeField, BoxGroup("Suspension")] private float _springStiffness;
    [SerializeField, BoxGroup("Suspension")] private float _damperStiffness;
    [SerializeField, BoxGroup("Suspension")] private float _legLength;
    [SerializeField, BoxGroup("Suspension")] private float _minLength;

    private float _lastLenght;
    private float _springLength;

    private bool _isGrounded;

    private void OnDrawGizmosSelected()
    {
        // Max extention leg
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, -transform.up);

        // Rest lenght
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(-transform.up, 0.04f);
    }

    private void FixedUpdate()
    {
        RaycastHit info;
        if (Physics.Raycast(gameObject.transform.position, -transform.up, out info, _legLength))
        {
            _lastLenght = _springLength;
            _springLength = Mathf.Clamp(info.distance, _minLength, _legLength);

            float springVelocity = (_lastLenght - _springLength) / Time.fixedDeltaTime;
            float springForce = _springStiffness * (_springLength - _restLength);
            float damperForce = _damperStiffness * springVelocity;

            Vector3 suspensionForce = (springForce + damperForce) * -transform.up;
            _rb.AddForceAtPosition(suspensionForce, info.point);
            _isGrounded = true;

            Debug.DrawLine(gameObject.transform.position, info.point, Color.green);
        }
    }
}
