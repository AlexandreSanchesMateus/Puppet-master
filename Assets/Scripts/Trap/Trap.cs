using System;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace Game
{
	public abstract class  Trap : MonoBehaviour, ITrap
	{
		public event Action onTrapDamage;

		[SerializeField] private PlayerReference m_playerReference;

		public GameObject Model => m_model;
		[SerializeField] private GameObject m_model;

		public Vector3 DefaultModelScale { get; private set; }
		protected bool m_isTrapActive;

		[SerializeField, BoxGroup("Settings")] protected int m_scoreGain;
		[SerializeField, BoxGroup("Settings")] protected int m_scoreLeftToGain;

		protected virtual void Awake()
		{
			DefaultModelScale = m_model.transform.localScale;
		}

		protected virtual void Start ()
		{
			Init();
		}

		protected virtual void Init ()
		{
			m_scoreLeftToGain = m_scoreGain;
		}

		public virtual void Activate ()
		{
			

			onTrapDamage?.Invoke();
		}

		public virtual void ResetTrap ()
		{
			Init();
		}

		protected void InflictFullDamageToPlayer()
		{
			m_playerReference.Instance.Health.TakeDamage(m_scoreGain);

			m_scoreLeftToGain -= m_scoreGain;
		}

		/// <summary>
		/// Call this function to inflict the full damage split on different tick.
		/// </summary>
		/// <param name="_damageSplitBy"></param>
		protected void InflictSplitDamageToPlayer (int _damageSplitBy)
		{
			float damageToDo = (float)m_scoreGain / _damageSplitBy;

			if (damageToDo > m_scoreLeftToGain)
			{
				m_playerReference.Instance.Health.TakeDamage(m_scoreLeftToGain);
				m_scoreLeftToGain -= m_scoreLeftToGain;
			}
			else
			{
				m_playerReference.Instance.Health.TakeDamage((int)damageToDo);
				m_scoreLeftToGain -= (int)damageToDo;
			}
		}
	}
}

