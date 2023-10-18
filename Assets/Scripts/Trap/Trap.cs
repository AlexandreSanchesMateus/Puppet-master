using System;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace Game
{
	public abstract class  Trap : MonoBehaviour, ITrap
	{
		public event Action onTrapDamage;

		[SerializeField] protected PlayerReference m_playerReference;

		public GameObject Model => m_model;
		[SerializeField] private GameObject m_model;

		public Vector3 DefaultModelScale { get; private set; }

		[Header("Settings")]
		[SerializeField] private int m_scoreGain;

		protected void Awake()
		{
			DefaultModelScale = m_model.transform.localScale;
		}

		protected virtual void Start ()
		{
			Init();
		}

		protected virtual void Init ()
		{

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
		}

		/// <summary>
		/// Call this function to inflict the full damage split on different tick.
		/// </summary>
		/// <param name="_damageSplit"></param>
		protected void InflictSplitDamageToPlayer (int _damageSplit)
		{
			m_playerReference.Instance.Health.TakeDamage(m_scoreGain / _damageSplit);
		}
	}
}

