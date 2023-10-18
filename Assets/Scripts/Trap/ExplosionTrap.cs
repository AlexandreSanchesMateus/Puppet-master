using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Game;

public class EnclumeTrap : Trap 
{
    [SerializeField] private float _activeRadius;
    [SerializeField] private float _explosionDelay;

    [SerializeField, Foldout("Events")] private UnityEvent _onTrapActivate;
    [SerializeField, Foldout("Events")] private UnityEvent _onTrapExplode;

    private void Update()
    {
        if((m_playerReference.Instance.transform.position - transform.position).magnitude <= _activeRadius)
        {
            Activate();
        }
    }


}
