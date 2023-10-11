using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HealthProxy : MonoBehaviour, IDamageable
    {
        [SerializeField, Required] Health _target;

        [SerializeField] private float m_minSpeedForDamage = 5f;
        [SerializeField] private float m_damageMultiply = 1f;

        public void TakeDamage(int amount) => _target.TakeDamage(amount);

		private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("OnCollisionEnter");
        }

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer != 6)
				return;

			float thisVelocity = _target.Entity.Rigidbody.velocity.magnitude;
			float otherVelocity = 0;

			if (TryGetComponent(out Rigidbody _rb))
			{
				otherVelocity = _rb.velocity.magnitude;
			}

			float scoreCalculation = (thisVelocity + otherVelocity) * 10;

			Debug.Log($"{thisVelocity} + {otherVelocity} = {scoreCalculation}");

			Score = scoreCalculation;

			Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();

			if (otherRigidbody)
			{
				OnCollideWithRigidbody(otherRigidbody);
			}
		}

		private void OnCollideWithRigidbody(Rigidbody _rigidbody)
		{
			float relativeSpeed = _rigidbody.velocity.magnitude;

			if (relativeSpeed >= m_minSpeedForDamage)
			{
				float dommages = relativeSpeed * m_damageMultiply;

				// Vous pouvez ici déduire les dégâts de la santé du joueur
				// Par exemple : joueur.SoustraireSante(dommages);
				Debug.Log("Dommages infligés : " + dommages);
			}
		}
	}
}
