using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Seringue_Object : Trap
{
    public UnityEvent OnActivate;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            InflictFullDamageToPlayer();
            OnActivate?.Invoke();
        }
    }
}
