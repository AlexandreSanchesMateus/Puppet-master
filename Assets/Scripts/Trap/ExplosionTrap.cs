using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Game;

public class EnclumeTrap : Trap 
{
    [SerializeField, BoxGroup("Settings")] private float _explosionDelay;
    [SerializeField, BoxGroup("Settings")] private float _explosionForce;
    [SerializeField, BoxGroup("Settings")] private LayerMask _layer;
    [SerializeField, BoxGroup("Settings")] private float _activeRadius;
    [SerializeField, BoxGroup("Settings")] private float _explosionRadius;

    [SerializeField, Foldout("Events")] private UnityEvent _onTrapIgnition;
    private bool _isIgnited;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _explosionRadius);

        Gizmos.color = new Color(0, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, _activeRadius);
    }

    /*private void Update()
    {
        if((m_playerReference.Instance.transform.position - transform.position).magnitude <= _activeRadius&& !_isIgnited)
        {
            _onTrapIgnition?.Invoke();
            Invoke(nameof(Activate), _explosionDelay);
            _isIgnited = true;
        }
    }*/

    public override void Activate()
    {
        RaycastHit[] colliders = Physics.SphereCastAll(transform.position, _explosionRadius, Vector3.up, Mathf.Infinity, _layer);
        foreach (RaycastHit info in colliders)
        {
            // Applie damage
            if (info.transform.TryGetComponent<IDamageable>(out IDamageable healthComponent))
            {
                healthComponent.TakeDamage(m_scoreGain);
            }

            // Add force
            if (info.transform.TryGetComponent<Rigidbody>(out Rigidbody rigidbodyComponent))
            {
                Vector3 direction = (rigidbodyComponent.transform.position - transform.position).normalized;
                rigidbodyComponent.AddForce(direction * _explosionForce, ForceMode.Impulse);
            }
        }

        base.Activate();
    }
}
