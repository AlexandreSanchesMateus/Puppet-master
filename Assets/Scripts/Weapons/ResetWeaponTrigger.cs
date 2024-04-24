using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetWeaponTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Weapons>(out Weapons script) && !script.m_isWield)
        {
            script.ResetToOriginPosition();
        }
        else if (other.TryGetComponent<Seringue_Object>(out Seringue_Object seringe) && !seringe.is_wield)
        {
            seringe.ResetToOriginPosition();
        }
    }
}
