using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HealthProxy : MonoBehaviour, IDamageable
    {
        [SerializeField, Required] Health _target;

        public void TakeDamage(int amount) => _target.TakeDamage(amount);

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("OnCollisionEnter");
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("OnTriggerEnter");
        }
    }
}
