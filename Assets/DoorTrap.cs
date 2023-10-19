using DG.Tweening;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class DoorTrap : Trap
{
    [SerializeField, Foldout("Events")] private UnityEvent OnDoorClose;

    [SerializeField] private PuppetPhysic _physic;
    [SerializeField] private Transform pivot1, pivot2;
    [SerializeField] private float _impulseForce = 20;
    [SerializeField] private Vector3 _impulseDirection;

    private HashSet<Rigidbody> _collidersIn = new HashSet<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Rigidbody>(out Rigidbody rb) && !_collidersIn.Contains(rb))
        {
            _collidersIn.Add(rb);

            if (!m_isTrapActive)
            {
                StartCoroutine(TrapTrigger());
                m_isTrapActive = true;
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
        OnDoorClose.Invoke();
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(pivot1.DOLocalRotate(new Vector3(0, 0, 0), 0.20f, RotateMode.Fast));
        mySequence.Join(pivot2.DOLocalRotate(new Vector3(0, 0, 0), 0.20f, RotateMode.Fast).OnComplete(() => InflictFullDamageToPlayer()));
    }

    private IEnumerator TrapTrigger()
    {
        yield return new WaitForSeconds(0.2f);
        Activate();

        yield return null;
        foreach (Rigidbody rb in _collidersIn)
        {
            rb.AddForce(_impulseDirection.normalized * _impulseForce, ForceMode.Impulse);
        }
        _physic.SetPuppetPhysicToDisable();

        yield return new WaitForSeconds(1f);
        _collidersIn.Clear();
        m_isTrapActive = false;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(pivot1.DOLocalRotate(new Vector3(0, -130, 0), 0.20f, RotateMode.Fast));
        mySequence.Join(pivot2.DOLocalRotate(new Vector3(0, 130, 0), 0.20f, RotateMode.Fast).OnComplete(() => InflictFullDamageToPlayer()));
    }

    [Button]
    private void CheckImpulseDirection()
    {
        Debug.DrawRay(transform.position, _impulseDirection.normalized * 5, Color.red, 10);
    }
}
