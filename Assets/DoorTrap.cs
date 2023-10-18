using DG.Tweening;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorTrap : Trap
{
    public UnityEvent OnActivate;
    public UnityEvent OnDoorClose;
    [SerializeField] private Transform pivot1, pivot2;
    [SerializeField] private float _impulseForce=20;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Activate();
            other.GetComponent<Rigidbody>().AddForce(transform.forward * _impulseForce, ForceMode.Impulse);
        }
    }
    private void Activate()
    {
        OnActivate.Invoke();
        OnDoorClose.Invoke();
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(pivot1.DOLocalRotate(new Vector3(0, 0, 0), 0.20f, RotateMode.Fast));
        mySequence.Join(pivot2.DOLocalRotate(new Vector3(0, 0, 0), 0.20f, RotateMode.Fast).OnComplete(() => InflictFullDamageToPlayer()));
        
    }
}
