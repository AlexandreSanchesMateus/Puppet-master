using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class ScoreProxy : MonoBehaviour, IDamageable
    {
        [SerializeField, BoxGroup("Setup"), Required] ScoreReference _target;
        [SerializeField, BoxGroup("Setup")] private Rigidbody m_limbRigidbody;

        [SerializeField, BoxGroup("settings")] private float m_damageMultiply = 1f;
        [SerializeField, BoxGroup("settings")] private int m_minDamage = 1;
        [SerializeField, BoxGroup("settings")] private int m_maxDamage = 1;

        [SerializeField, Foldout("Events")] private UnityEvent m_onTakeDamage;

        public void TakeDamage(int amount)
        {
            _target.Instance.AddScore(amount);
            m_onTakeDamage?.Invoke();
        }

		private void OnCollisionEnter(Collision _collider)
		{
            if (_collider.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                OnCollideWithRigidbody(rb);
            }
			else
			{
                OnCollideWithCollider(_collider.collider);
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
            float thisVelocity = m_limbRigidbody.velocity.magnitude;
            float otherVelocity = _rigidbody.velocity.magnitude;

            _target.Instance.AddScore(CalculateDamageBaseOnSpeed(thisVelocity, "OnCollideWithRigidbody 1"));
			_target.Instance.AddScore(CalculateDamageBaseOnSpeed(otherVelocity, "OnCollideWithRigidbody 2"));
        }

        private void OnCollideWithCollider(Collider _collider)
        {
            float relativeSpeed = m_limbRigidbody.velocity.magnitude;
			_target.Instance.AddScore(CalculateDamageBaseOnSpeed(relativeSpeed, "OnCollideWithCollider"));
        }

        private int CalculateDamageBaseOnSpeed(float _relativeSpeed, string _name)
        {
            int damages = Mathf.RoundToInt(_relativeSpeed * m_damageMultiply);

            if (damages <= m_minDamage)
                return 0;

            return Mathf.Min(damages, m_maxDamage);
        }
    }
}
