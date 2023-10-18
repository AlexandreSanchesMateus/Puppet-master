using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private ScoreReference _reference;

	    public event UnityAction OnIncreasingScore { add => _onScoreIncrease.AddListener(value); remove => _onScoreIncrease.RemoveListener(value); }
        [SerializeField, Foldout("Events")] private UnityEvent _onScoreIncrease;

        public float score { get; private set; }


        private void Start()
        {
            ((ISet<Health>)_reference).Set(this);
        }

        public void AddScore(int amount)
        {
            if (amount <= 0) return;

            score += amount;
            _onScoreIncrease?.Invoke();
        }
    }
}
