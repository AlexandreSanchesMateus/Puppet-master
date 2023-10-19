using Cinemachine;
using DG.Tweening;
using Game;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BearTrap : Trap
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Transform _firstPart, _secondPart;
    [SerializeField] private PuppetPhysic _physic;
    [SerializeField] private float _impulseForce = 20;
    [SerializeField] private List<Collider> _colliders = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (m_isTrapActive) return;

        Activate(other.GetComponent<Rigidbody>());
    }

    private void Activate(Rigidbody rb)
    {
        m_isTrapActive = true;
        _camera.gameObject.SetActive(true);
        _colliders.ForEach(c => c.enabled = true);

        _physic.SetPuppetPhysicToDisable();

        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.2f);
        mySequence.Append(_firstPart.DOLocalRotate(new Vector3(0, 0, 90),0.20f,RotateMode.Fast));
        mySequence.Join(_secondPart.DOLocalRotate(new Vector3(0, -180, 90), 0.20f, RotateMode.Fast).OnComplete(()=> m_onTrapActivate?.Invoke()).OnComplete(()=> InflictFullDamageToPlayer()));
        
        StartCoroutine(launchPlayer());

        IEnumerator launchPlayer()
        {
            yield return new WaitForSeconds(5f);
            rb.AddForce(transform.forward * _impulseForce, ForceMode.Impulse);
            _camera.gameObject.SetActive(false);
        }
    }
}
