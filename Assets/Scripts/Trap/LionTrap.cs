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
    public UnityEvent OnActivate;
    [SerializeField] private float _impulseForce = 20;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Activate(other.GetComponent<Rigidbody>());
        }

    }
    [Button]
    private void Activate(Rigidbody rb )
    {
        _camera.gameObject.SetActive(true);
        OnActivate.Invoke();
        Debug.Log("activate");
        InflictFullDamageToPlayer();
        StartCoroutine(launchPlayer());
        IEnumerator launchPlayer()
        {
            yield return new WaitForSeconds(5f);
            rb.AddForce(transform.forward * _impulseForce, ForceMode.Impulse);
            _camera.gameObject.SetActive(false);
        }
    }
    
}
