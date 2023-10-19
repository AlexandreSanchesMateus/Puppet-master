using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private ScoreReference _reference;
        [SerializeField] private TextMeshProUGUI _scoreTxt;

	    public event UnityAction OnIncreasingScore { add => _onScoreIncrease.AddListener(value); remove => _onScoreIncrease.RemoveListener(value); }
        [SerializeField, Foldout("Events")] private UnityEvent _onScoreIncrease;

        public float score { get; private set; }


        private void Start()
        {
            ((ISet<ScoreManager>)_reference).Set(this);
        }

        public void AddScore(int amount)
        {
            if (amount <= 0) return;

            score += amount;
            _scoreTxt.text = string.Format(score.ToString(), "D5");
            _onScoreIncrease?.Invoke();
        }
    }
}
