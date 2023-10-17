using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seringue_Object : Trap
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            InflictFullDamageToPlayer();
        }
    }
}
