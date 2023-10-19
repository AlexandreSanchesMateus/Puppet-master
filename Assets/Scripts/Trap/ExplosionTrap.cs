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
    [SerializeField, BoxGroup("Settings")] private PuppetPhysic _physic;
    [SerializeField, BoxGroup("Settings")] private Physics3DInteraction _interaction;
    [SerializeField, BoxGroup("Settings")] private float _activeRadius;

    [SerializeField, Foldout("Events")] private UnityEvent _onTrapIgnition;
    
    HashSet<Collider> _collidersIn = new HashSet<Collider>();

    private void OnDrawGizmosSelected()
    {
        if (_interaction != null)
        {
            Gizmos.color = new Color(0, 0, 0, 0.2f);
            Gizmos.DrawSphere(_interaction.transform.position, _activeRadius);
        }
    }

    protected override void Start()
    {
        m_isTrapActive = true;

        _interaction.TriggerEnter3D += AddObject;
        _interaction.TriggerExit3D += RemoveObject;
    }

    private void Update()
    {
        if (!m_isTrapActive) return;

        foreach(Collider info in _collidersIn)
        {
            if((info.transform.position - _interaction.transform.position).magnitude <= _activeRadius)
            {
                Invoke(nameof(Activate), _explosionDelay);
                _onTrapIgnition?.Invoke();
                m_isTrapActive = false;
            }
        }
    }

    private void OnDestroy()
    {
        _interaction.TriggerEnter3D -= AddObject;
        _interaction.TriggerExit3D -= RemoveObject;
    }

    private void AddObject(Collider other)
    {
        if (!_collidersIn.Contains(other))
        {
            _collidersIn.Add(other);
        }
    }

    private void RemoveObject(Collider other)
    {
        if (_collidersIn.Contains(other))
        {
            _collidersIn.Remove(other);
        }
    }

    public override void Activate()
    {
        InflictFullDamageToPlayer();
        _physic.SetPuppetPhysicToDisable();

        // Add force
        foreach (Collider info in _collidersIn)
        {
            if (info.transform.TryGetComponent<Rigidbody>(out Rigidbody rigidbodyComponent))
            {
                Vector3 direction = (rigidbodyComponent.transform.position - transform.position).normalized;
                rigidbodyComponent.AddForce(direction * _explosionForce, ForceMode.Impulse);
            }
        }

        base.Activate();
    }
}
