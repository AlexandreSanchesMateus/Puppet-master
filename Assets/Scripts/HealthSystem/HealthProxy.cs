using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HealthProxy : MonoBehaviour, IDamageable
    {
        [SerializeField, Required] Health _target;

        private float m_minSpeedForDamage = 0.1f;
        [SerializeField] private float m_damageMultiply = 1f;

		private Rigidbody m_playerRigidbody => _target.Entity.Rigidbody;

        public void TakeDamage(int amount) => _target.TakeDamage(amount);

		private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("OnCollisionEnter");
        }

		private void OnTriggerEnter(Collider _collider)
		{
			if (_collider.gameObject.layer != 6)
				return;

            if (TryGetComponent(out Rigidbody rb))
            {
                OnCollideWithRigidbody(rb);
            }
			else
			{
                OnCollideWithCollider(_collider);
            }

            /*float thisVelocity = m_playerRigidbody.velocity.magnitude;
			float otherVelocity = 0;

			Rigidbody _rb = null;
            if (TryGetComponent(out _rb))
			{
				otherVelocity = _rb.velocity.magnitude;
			}

			float scoreCalculation = (thisVelocity + otherVelocity) * 10;

			Debug.Log($"{thisVelocity} + {otherVelocity} = {scoreCalculation}");

			Score = scoreCalculation;*/

            /*if (TryGetComponent(out Rigidbody _rb))
            {
                
            }*/
        }

		private void OnCollideWithRigidbody(Rigidbody _rigidbody)
		{
            float thisVelocity = m_playerRigidbody.velocity.magnitude;
            float otherVelocity = _rigidbody.velocity.magnitude;

            _target.score += CalculateDamageBaseOnSpeed(thisVelocity, "OnCollideWithRigidbody 1");
			_target.score += CalculateDamageBaseOnSpeed(otherVelocity, "OnCollideWithRigidbody 2");
        }

        private void OnCollideWithCollider(Collider _collider)
        {
            float relativeSpeed = m_playerRigidbody.velocity.magnitude;

			_target.score += CalculateDamageBaseOnSpeed(relativeSpeed, "OnCollideWithCollider");
        }

        private float CalculateDamageBaseOnSpeed(float _relativeSpeed, string _name)
        {
            if (_relativeSpeed <= m_minSpeedForDamage)
                return 0;

            float damages = _relativeSpeed * m_damageMultiply;

            Debug.Log(_name + damages);

            return damages;
        }
    }
}
