using System;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace Game
{
	public class Trap : MonoBehaviour, ITrap
	{
		public event Action onTrapDamage;

		[SerializeField] private PlayerReference m_playerReference;

		public GameObject Model => m_model;
		[SerializeField] private GameObject m_model;

		public Vector3 DefaultModelScale { get; private set; }

		[Header("Settings")] 
		private int scoreGain;

		protected void Awake()
		{
			DefaultModelScale = m_model.transform.localScale;
		}

		protected virtual void Start ()
		{
			Init();
		}

		protected void Init ()
		{

		}

		public void Activate ()
		{
			

			onTrapDamage?.Invoke();
		}

		public void Reset ()
		{
			Init();
		}

		protected void InflictFullDamageToPlayer()
		{
			m_playerReference.Instance.Health.TakeDamage(scoreGain);
		}

		/// <summary>
		/// Call this function to inflict the full damage split on different tick.
		/// </summary>
		/// <param name="_damageSplit"></param>
		protected void InflictSplitDamageToPlayer (int _damageSplit)
		{
			m_playerReference.Instance.Health.TakeDamage(scoreGain / _damageSplit);
		}
	}
}

