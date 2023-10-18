using System;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using NaughtyAttributes;

namespace Game
{
	public abstract class  Trap : MonoBehaviour, ITrap
	{
		public event UnityAction OnTrapActivate { add => m_onTrapActivate.AddListener(value); remove => m_onTrapActivate.RemoveListener(value); }
		[SerializeField, Foldout("Events")] private UnityEvent m_onTrapActivate;

		[SerializeField, BoxGroup("Dependencies")] protected ScoreReference m_scoreReference;

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
			m_onTrapActivate?.Invoke();
		}

		public virtual void ResetTrap ()
		{
			Init();
		}

		protected void InflictFullDamageToPlayer()
		{
			m_scoreReference.Instance.AddScore(m_scoreGain);
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
				m_scoreReference.Instance.AddScore(m_scoreLeftToGain);
				m_scoreLeftToGain -= m_scoreLeftToGain;
			}
			else
			{
				m_scoreReference.Instance.AddScore((int)damageToDo);
				m_scoreLeftToGain -= (int)damageToDo;
			}
		}
	}
}
