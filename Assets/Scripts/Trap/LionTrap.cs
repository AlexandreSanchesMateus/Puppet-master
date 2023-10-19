using Cinemachine;
using DG.Tweening;
using Game;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LionTrap : Trap
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private PuppetPhysic _physic;
    [SerializeField] private float _impulseForce = 1;
    [SerializeField] private Vector3 _impulseDirection;

    private HashSet<Rigidbody> _collidersIn = new HashSet<Rigidbody>();
    private IMovable movable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb) && !_collidersIn.Contains(rb))
        {
            _collidersIn.Add(rb);
        }

        if (m_isTrapActive) return;
        
        if(other.transform.TryGetComponent<IMovable>(out movable))
        {
            Activate();
            m_isTrapActive = true;
        }
    }

    [Button]
    public override void Activate()
    {
        movable.MovePlayerTo(transform.position, 400, true);

        _camera.gameObject.SetActive(true);
        m_onTrapActivate?.Invoke();

        InflictFullDamageToPlayer();
        StartCoroutine(launchPlayer());

        IEnumerator launchPlayer()
        {
            yield return new WaitForSeconds(5f);

            _camera.gameObject.SetActive(false);
            foreach(Rigidbody rb in _collidersIn)
            {
                rb.AddForce(_impulseDirection.normalized * _impulseForce, ForceMode.Impulse);
            }

            _physic.SetPuppetPhysicToDisable();

            if (movable != null)
                movable.EnableInput(true);

            yield return new WaitForSeconds(1f);
            _collidersIn.Clear();
            m_isTrapActive = false;
        }
    }

    [Button]
    private void CheckImpulseDirection()
    {
        Debug.DrawRay(transform.position, _impulseDirection.normalized * 5, Color.red, 10);
    }
}
